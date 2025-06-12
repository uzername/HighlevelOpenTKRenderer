using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLevelOpenTKRenderLib
{
    public class MeshConstructor3D
    {
        public static void GetCubeMesh(in float cubeSide, out float[] cubeVertices, out uint[] cubeIndices)
        {
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
    }
}
