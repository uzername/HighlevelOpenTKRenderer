using Microsoft.VisualBasic.Devices;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HighLevelOpenTKRenderLib
{
    public class OrbitingCamera : CameraMk2, ICameraControl
    {
        public float cameraSpeed = 0.15f;
        public float sensitivity = 0.08f;

        public bool firstMove = true;
        private Vector2 _lastPos;
        // View target of camera. when this point moves then camera moves together with it, they are bound
        public Vector3 ViewTarget;
        private float distanceToTarget = 5f;
        /// <summary>
        /// Initializes camera from position. 
        /// Sets ViewTarget based on Front and distanceToTarget. 
        ///  Good for compatibility with existing camera code but better not to use this
        /// </summary>
        /// <param name="position">point where camera is expected to appear</param>
        /// <param name="aspectRatio"></param>
        /// <param name="fieldOfView">angle field of view in DEGREES, it will be converted later. Best value is 60</param>
        public OrbitingCamera(Vector3 position, float aspectRatio, float fieldOfView) 
            : base(position, aspectRatio, fieldOfView)   {
            Yaw = 90f;
            ViewTarget = Position + Front * distanceToTarget;
            UpdateCameraPosition();
        }
        /// <summary>
        /// Initializes camera from view target  and distance to it.
        /// BETTER USE THIS. More precise, ideal for orbit control . Avoids ambiguity . 
        /// </summary>
        /// <param name="viewTarget">point at which camera is looking</param>
        /// <param name="distanceToTarget">distance from camera to point at which camera is looking</param>
        /// <param name="aspectRatio"></param>
        /// <param name="fieldOfView">angle field of view in DEGREES, it will be converted later. Best value is 60</param>
        public OrbitingCamera(Vector3 viewTarget, float distanceToTarget, float aspectRatio, float fieldOfView)
    : base(Vector3.Zero, aspectRatio, fieldOfView) // Position is calculated later
        {
            this.ViewTarget = viewTarget;
            this.distanceToTarget = distanceToTarget;

            // Set default orientation
            Yaw = 90f;
            Pitch = 0f;

            UpdateCameraPosition(); // Derives Position based on ViewTarget and distance
        }
        /// <summary>
        /// Most detailed and mathematically correct initializer of Orbiting camera
        /// </summary>
        /// <param name="viewTarget"></param>
        /// <param name="distanceToTarget"></param>
        /// <param name="yawDegrees">Yaw (horizontal angle). May be any number. Yaw = 90deg is +Z, 180deg is –X, 270deg is –Z (and so on) </param>
        /// <param name="pitchDegrees">Pitch (vertical angle). Range -89 degrees to +89 degrees. Pitch +90 or -90 may look camera look up or down straight which causes div by zero and bad cross product and bad Right vector of camera (Front and Up vectors become parallel)</param>
        /// <param name="aspectRatio"></param>
        /// <param name="fieldOfView"></param>
        public OrbitingCamera(Vector3 viewTarget, float distanceToTarget, float yawDegrees, float pitchDegrees, float aspectRatio, float fieldOfView)
    : base(Vector3.Zero, aspectRatio, fieldOfView)
        {
            ViewTarget = viewTarget;
            distanceToTarget = MathF.Max(distanceToTarget, 0.001f); // avoid zero-distance

            Yaw = yawDegrees;
            Pitch = MathHelper.Clamp(pitchDegrees, -89f, 89f); // enforce safe pitch

            UpdateCameraPosition();
        }

        private void UpdateCameraPosition()
        {
            // Convert yaw and pitch to radians
            float yawRad = MathHelper.DegreesToRadians(Yaw);
            float pitchRad = MathHelper.DegreesToRadians(Pitch);

            // Spherical to Cartesian conversion
            Position = new Vector3(
                ViewTarget.X + distanceToTarget * MathF.Cos(pitchRad) * MathF.Cos(yawRad),
                ViewTarget.Y + distanceToTarget * MathF.Sin(pitchRad),
                ViewTarget.Z + distanceToTarget * MathF.Cos(pitchRad) * MathF.Sin(yawRad)
            );

            // Update Front vector to look at ViewTarget
            _front = Vector3.Normalize(ViewTarget - Position);

            // Recalculate Right and Up
            _right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }

        public void MoveBackward()
        {
            // do move ViewTarget point backward
            ViewTarget -= Front * cameraSpeed;
            UpdateCameraPosition();
        }

        public void MoveDown()
        {
            // do move ViewTarget point down
            ViewTarget -= Up * cameraSpeed;
            UpdateCameraPosition();
        }

        public void MoveForward()
        {
            // do move ViewTarget point forward
            ViewTarget += Front * cameraSpeed;
            UpdateCameraPosition();
        }

        public void MoveLeft()
        {
            // do move ViewTarget point left
            ViewTarget -= Right * cameraSpeed;
            UpdateCameraPosition();
        }

        public void MoveRight()
        {
            // do move ViewTarget point right
            ViewTarget += Right * cameraSpeed;
            UpdateCameraPosition();
        }

        public void MoveUp()
        {
            // do move ViewTarget point up
            ViewTarget += Up * cameraSpeed;
            UpdateCameraPosition();
        }

        public void ProcessMouseInputLook(int mouseX, int mouseY)
        {
            if (firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouseX, mouseY);
                firstMove = false;
                return;
            }
                // Calculate the offset of the mouse position
                var deltaX = mouseX - _lastPos.X;
                var deltaY = mouseY - _lastPos.Y;
                _lastPos = new Vector2(mouseX, mouseY);
            // do orbiting around ViewTarget point according to mouse delta move
                Yaw += deltaX * sensitivity;
                Pitch -= deltaY * sensitivity;

                // Clamp pitch to prevent flipping
                Pitch = Math.Clamp(Pitch, -89f, 89f);

                UpdateCameraPosition();
        }

        public void ProcessMouseScroll(int deltaScroll)
        {
            // do move closer (if deltaScroll positive) or further (if deltaScroll negative) to ViewTarget point by view vector
            distanceToTarget -= deltaScroll * 0.02f;
            distanceToTarget = Math.Clamp(distanceToTarget, 0.1f, 100f); // prevent flipping or zooming too far
            UpdateCameraPosition();
        }
    }
}
