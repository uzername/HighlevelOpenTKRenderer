using HighLevelOpenTKRenderLib;
using Microsoft.VisualBasic.Devices;
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
        public float sensitivity = 0.08f;
        
        public bool firstMove = true;
        private Vector2 _lastPos;

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

        public void ProcessMouseInputLook(int mouseX, int mouseY)
        {
            if (firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouseX, mouseY);
                firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouseX - _lastPos.X;
                var deltaY = mouseY - _lastPos.Y;
                _lastPos = new Vector2(mouseX, mouseY);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                this.Yaw += deltaX * sensitivity;
                this.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }
        }
    }
}
