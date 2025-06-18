using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLevelOpenTKRenderLib
{
    public class MeshConstructor3D
    {
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
