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

                        // First triangle
                        Vector3 normal1 = Vector3.Normalize(Vector3.Cross(p2 - p1, p3 - p1));
                        verts.AddRange(new float[] { p1.X, p1.Y, p1.Z, normal1.X, normal1.Y, normal1.Z });
                        verts.AddRange(new float[] { p2.X, p2.Y, p2.Z, normal1.X, normal1.Y, normal1.Z });
                        verts.AddRange(new float[] { p3.X, p3.Y, p3.Z, normal1.X, normal1.Y, normal1.Z });

                        uint baseIndex = (uint)verts.Count / 6 - 3;
                        inds.Add(baseIndex);
                        inds.Add(baseIndex + 1);
                        inds.Add(baseIndex + 2);

                        // Second triangle
                        Vector3 normal2 = Vector3.Normalize(Vector3.Cross(p4 - p2, p3 - p2));
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
    }
}
