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
        public int Vao, Vbo, Ebo;
        public Matrix4 Transform = Matrix4.Identity;
        public virtual void Draw()
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
    }
    /// <summary>
    /// class that used to render object that does not use lighting. 
    /// Accepts only vertex positions. Binds a single attribute (location = 0). 
    /// Is suitable for flat/unlit color rendering.
    /// </summary>
    public class SimpleObject3D: Object3D
    {

        public SimpleObject3D(float[] vertices, uint[] indices)
        {
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
        }

        public override void Draw()
        {
            GL.BindVertexArray(Vao);
            GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, 0); // assuming 36 indices
            GL.BindVertexArray(0);
        }
    }

    public class LitObject3D : Object3D
    {
        public LitObject3D(float[] vertexData, uint[] indices)
        {
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
        }

        public override void Draw()
        {
            GL.BindVertexArray(Vao);
            GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
    }

}
