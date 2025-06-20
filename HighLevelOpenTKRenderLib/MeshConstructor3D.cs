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
    }
}
