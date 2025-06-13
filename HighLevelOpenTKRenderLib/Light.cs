using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLevelOpenTKRenderLib
{
    public class Light
    {
        public Vector3 Position;
        public Vector3 Color;
    }
    public class Material
    {
        public Vector3 DiffuseColor;
        public Vector3 SpecularColor;
        public float Shininess;
    }
}
