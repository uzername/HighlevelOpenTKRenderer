### Separating Camera Data from Input Behavior

`CameraMk2` (from LearnOpenTK) is a pure data + math class: it holds position, front vector, up vector, FOV, projection matrix, etc.

`ICameraControl` defines how the user can manipulate the camera, abstracting control logic from the camera's data.

`FirstPersonCamera`, `OrbitingCamera`, etc., provide specific input processing behaviors (e.g., WASD for FPS, drag to orbit, etc.).

This separation adheres to the Single Responsibility Principle and makes system highly flexible.

### Perspective distortion

When your camera is aimed directly at the cube, it lies near the center of the view frustum, and each of its edges is evenly projected. So:

The left edge appears small. Perspective is "minimal" because the object is centered. 

Then, you rotate the camera to the right, causing the cube to: Move left in screen space (closer to the screen’s left edge). 
Still remain at the same distance from the camera in 3D, but... 
Project onto a skewed part of the frustum — specifically, the left edge of the near plane, where rays are more angled. This results in the left edge of the cube being projected larger.

The camera frustum is like a cone pointing forward:

```
          /\
         /  \  ← Frustum walls
        /----\
      camera   far plane
```

Objects in the center are projected "normally."

Objects near the edges of this cone get stretched outward more — their projected width increases.

That's why objects moving toward the edge of the screen look distorted or enlarged in screen space.

**Option 1: Reduce Field of View**

Lower FoV = less distortion at the screen edges.

```
// From 70–90° default to a more natural 45°
Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), aspect, near, far);
```

This reduces the "edge pull" and keeps sizes more stable across screen positions.

**Option 2: Use Orthographic Projection**

If you're doing CAD, engineering, or diagram-like rendering, use orthographic mode — it removes all size variation due to distance or screen position.

But it lacks depth feeling and can feel "flat."