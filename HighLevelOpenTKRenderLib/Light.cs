using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLevelOpenTKRenderLib
{
    /// <summary>
    /// phong shader - light source
    /// </summary>
    public class Light
    {
        public Vector3 Position;
        public Vector3 Color;
    }
    /// <summary>
    /// phong shader - material
    /// </summary>
    public class Material
    {
        public Vector3 DiffuseColor;
        public Vector3 SpecularColor;
        public float Shininess;
    }
}
