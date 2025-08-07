using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLevelOpenTKRenderLib
{
    /// <summary>
    /// class constructs some generic meshes that are zero-centered (mostly)
    /// </summary>
    public class MeshConstructor3D
    {
        /// <summary>
        /// Generate simple Grid Mesh that is rendered as lines. it is centered at zero during generation, it is translocated later
        /// </summary>
        /// <param name="sizeWidth">absolute size on width dimension</param>
        /// <param name="sizeDepth">absolute size on depth dimension</param>
        /// <param name="divisionsWidth">number of divisions on width dimension. min value is 1 - it is split in center</param>
        /// <param name="divisionsDepth">number of divisions on depth dimension. min value is 1 - it is split in center</param>
        /// <param name="gridVertices">output array - vertices</param>
        /// <param name="gridIndices">output array - indices</param>
        public static void GetSimpleGridMesh(in float sizeWidth, in float sizeDepth, in uint divisionsWidth, in uint divisionsDepth, out float[] gridVertices, out uint[] gridIndices)
        {
            // Ensure minimum of 1 division
            uint divW = Math.Max(1, divisionsWidth);
            uint divD = Math.Max(1, divisionsDepth);

            // Step size between lines
            float stepW = sizeWidth / divW;
            float stepD = sizeDepth / divD;

            // Starting positions
            float startX = -sizeWidth / 2f;
            float startZ = -sizeDepth / 2f;

            // We generate (divW + 1) lines along Z, and (divD + 1) lines along X
            int lineCount = (int)(divW + 1 + divD + 1);
            List<float> vertices = new List<float>();
            List<uint> indices = new List<uint>();

            uint index = 0;

            // Vertical lines (along Z)
            for (uint i = 0; i <= divW; i++)
            {
                float x = startX + i * stepW;
                float z1 = startZ;
                float z2 = startZ + sizeDepth;

                // From (x, 0, z1) to (x, 0, z2)
                vertices.Add(x); vertices.Add(0); vertices.Add(z1);
                vertices.Add(x); vertices.Add(0); vertices.Add(z2);

                indices.Add(index++);
                indices.Add(index++);
            }

            // Horizontal lines (along X)
            for (uint j = 0; j <= divD; j++)
            {
                float z = startZ + j * stepD;
                float x1 = startX;
                float x2 = startX + sizeWidth;

                // From (x1, 0, z) to (x2, 0, z)
                vertices.Add(x1); vertices.Add(0); vertices.Add(z);
                vertices.Add(x2); vertices.Add(0); vertices.Add(z);

                indices.Add(index++);
                indices.Add(index++);
            }

            gridVertices = vertices.ToArray();
            gridIndices = indices.ToArray();
        }
        public static void GetSingleLineMesh(in float xStart, in float yStart, in float zStart, in float xEnd, in float yEnd, in float zEnd, out float[] lineVertices, out uint[] lineIndices)
        {
            List<float> vertices = new List<float>();
            List<uint> indices = new List<uint>();

            vertices.Add(xStart); vertices.Add(yStart); vertices.Add(zStart);
            vertices.Add(xEnd); vertices.Add(yEnd); vertices.Add(zEnd);

            indices.Add(0);
            indices.Add(1);

            lineVertices = vertices.ToArray();
            lineIndices = indices.ToArray();
        }
        #region Mesh generator  Cube
        public static void GetCubeMesh(in float size, out float[] cubeVertices, out uint[] cubeIndices)
        {
            float cubeSide = size / 2f;
            cubeIndices = new uint[]   {
                0, 1, 5,  5, 1, 6,
                1, 2, 6,  6, 2, 7,
                2, 3, 7,  7, 3, 8,
                3, 4, 8,  8, 4, 9,
                10,11, 0,  0,11, 1,
                5, 6,12, 12, 6,13
            };
            cubeVertices = new float[] {
                   -cubeSide,-cubeSide,-cubeSide,
                    cubeSide,-cubeSide,-cubeSide,
                    cubeSide, cubeSide,-cubeSide,
                   -cubeSide, cubeSide,-cubeSide,
                   -cubeSide,-cubeSide,-cubeSide,
                   -cubeSide,-cubeSide, cubeSide,
                    cubeSide,-cubeSide, cubeSide,
                    cubeSide, cubeSide, cubeSide,
                   -cubeSide, cubeSide, cubeSide,
                   -cubeSide,-cubeSide, cubeSide,
                   -cubeSide, cubeSide,-cubeSide,
                    cubeSide, cubeSide,-cubeSide,
                   -cubeSide, cubeSide, cubeSide,
                    cubeSide, cubeSide, cubeSide,
            };
        }
        public static void GetCubeMeshWithNormals(float size, out float[] vertices, out uint[] indices)
        {
            float s = size / 2f;

            // 6 faces × 4 vertices per face = 24 unique vertices
            vertices = new float[]
            {
        // Front face (+Z)
        -s, -s,  s,   0f, 0f, 1f,
         s, -s,  s,   0f, 0f, 1f,
         s,  s,  s,   0f, 0f, 1f,
        -s,  s,  s,   0f, 0f, 1f,

        // Back face (-Z)
         s, -s, -s,   0f, 0f, -1f,
        -s, -s, -s,   0f, 0f, -1f,
        -s,  s, -s,   0f, 0f, -1f,
         s,  s, -s,   0f, 0f, -1f,

        // Left face (-X)
        -s, -s, -s,  -1f, 0f, 0f,
        -s, -s,  s,  -1f, 0f, 0f,
        -s,  s,  s,  -1f, 0f, 0f,
        -s,  s, -s,  -1f, 0f, 0f,

        // Right face (+X)
         s, -s,  s,   1f, 0f, 0f,
         s, -s, -s,   1f, 0f, 0f,
         s,  s, -s,   1f, 0f, 0f,
         s,  s,  s,   1f, 0f, 0f,

        // Top face (+Y)
        -s,  s,  s,   0f, 1f, 0f,
         s,  s,  s,   0f, 1f, 0f,
         s,  s, -s,   0f, 1f, 0f,
        -s,  s, -s,   0f, 1f, 0f,

        // Bottom face (-Y)
        -s, -s, -s,   0f, -1f, 0f,
         s, -s, -s,   0f, -1f, 0f,
         s, -s,  s,   0f, -1f, 0f,
        -s, -s,  s,   0f, -1f, 0f,
            };

            indices = new uint[]
            {
        // Front
        0, 1, 2,  2, 3, 0,
        // Back
        4, 5, 6,  6, 7, 4,
        // Left
        8, 9,10, 10,11, 8,
        // Right
       12,13,14, 14,15,12,
        // Top
       16,17,18, 18,19,16,
        // Bottom
       20,21,22, 22,23,20
            };
        }
        /// <summary>
        /// get vertices and indices arrays for box mesh, with normals. Center of it lies in its geometrical center, in the very middle
        /// </summary>
        /// <param name="XDimSize">size in X direction</param>
        /// <param name="YDimSize">size in Y direction</param>
        /// <param name="ZDimSize">size in Z direction</param>
        /// <param name="vertices">array of vertices with normals</param>
        /// <param name="indices">array of indices</param>
        public static void GetBoxMeshWithNormals(float XDimSize, float YDimSize, float ZDimSize, out float[] vertices, out uint[] indices)
        {
            // Half dimensions
            float hx = XDimSize / 2f;
            float hy = YDimSize / 2f;
            float hz = ZDimSize / 2f;

            // Each face has 4 vertices and 2 triangles (6 indices)
            // Each vertex has 3 floats for position + 3 floats for normal = 6 floats

            List<float> verts = new List<float>();
            List<uint> inds = new List<uint>();
            uint index = 0;

            void AddFace(Vector3 normal, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
            {
                void AddVertex(Vector3 pos)
                {
                    verts.Add(pos.X); verts.Add(pos.Y); verts.Add(pos.Z);
                    verts.Add(normal.X); verts.Add(normal.Y); verts.Add(normal.Z);
                }

                AddVertex(v0);
                AddVertex(v1);
                AddVertex(v2);
                AddVertex(v3);

                // Triangle 1: v0, v1, v2
                inds.Add(index);
                inds.Add(index + 1);
                inds.Add(index + 2);
                // Triangle 2: v2, v3, v0
                inds.Add(index + 2);
                inds.Add(index + 3);
                inds.Add(index);

                index += 4;
            }

            // +Z Front face
            AddFace(new Vector3(0, 0, 1),
                new Vector3(-hx, -hy, hz),
                new Vector3(hx, -hy, hz),
                new Vector3(hx, hy, hz),
                new Vector3(-hx, hy, hz));

            // -Z Back face
            AddFace(new Vector3(0, 0, -1),
                new Vector3(hx, -hy, -hz),
                new Vector3(-hx, -hy, -hz),
                new Vector3(-hx, hy, -hz),
                new Vector3(hx, hy, -hz));

            // +X Right face
            AddFace(new Vector3(1, 0, 0),
                new Vector3(hx, -hy, hz),
                new Vector3(hx, -hy, -hz),
                new Vector3(hx, hy, -hz),
                new Vector3(hx, hy, hz));

            // -X Left face
            AddFace(new Vector3(-1, 0, 0),
                new Vector3(-hx, -hy, -hz),
                new Vector3(-hx, -hy, hz),
                new Vector3(-hx, hy, hz),
                new Vector3(-hx, hy, -hz));

            // +Y Top face
            AddFace(new Vector3(0, 1, 0),
                new Vector3(-hx, hy, hz),
                new Vector3(hx, hy, hz),
                new Vector3(hx, hy, -hz),
                new Vector3(-hx, hy, -hz));

            // -Y Bottom face
            AddFace(new Vector3(0, -1, 0),
                new Vector3(-hx, -hy, -hz),
                new Vector3(hx, -hy, -hz),
                new Vector3(hx, -hy, hz),
                new Vector3(-hx, -hy, hz));

            vertices = verts.ToArray();
            indices = inds.ToArray();
        }
        #endregion
        public static void GetCylinderMeshWithNormals(float diameter, float height, out float[] vertices, out uint[] indices, bool isSmoothed)
        {
            const int segments = 36; // resolution of the circle
            float radius = diameter / 2f;
            float halfHeight = height / 2f;

            List<float> verts = new List<float>();
            List<uint> inds = new List<uint>();
            uint index = 0;

            // Helper method to add a vertex (position + normal)
            void AddVertex(Vector3 pos, Vector3 normal)
            {
                verts.Add(pos.X); verts.Add(pos.Y); verts.Add(pos.Z);
                verts.Add(normal.X); verts.Add(normal.Y); verts.Add(normal.Z);
            }

            // --- SIDE WALL ---
            if (isSmoothed)
            {
                for (int i = 0; i <= segments; i++)
                {
                    float angle = i * MathF.PI * 2f / segments;
                    float x = MathF.Cos(angle);
                    float z = MathF.Sin(angle);
                    Vector3 normal = new Vector3(x, 0, z);

                    Vector3 top = new Vector3(x * radius, +halfHeight, z * radius);
                    Vector3 bottom = new Vector3(x * radius, -halfHeight, z * radius);

                    AddVertex(bottom, normal);
                    AddVertex(top, normal);
                }

                for (int i = 0; i < segments; i++)
                {
                    inds.Add(index + (uint)(i * 2));
                    inds.Add(index + (uint)(i * 2 + 1));
                    inds.Add(index + (uint)(i * 2 + 3));

                    inds.Add(index + (uint)(i * 2));
                    inds.Add(index + (uint)(i * 2 + 3));
                    inds.Add(index + (uint)(i * 2 + 2));
                }

                index += (uint)((segments + 1) * 2);
            }
            else
            {
                for (int i = 0; i < segments; i++)
                {
                    float angle1 = i * MathF.PI * 2f / segments;
                    float angle2 = (i + 1) * MathF.PI * 2f / segments;

                    float x1 = MathF.Cos(angle1), z1 = MathF.Sin(angle1);
                    float x2 = MathF.Cos(angle2), z2 = MathF.Sin(angle2);

                    Vector3 p0 = new Vector3(x1 * radius, -halfHeight, z1 * radius);
                    Vector3 p1 = new Vector3(x1 * radius, +halfHeight, z1 * radius);
                    Vector3 p2 = new Vector3(x2 * radius, +halfHeight, z2 * radius);
                    Vector3 p3 = new Vector3(x2 * radius, -halfHeight, z2 * radius);

                    Vector3 faceNormal = Vector3.Normalize(Vector3.Cross(p1 - p0, p3 - p0));

                    AddVertex(p0, faceNormal);
                    AddVertex(p1, faceNormal);
                    AddVertex(p2, faceNormal);
                    AddVertex(p3, faceNormal);

                    inds.Add(index);
                    inds.Add(index + 1);
                    inds.Add(index + 2);
                    inds.Add(index);
                    inds.Add(index + 2);
                    inds.Add(index + 3);

                    index += 4;
                }
            }

            // --- TOP CIRCLE ---
            Vector3 topCenter = new Vector3(0, +halfHeight, 0);
            AddVertex(topCenter, Vector3.UnitY);
            uint topCenterIndex = index++;
            for (int i = 0; i <= segments; i++)
            {
                float angle = i * MathF.PI * 2f / segments;
                float x = MathF.Cos(angle);
                float z = MathF.Sin(angle);
                Vector3 pos = new Vector3(x * radius, +halfHeight, z * radius);
                AddVertex(pos, Vector3.UnitY);

                if (i > 0)
                {
                    inds.Add(topCenterIndex);
                    inds.Add(topCenterIndex + (uint)i);
                    inds.Add(topCenterIndex + (uint)i - 1);
                }
            }

            index += (uint)(segments + 1);

            // --- BOTTOM CIRCLE ---
            Vector3 bottomCenter = new Vector3(0, -halfHeight, 0);
            AddVertex(bottomCenter, -Vector3.UnitY);
            uint bottomCenterIndex = index++;
            for (int i = 0; i <= segments; i++)
            {
                float angle = i * MathF.PI * 2f / segments;
                float x = MathF.Cos(angle);
                float z = MathF.Sin(angle);
                Vector3 pos = new Vector3(x * radius, -halfHeight, z * radius);
                AddVertex(pos, -Vector3.UnitY);

                if (i > 0)
                {
                    inds.Add(bottomCenterIndex);
                    inds.Add(bottomCenterIndex + (uint)i - 1);
                    inds.Add(bottomCenterIndex + (uint)i);
                }
            }

            vertices = verts.ToArray();
            indices = inds.ToArray();
        }
        /// <summary>
        /// Gets a mesh and normals for a cone to use with phong shader. Bottom cap - Always rendered as a fan of triangles, flat-shaded with downward normal.
        /// Coordinates: Base of the cone is centered at(0, -height/2), top is at(0, +height/2).
        /// </summary>
        /// <param name="diameter">diameter of bottom cap</param>
        /// <param name="height"> total height of cone </param>
        /// <param name="vertices"></param>
        /// <param name="indices"></param>
        /// <param name="isSmoothed"> isSmoothed=true : Normals are averaged to produce a smooth side surface. </param>
        public static void GetConeMeshWithNormals(float diameter, float height, out float[] vertices, out uint[] indices, bool isSmoothed)
        {
            const int segments = 36;
            float radius = diameter / 2f;
            float halfHeight = height / 2f;

            List<float> verts = new List<float>();
            List<uint> inds = new List<uint>();
            uint index = 0;

            Vector3 apex = new Vector3(0, halfHeight, 0);

            void AddVertex(Vector3 pos, Vector3 normal)
            {
                verts.Add(pos.X); verts.Add(pos.Y); verts.Add(pos.Z);
                verts.Add(normal.X); verts.Add(normal.Y); verts.Add(normal.Z);
            }

            // --- Side Wall ---
            if (isSmoothed)
            {
                for (int i = 0; i <= segments; i++)
                {
                    float angle = i * MathF.PI * 2f / segments;
                    float x = MathF.Cos(angle);
                    float z = MathF.Sin(angle);

                    Vector3 point = new Vector3(x * radius, -halfHeight, z * radius);
                    Vector3 toApex = Vector3.Normalize(apex - point);
                    Vector3 tangent = Vector3.Normalize(new Vector3(-z, 0, x)); // perpendicular on XZ circle
                    Vector3 normal = Vector3.Normalize(Vector3.Cross(tangent, toApex)); // normal lies between point and apex

                    AddVertex(point, normal);
                    AddVertex(apex, normal);
                }

                for (int i = 0; i < segments; i++)
                {
                    inds.Add(index + (uint)(i * 2));
                    inds.Add(index + (uint)(i * 2 + 1));
                    inds.Add(index + (uint)(i * 2 + 3));
                }

                index += (uint)((segments + 1) * 2);
            }
            else
            {
                for (int i = 0; i < segments; i++)
                {
                    float angle1 = i * MathF.PI * 2f / segments;
                    float angle2 = (i + 1) * MathF.PI * 2f / segments;

                    Vector3 p1 = new Vector3(MathF.Cos(angle1) * radius, -halfHeight, MathF.Sin(angle1) * radius);
                    Vector3 p2 = new Vector3(MathF.Cos(angle2) * radius, -halfHeight, MathF.Sin(angle2) * radius);

                    Vector3 faceNormal = Vector3.Normalize(Vector3.Cross(p2 - apex, p1 - apex));

                    AddVertex(p1, faceNormal);
                    AddVertex(p2, faceNormal);
                    AddVertex(apex, faceNormal);

                    inds.Add(index);
                    inds.Add(index + 1);
                    inds.Add(index + 2);

                    index += 3;
                }
            }

            // --- Bottom Cap ---
            Vector3 bottomCenter = new Vector3(0, -halfHeight, 0);
            AddVertex(bottomCenter, -Vector3.UnitY);
            uint centerIndex = index++;

            for (int i = 0; i <= segments; i++)
            {
                float angle = i * MathF.PI * 2f / segments;
                float x = MathF.Cos(angle);
                float z = MathF.Sin(angle);

                Vector3 pos = new Vector3(x * radius, -halfHeight, z * radius);
                AddVertex(pos, -Vector3.UnitY);

                if (i > 0)
                {
                    inds.Add(centerIndex);
                    inds.Add(centerIndex + (uint)i);
                    inds.Add(centerIndex + (uint)i - 1);
                }
            }

            vertices = verts.ToArray();
            indices = inds.ToArray();
        }
        #region Mesh generator  Sphere
        /// <summary>
        /// generate sphere with center in 0;0 with specified diameter and level of detail, include normals for rendering as a lit shape.
        /// ChatGPT generated this code for me, thanks a lot
        /// </summary>
        /// <param name="diameter">how large is a sphere</param>
        /// <param name="levelOfDetail">how detailed is sphere. 1 - for icosaedr, higher for more detailed shape</param>
        /// <param name="vertices">output array - vertices </param>
        /// <param name="indices">output array - indices</param>
        /// <param name="isSmoothed">if true then generate normals and indices so that shape is shown smoothed. If false then shape should not be shown smoothed</param>
        public static void getSphereMeshWithNormals(float diameter, byte levelOfDetail, out float[] vertices, out uint[] indices, bool isSmoothed)
        {

            float radius = diameter / 2f;
            int latitudeBands = 8 * levelOfDetail;
            int longitudeBands = 16 * levelOfDetail;

            List<float> verts = new();
            List<uint> inds = new();

            if (isSmoothed)
            {
                // Shared vertices with smooth normals
                for (int lat = 0; lat <= latitudeBands; lat++)
                {
                    float theta = lat * MathF.PI / latitudeBands;
                    float sinTheta = MathF.Sin(theta);
                    float cosTheta = MathF.Cos(theta);

                    for (int lon = 0; lon <= longitudeBands; lon++)
                    {
                        float phi = lon * 2 * MathF.PI / longitudeBands;
                        float sinPhi = MathF.Sin(phi);
                        float cosPhi = MathF.Cos(phi);

                        float x = cosPhi * sinTheta;
                        float y = cosTheta;
                        float z = sinPhi * sinTheta;

                        // Position
                        verts.Add(x * radius);
                        verts.Add(y * radius);
                        verts.Add(z * radius);

                        // Normal (same as position normalized)
                        verts.Add(x);
                        verts.Add(y);
                        verts.Add(z);
                    }
                }

                for (int lat = 0; lat < latitudeBands; lat++)
                {
                    for (int lon = 0; lon < longitudeBands; lon++)
                    {
                        uint first = (uint)(lat * (longitudeBands + 1) + lon);
                        uint second = first + (uint)(longitudeBands + 1);

                        inds.Add(first);
                        inds.Add(second);
                        inds.Add(first + 1);

                        inds.Add(second);
                        inds.Add(second + 1);
                        inds.Add(first + 1);
                    }
                }
            }
            else
            {
                // Flat shading: no shared vertices
                for (int lat = 0; lat < latitudeBands; lat++)
                {
                    float theta1 = lat * MathF.PI / latitudeBands;
                    float theta2 = (lat + 1) * MathF.PI / latitudeBands;

                    for (int lon = 0; lon < longitudeBands; lon++)
                    {
                        float phi1 = lon * 2 * MathF.PI / longitudeBands;
                        float phi2 = (lon + 1) * 2 * MathF.PI / longitudeBands;

                        // Generate 4 vertices of quad (2 triangles)
                        Vector3 p1 = new(
                            radius * MathF.Cos(phi1) * MathF.Sin(theta1),
                            radius * MathF.Cos(theta1),
                            radius * MathF.Sin(phi1) * MathF.Sin(theta1)
                        );
                        Vector3 p2 = new(
                            radius * MathF.Cos(phi1) * MathF.Sin(theta2),
                            radius * MathF.Cos(theta2),
                            radius * MathF.Sin(phi1) * MathF.Sin(theta2)
                        );
                        Vector3 p3 = new(
                            radius * MathF.Cos(phi2) * MathF.Sin(theta1),
                            radius * MathF.Cos(theta1),
                            radius * MathF.Sin(phi2) * MathF.Sin(theta1)
                        );
                        Vector3 p4 = new(
                            radius * MathF.Cos(phi2) * MathF.Sin(theta2),
                            radius * MathF.Cos(theta2),
                            radius * MathF.Sin(phi2) * MathF.Sin(theta2)
                        );

                        // First triangle. Normals are set for each point but they are the same
                        Vector3 normal1 = Vector3.Normalize(Vector3.Cross(p3 - p1, p2 - p1));
                        verts.AddRange(new float[] { p1.X, p1.Y, p1.Z, normal1.X, normal1.Y, normal1.Z });
                        verts.AddRange(new float[] { p2.X, p2.Y, p2.Z, normal1.X, normal1.Y, normal1.Z });
                        verts.AddRange(new float[] { p3.X, p3.Y, p3.Z, normal1.X, normal1.Y, normal1.Z });

                        uint baseIndex = (uint)verts.Count / 6 - 3;
                        inds.Add(baseIndex);
                        inds.Add(baseIndex + 1);
                        inds.Add(baseIndex + 2);

                        // Second triangle
                        Vector3 normal2 = Vector3.Normalize(Vector3.Cross(p3 - p2, p4 - p2));
                        /*
                         in case of abnormalities (if scalar multiplication is bigger than 0 then vector of view runs in similar direction as normal), flip normals:
                        if (Vector3.Dot(normal1, ViewTarget - Position) > 0)
                            normal1 = -normal1;
                         */
                        verts.AddRange(new float[] { p2.X, p2.Y, p2.Z, normal2.X, normal2.Y, normal2.Z });
                        verts.AddRange(new float[] { p4.X, p4.Y, p4.Z, normal2.X, normal2.Y, normal2.Z });
                        verts.AddRange(new float[] { p3.X, p3.Y, p3.Z, normal2.X, normal2.Y, normal2.Z });

                        baseIndex = (uint)verts.Count / 6 - 3;
                        inds.Add(baseIndex);
                        inds.Add(baseIndex + 1);
                        inds.Add(baseIndex + 2);
                    }
                }
            }
            vertices = verts.ToArray();
            indices = inds.ToArray();
        }
        /// <summary>
        /// get vertices and indices for sphere with specified diameter to render as unlit shape
        /// </summary>
        /// <param name="diameter"></param>
        /// <param name="vertices"></param>
        /// <param name="indices"></param>
        public static void getSphereMesh(float diameter, out float[] vertices, out uint[] indices)
        {
            byte levelOfDetail = 1;
            float radius = diameter / 2f;
            int latitudeBands = 8 * levelOfDetail;
            int longitudeBands = 16 * levelOfDetail;

            List<float> verts = new();
            List<uint> inds = new();

            for (int lat = 0; lat <= latitudeBands; lat++)
            {
                float theta = lat * MathF.PI / latitudeBands;
                float sinTheta = MathF.Sin(theta);
                float cosTheta = MathF.Cos(theta);

                for (int lon = 0; lon <= longitudeBands; lon++)
                {
                    float phi = lon * 2 * MathF.PI / longitudeBands;
                    float sinPhi = MathF.Sin(phi);
                    float cosPhi = MathF.Cos(phi);

                    float x = cosPhi * sinTheta;
                    float y = cosTheta;
                    float z = sinPhi * sinTheta;

                    // Position
                    verts.Add(x * radius);
                    verts.Add(y * radius);
                    verts.Add(z * radius);

                }
            }

            for (int lat = 0; lat < latitudeBands; lat++)
            {
                for (int lon = 0; lon < longitudeBands; lon++)
                {
                    uint first = (uint)(lat * (longitudeBands + 1) + lon);
                    uint second = first + (uint)(longitudeBands + 1);

                    inds.Add(first);
                    inds.Add(second);
                    inds.Add(first + 1);

                    inds.Add(second);
                    inds.Add(second + 1);
                    inds.Add(first + 1);
                }
            }
            vertices = verts.ToArray();
            indices = inds.ToArray();
        }
        #endregion
    
    }
}
