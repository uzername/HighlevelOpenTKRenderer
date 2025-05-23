using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLevelOpenTKRenderLib
{
    public class SimpleCamera  {
        public Vector3 Position = new Vector3(0, 0, 5);
        public Vector3 Target = Vector3.Zero;
        public Vector3 Up = Vector3.UnitY;

        public float Fov = MathHelper.DegreesToRadians(60);
        public float AspectRatio = 1.0f;
        public float NearClip = 0.1f;
        public float FarClip = 100f;

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Target, Up);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(Fov, AspectRatio, NearClip, FarClip);
        }
    }
}
