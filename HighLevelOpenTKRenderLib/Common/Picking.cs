using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics; // Matrix4, Vector3, Vector4

namespace HighLevelOpenTKRenderLib.Common
{
    /// <summary>
    /// it is math util for picking objects on scene. It is really complicated, I don't understand it much
    /// </summary>
    public static class Picking
    {
        // Multiply a Matrix4 (row-major) by a Vector4 -> Vector4 (row dot product)
        // This implementation matches OpenTK's Matrix4.RowX properties.
        public static Vector4 Multiply(Matrix4 m, Vector4 v)
        {
            return new Vector4(
                m.Row0.X * v.X + m.Row0.Y * v.Y + m.Row0.Z * v.Z + m.Row0.W * v.W,
                m.Row1.X * v.X + m.Row1.Y * v.Y + m.Row1.Z * v.Z + m.Row1.W * v.W,
                m.Row2.X * v.X + m.Row2.Y * v.Y + m.Row2.Z * v.Z + m.Row2.W * v.W,
                m.Row3.X * v.X + m.Row3.Y * v.Y + m.Row3.Z * v.Z + m.Row3.W * v.W
            );
        }
        /*
         For orthographic projection you should unproject two points (near & far) and use direction = (farWorld - nearWorld).Normalized(); origin = nearWorld. 
         for ortho slightly adjust (use clipNear/clipFar unprojection and divide by w to get both world points).
         */
        /*
         The math is the same as perspective for the unprojection part — the difference lies entirely in the projection matrix itself.
        Matrix4.CreateOrthographic(...) makes projMatrix such that the near and far planes are just parallel slices through the world.
        Inverse ViewProjection transforms the mouse point from NDC directly to the correct world position at both near and far planes.
        The resulting (nearWorld, farWorld) form a parallel ray segment that you can use for picking.
         */
        /*
         In perspective, the near point is always camera position.
         In orthographic, the near point is directly under the mouse cursor in world space.
         */
        public static void UnprojectOrtho(
    Vector2 mousePos,          // in window coords, e.g. (mouseX, mouseY)
    Vector2 viewportSize,      // window size in pixels
    Matrix4 viewMatrix,
    Matrix4 projMatrix,
    out Vector3 nearWorld,
    out Vector3 farWorld)
        {
            // Convert to normalized device coordinates (-1..1)
            float x = (2f * mousePos.X) / viewportSize.X - 1f;
            float y = 1f - (2f * mousePos.Y) / viewportSize.Y; // flip Y for OpenGL
            Vector3 ndcNear = new Vector3(x, y, -1f); // z = -1 for near plane
            Vector3 ndcFar = new Vector3(x, y, 1f); // z =  1 for far plane

            // Inverse of ViewProjection
            Matrix4 viewProj = viewMatrix * projMatrix;
            Matrix4 invViewProj;
            Matrix4.Invert(viewProj, out invViewProj);

            // Transform NDC to world space
            // Vector4.Transform does not accept Matrix4, it accepts Quaternion
            /*
            Vector4 nearH = Vector4.Transform(new Vector4(ndcNear, 1f), invViewProj);
            Vector4 farH = Vector4.Transform(new Vector4(ndcFar, 1f), invViewProj);
            */
            Vector4 nearH = Multiply(invViewProj, new Vector4(ndcNear, 1f));
            Vector4 farH = Multiply(invViewProj, new Vector4(ndcFar, 1f));
            // Divide by W to get Cartesian coordinates
            nearWorld = new Vector3(nearH.X / nearH.W, nearH.Y / nearH.W, nearH.Z / nearH.W);
            farWorld = new Vector3(farH.X / farH.W, farH.Y / farH.W, farH.Z / farH.W);
        }
        public static (Vector3 origin, Vector3 direction) ScreenPointToRayOrtho(
            float mouseX, float mouseY,
            float viewportWidth, float viewportHeight,
            Matrix4 projection, Matrix4 view,
            Vector3 cameraPosition)
        {
            Vector3 farWorld; Vector3 nearWorld;
            UnprojectOrtho(new Vector2(mouseX, mouseY), new Vector2(viewportWidth, viewportHeight), view, projection, out nearWorld, out farWorld);
            Vector3 direction = (farWorld - nearWorld).Normalized(); Vector3 origin = nearWorld;
            return (origin, direction);
        }

        /// <summary>
        /// Convert window (mouse) coordinates to a world-space ray.
        /// </summary>
        /// <param name="mouseX">mouse X in client pixels (left = 0)</param>
        /// <param name="mouseY">mouse Y in client pixels (top = 0)</param>
        /// <param name="viewportWidth">viewport width in pixels</param>
        /// <param name="viewportHeight">viewport height in pixels</param>
        /// <param name="projection">projection matrix used for rendering</param>
        /// <param name="view">view matrix used for rendering</param>
        /// <param name="cameraPosition">camera world position</param>
        /// <returns>(origin, direction) where direction is normalized</returns>
        public static (Vector3 origin, Vector3 direction) ScreenPointToRay(
            float mouseX, float mouseY,
            float viewportWidth, float viewportHeight,
            Matrix4 projection, Matrix4 view,
            Vector3 cameraPosition)
        {
            // 1) convert mouse pixel -> NDC
            float ndcX = (2.0f * mouseX) / viewportWidth - 1.0f;
            float ndcY = 1.0f - (2.0f * mouseY) / viewportHeight; // flip Y

            // 2) clip space coordinates for near plane (z = -1) and far plane (z = 1)
            Vector4 clipNear = new Vector4(ndcX, ndcY, -1.0f, 1.0f);
            Vector4 clipFar = new Vector4(ndcX, ndcY, 1.0f, 1.0f);

            // 3) invert projection and view
            Matrix4 invProj;
            Matrix4.Invert(projection, out invProj);
                //there may be exception: throw new InvalidOperationException("Projection matrix not invertible.");

            Matrix4 invView;
            Matrix4.Invert(view, out invView);
                //there may be exception: throw new InvalidOperationException("View matrix not invertible.");

            // 4) unproject to eye (view) space (multiplying clip by inverse projection)
            Vector4 eyeNear = Multiply(invProj, clipNear); // maybe (x,y,z,w)
            Vector4 eyeFar = Multiply(invProj, clipFar);

            // Convert to "direction" form in eye space:
            // set z to -1 and w to 0 to represent a direction (not a position).
            // This is the usual convention used in many GL tutorials.
            eyeNear = new Vector4(eyeNear.X, eyeNear.Y, -1.0f, 0.0f);
            eyeFar = new Vector4(eyeFar.X, eyeFar.Y, -1.0f, 0.0f);

            // 5) transform direction into world space (multiplying by inverse view)
            Vector4 worldDir4 = Multiply(invView, eyeFar);
            Vector3 worldDir = new Vector3(worldDir4.X, worldDir4.Y, worldDir4.Z);
            worldDir.Normalize();

            // Ray origin = camera position (you can also compute world-space near-point if needed)
            Vector3 origin = cameraPosition;

            return (origin, worldDir);
        }
    }
}
