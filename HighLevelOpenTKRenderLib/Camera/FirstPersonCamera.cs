using HighLevelOpenTKRenderLib;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLevelOpenTKRenderLib
{
    public class FirstPersonCamera: CameraMk2, ICameraControl
    {
        public float cameraSpeed = 0.15f;
        public float sensitivity = 0.02f;

        public FirstPersonCamera(Vector3 position, float aspectRatio, float fieldOfView) : base(position, aspectRatio, fieldOfView)
        {
        }
        public void MoveForward()
        {
            this.Position += this.Front * cameraSpeed;
        }

        public void MoveBackward()
        {
            this.Position -= this.Front * cameraSpeed;
        }

        public void MoveDown()
        {
            this.Position -= this.Up * cameraSpeed;
        }

        
        public void MoveLeft()
        {
            this.Position -= this.Right * cameraSpeed;
        }

        public void MoveRight()
        {
            this.Position += this.Right * cameraSpeed;
        }

        public void MoveUp()
        {
            this.Position += this.Up * cameraSpeed;
        }

        public void ProcessMouseInputLook(int cX, int cY)
        {
            throw new NotImplementedException();
        }
    }
}
