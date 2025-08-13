using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLevelOpenTKRenderLib
{
    public class Object3D
    {
        /// <summary>
        /// Vao - vertex array object, vbo - vertex buffer object, that contain vertex attribute and index data (the latter is sometimes called element data). 
        /// EBO also has index data. those are number - pointers to arrays that are stored in video memory
        /// </summary>
        public int Vao, Vbo, Ebo;
        // If Object3D might need picking, collision checks, or geometric-center rotations then it is required to store all the vertices
        // because obtaining them from VBO - video memory - is difficult
        public List<Vector3> Vertices; // Keep positions for CPU-side calculations
        /// <summary>
        /// identifier in scene collection
        /// </summary>
        public string UniqueName = "MyObject";
        /// <summary>
        /// if false then skipped from rendering
        /// </summary>
        public bool IsShown = true;
        /// <summary>
        /// Matrix  which holds transformations:
        /// [(r11 r12 r13 t1)
        /// (r21 r22 r23 t2)
        /// (r31 r32 r33 t3)
        /// (0   0   0   1 )]
        /// R is the 3×3 rotation & scale part.
        /// T is the translation vector(object’s world position).
        /// </summary>
        public Matrix4 Transform = Matrix4.Identity;
        /* for example A standard triangulated cube has: 6 faces. Each face is made of 2 triangles. Each triangle has 3 vertices */
        /// <summary>
        /// how to draw this Object3D: either lines or filled triangles. Lines are best used for MeshConstructor3D::GetSimpleGridMesh()
        /// </summary>
        public bool DrawTriangles =true;
        protected int indicesCount;
        public (Vector3 min, Vector3 max) GetBoundBox()
        {
            if ((Vertices==null)||(Vertices.Count < 2))
            {
                throw new Exception("Vertices does not contain enough data for bound box (min 2)");
            }
            Vector3 min = new Vector3( Vertices[0] );
            Vector3 max = new Vector3( Vertices[0] );

            foreach (var v in Vertices)
            {
                min = Vector3.ComponentMin(min, v);
                max = Vector3.ComponentMax(max, v);
            }
            return (min, max);
        }
        // redefined in subclasses 
        public virtual void Draw()
        {

        }
        /// <summary>
        /// assign Vertices array
        /// </summary>
        /// <param name="vertices">vertices data supplied to constructor and subsequently to VBO from Mesh Generator</param>
        public virtual void assignVertices(float[] verticesData)
        {

        }
        /// <summary>
        /// translate object3d by offsets
        /// </summary>
        /// <param name="X">x offset</param>
        /// <param name="Y">y offset</param>
        /// <param name="Z">z offset</param>
        public void MoveBy(float X, float Y, float Z, bool useLocalAxis=false)  {
            var translation = Matrix4.CreateTranslation(X, Y, Z);
            if (useLocalAxis==false)
            { // move in world
                Transform = Transform * translation;
            } else
            { // move locally
                Transform = translation * Transform;
            }
        }
        public void MoveTo(float X, float Y, float Z)
        {
            // Extract current translation vector to isolate rotation/scale
            Vector3 newPosition = new Vector3(X, Y, Z);

            // Keep rotation and scale
            Vector3 right = new Vector3(Transform.M11, Transform.M12, Transform.M13);
            Vector3 up = new Vector3(Transform.M21, Transform.M22, Transform.M23);
            Vector3 forward = new Vector3(Transform.M31, Transform.M32, Transform.M33);

            Transform = new Matrix4(
                new Vector4(right, 0),
                new Vector4(up, 0),
                new Vector4(forward, 0),
                new Vector4(newPosition, 1)
            );
        }
        /// <summary>
        /// rotate around local axes that run through zero point - origin that was used during creation of geometry in MeshConstructor3D.
        /// If the geometry is authored with its pivot at (0,0,0), rotations happen naturally around that pivot.
        /// To truly rotate around geometric center when pivot is not center, you must temporarily translate by -center, rotate, then translate back. But that’s when you do need to calculate the center.
        /// </summary>
        /// <param name="XLocal">do use local x axis for rotation</param>
        /// <param name="YLocal">do use local y axis for rotation</param>
        /// <param name="ZLocal">do use local z axis for rotation</param>
        /// <param name="degrees">value in degrees</param>
        public void RotateAroundLocal(bool XLocal, bool YLocal, bool ZLocal, float degrees)
        {
            Matrix4 rotation = Matrix4.Identity;

            if (XLocal)
                rotation *= Matrix4.CreateFromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians(degrees));
            if (YLocal)
                rotation *= Matrix4.CreateFromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(degrees));
            if (ZLocal)
                rotation *= Matrix4.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(degrees));
            /* When rotating around local axes, you post-multiply the existing Transform by a rotation matrix in local space. */
            // Local rotation : multiply on the RIGHT
            Transform = Transform* rotation;
        }
        /// <summary>
        /// rotate around global axes by degrees
        /// </summary>
        /// <param name="XGlobal">do use global x axis</param>
        /// <param name="YGlobal">do use global y axis</param>
        /// <param name="ZGlobal">do use global z axis</param>
        /// <param name="degrees"></param>
        public void RotateAroundGlobal(bool XGlobal, bool YGlobal, bool ZGlobal, float degrees)
        {
            Matrix4 rotation = Matrix4.Identity;

            if (XGlobal)
                rotation *= Matrix4.CreateFromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians(degrees));
            if (YGlobal)
                rotation *= Matrix4.CreateFromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(degrees));
            if (ZGlobal)
                rotation *= Matrix4.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(degrees));

            // Global rotation : multiply on the LEFT
            Transform = rotation * Transform;
        }

        public void MirrorAroundCoordinatePlane(bool useXYplane, bool useYZplane, bool useXZplane, bool globalScale)
        {
            /*
             Mirroring flips the triangle winding order, so if you use back-face culling, your object may disappear 
             or show its inside faces after mirroring.
             Fix: Either disable culling or reverse the winding order (can be done by multiplying with an 
             additional rotation of 180 degrees around the flipped axis, or changing vertex order).
             */
            // Mirroring across a coordinate plane is just scaling one axis by -1 while keeping the others at 1:
            // Start with uniform scale
            Vector3 scale = Vector3.One;

            if (useXYplane) scale.Z *= -1; // Mirror across XY : flip Z
            if (useYZplane) scale.X *= -1; // Mirror across YZ : flip X
            if (useXZplane) scale.Y *= -1; // Mirror across XZ : flip Y
            // scaling by a negative value in one axis inverts coordinates on that axis, which is mathematically equivalent to mirroring.
            // Apply mirror scale to current transform
            if (globalScale)
                Transform = Matrix4.CreateScale(scale) * Transform;
            else
                Transform = Transform * Matrix4.CreateScale(scale);
        }
        /// <summary>
        /// create reflection matrix around a plane defined by 3 points
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        /// <returns></returns>
        public static Matrix4 CreateReflection(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            // Compute plane normal
            Vector3 v1 = point2 - point1;
            Vector3 v2 = point3 - point1;
            Vector3 n = Vector3.Normalize(Vector3.Cross(v1, v2));

            float d = Vector3.Dot(n, point1); // plane distance from origin

            float nx = n.X, ny = n.Y, nz = n.Z;

            return new Matrix4(
                1 - 2 * nx * nx, -2 * nx * ny, -2 * nx * nz, -2 * d * nx,
                -2 * ny * nx, 1 - 2 * ny * ny, -2 * ny * nz, -2 * d * ny,
                -2 * nz * nx, -2 * nz * ny, 1 - 2 * nz * nz, -2 * d * nz,
                0, 0, 0, 1
            );
        }
        public void MirrorAroundArbitraryPlane(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Transform = CreateReflection(p1, p2, p3) * Transform;
        }

    }
    /// <summary>
    /// class that used to render object that does not use lighting. 
    /// Accepts only vertex positions. Binds a single attribute (location = 0). 
    /// Is suitable for flat/unlit color rendering.
    /// </summary>
    public class SimpleObject3D: Object3D
    {
        public Vector4 SimpleColor = new Vector4(1.0f,1.0f,1.0f,1.0f);
        public SimpleObject3D(float[] vertices, uint[] indices)
        {
            indicesCount = indices.Length;
            Vao = GL.GenVertexArray();
            Vbo = GL.GenBuffer();
            Ebo = GL.GenBuffer();

            GL.BindVertexArray(Vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // Assume vertices are: vec3 position
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindVertexArray(0);

            assignVertices(vertices);
        }
        /// <summary>
        /// fill up Vertices array. SimpleObject3D does 
        /// </summary>
        /// <param name="verticesData"></param>
        public override void assignVertices(float[] verticesData)
        {
            if (verticesData.Length % 3 != 0) { throw new ArgumentException("Cannot assignVertices in SimpleObject3D because some coordinates are missing"); }
            Vertices = new List<Vector3>();
            for (int i = 0; i < verticesData.Length; i+=3) {
                Vertices.Add(new Vector3(verticesData[i], verticesData[i + 1], verticesData[i + 2]));
            }
        }
        public override void Draw()
        {
            GL.BindVertexArray(Vao);
            if (DrawTriangles)  {
                GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);
            } else {
                GL.DrawElements(PrimitiveType.Lines, indicesCount, DrawElementsType.UnsignedInt, 0);
            }
                GL.BindVertexArray(0);
        }
    }

    public class ThickLineObject3D : SimpleObject3D
    {
        /// <summary>
        /// thickness of line in pixels
        /// </summary>
        public float thicknessLine;
        public ThickLineObject3D(float[] vertices, uint[] indices) : base(vertices, indices)
        {
        }
        public override void Draw()
        {
            GL.BindVertexArray(Vao);
            GL.DrawElements(PrimitiveType.Lines, indicesCount, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
    }

    public class LitObject3D : Object3D
    {
        public Material LitMaterial = new Material();
        public LitObject3D(float[] vertexData, uint[] indices)
        {
            indicesCount = indices.Length;
            Vao = GL.GenVertexArray();
            Vbo = GL.GenBuffer();
            Ebo = GL.GenBuffer();

            GL.BindVertexArray(Vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            int stride = 6 * sizeof(float); // 3 for pos, 3 for normal

            // Position -> location = 0
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);

            // Normal -> location = 1
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));

            GL.BindVertexArray(0);

            assignVertices(vertexData);
        }
        public override void assignVertices(float[] verticesData)
        {
            // don't forget to skip normals every 3 coordinates
            if (verticesData.Length % 3 != 0) { throw new ArgumentException("Cannot assignVertices in LitObject3D because some coordinates are missing"); }
            Vertices = new List<Vector3>();
            for (int i = 0; i < verticesData.Length; i += 6)
            {
                Vertices.Add(new Vector3(verticesData[i], verticesData[i + 1], verticesData[i + 2]));
            }
        }
        public override void Draw()
        {
            GL.BindVertexArray(Vao);
            if (DrawTriangles)  {
                GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);
            }
            else {
                GL.DrawElements(PrimitiveType.Lines, indicesCount, DrawElementsType.UnsignedInt, 0);
            }
                GL.BindVertexArray(0);
        }
    }

}
