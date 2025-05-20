### Opentk (Opengl) control for C# WinForms, for scientific and engineering visualisation

I think that Opentk (Opengl) control for WinForms may be useful, because WPF 3D does not work well on some computers (with Intel built-in videochips), and also Winforms does not have highlevel control for 3D graphics. It's OpenGL and it's too complicated for me to implement singlehanded. But it's my dream. One day it may come true. Desired functionality is similar to WPF 3D or Wolf Toolkit (WPF): 

- simple 3D scene setup and intuitive manipulation of objects on camera (OpenGL does not have definition of object by design, it operates with matrices and arrays of coordinates). 
- Camera class. Intuitive setup for camera with handy methods like Move camera, turn camera around, zoom-pan camera, fit all objects on scene into view, without thinking about matrix transformations. Orthogonal and perspective camera setting. 
- grouping of objects on scene should be implemented too, so when one object moves, parent objects should move with it.
- Math and transforms: rotate object around specific point, translate object to distance, mirror object over arbitrary mirroring plane. OpenTK already contains matrix calculations, those methods are to extend
- Predefined objects (cylinder, cube, sphere) and ability to setup my own object using vertices/indices/normals 
- Screen-space lines with ability to set up their thickness 
- Predefined Phong shaders and materials (uhh shaders), and also with transparency shaders too, with appropriate z-layering, no raytracing and physical lighting needed, it's for engineering. Just simple lighting. Ability to set specularity and color. No shadows needed. Also, ability to set up ambient lighting. Gradient background of scene is ok 
- Picking items on scene with mouse 
- Screen-space cube (like in Inventor or Sketchup or Maya) to quickly set desired view of camera, or axis lines (like in Blender 3D)

#### Implementation

I'd like to build a solution around OpenTK Winforms control v 4.0 , using modern OpenGL with shaders and buffers (not instant mode). There may be such entities that implement such description:
- MainRenderControl that contains OpenTK winforms control instance, an entry point of library
- Camera - base class for camera, parent for FreeMoveCamera and OrbitingCamera. Orthogonal and perspective camera setting (I assume it should not really be a separate class for it, it is just about mode of matrix calculation.
- Object3D - generic object for items on scene. it contains transformations in matrix form. Also it has vertices and indices
- MeshConstructor3D - utility class to construct vertices and indices for cylinder, cube, sphere
- Scene - collection of objects. While rendering inside MainRenderControl, a cycle runs over this collection to show object3d . It should have methods to remove object from scene, add created and set up Object3D to scene.
- Light - OpenGL has no concept of light, it is just a setting of shader. But for scene logic it is important to have light as object that can be translated around. 
- AmbientLight