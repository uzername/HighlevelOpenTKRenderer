### Separating Camera Data from Input Behavior

`CameraMk2` (from LearnOpenTK) is a pure data + math class: it holds position, front vector, up vector, FOV, projection matrix, etc.

`ICameraControl` defines how the user can manipulate the camera, abstracting control logic from the camera's data.

`FirstPersonCamera`, `OrbitingCamera`, etc., provide specific input processing behaviors (e.g., WASD for FPS, drag to orbit, etc.).

This separation adheres to the Single Responsibility Principle and makes system highly flexible.