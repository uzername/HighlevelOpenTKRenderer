### Opentk (Opengl) control for C# WinForms, for scientific and engineering visualisation

I think that Opentk (Opengl) control for WinForms may be useful, because WPF 3D does not work well on some computers (with Intel built-in videochips), and also Winforms does not have highlevel control for 3D graphics. It's OpenGL and it's too complicated for me to implement singlehanded. But it's my dream. One day it may come true. Desired functionality is similar to WPF 3D or Wolf Toolkit (WPF): 

- [x] simple 3D scene setup and intuitive manipulation of objects on camera (OpenGL does not have definition of object by design, it operates with matrices and arrays of coordinates). 
- [ ] Camera class.  
    - [x] Intuitive setup for camera with handy methods like Move camera, turn camera around, zoom-pan camera
	- [x] Orthogonal and perspective camera setting. 
	- [ ] fit all objects on scene into view, without thinking about matrix transformations. (this one corresponds to scene actually)
- [ ] grouping of objects on scene should be implemented too, so when one object moves, parent objects should move with it.
- [x] Math and transforms ( OpenTK already contains matrix calculations, those methods are to extend ) : 
    - [x] rotate object around specific ~~point~~ ~~vector~~ axis, 
    - [x] translate object to distance, 
    - [x] mirror object over coordinate mirroring plane. 
    - [x] mirror object over arbitrary plane.
- [x] Predefined objects ( :white_check_mark: cylinder, :white_check_mark: cube, :white_check_mark: sphere and :white_circle: rectangular prism) 
- [ ] ability to setup my own object using vertices/indices/normals 
- [x] Screen-space lines with ability to set up their thickness ( :heavy_check_mark: There is also a single-pixel wide line using opengl setting ). 
Those are World-space lines with screen-space thickness. These lines: Exist in 3D world space, like any mesh; Are correctly affected by camera perspective and occlusion; Appear with constant screen-space width (e.g., always 2px or 5px wide), even when close or far.
(sort of works now, but there is always space for improvement. It uses geometry shader)
- [ ] Also, ability to set up ambient lighting. 
- [ ] Just simple lighting and light sources
- [x] Predefined Phong shaders and materials (uhh shaders). Ability to set specularity and color. No shadows needed.
- [x] transparency shaders too, with appropriate z-layering, no raytracing and physical lighting needed, it's for engineering.   
- [x] Gradient background of scene is ok 
- [ ] Picking items on scene with mouse ( :heavy_check_mark: now I can get a vector and starting point. Next I need to calculate collision of obtained vector with Axis Aligned Bound Box, as for rough pick direction. Then I need to perform more fine picking processing with obtained rough picking results )
- [ ] Screen-space cube (like in Inventor or Sketchup or Maya) to quickly set desired view of camera, or axis lines (like in Blender 3D)

#### Implementation

ChatGPT helps me a lot with this project, this project won't be possible without its aid.

I'd like to build a solution around OpenTK Winforms control v 4.0 , using modern OpenGL with shaders and buffers (not instant mode). There may be such entities that implement such description:
- MainRenderControl that contains OpenTK winforms control instance, an entry point of library
- CameraMk2 - base class for camera, parent for FreeMoveCamera and OrbitingCamera. Orthogonal and perspective camera setting (I assume it should not really be a separate class for it, it is just about mode of matrix calculation). Call `SetPerspectiveCamera(bool isPerspectiveCamera)` to toggle between camera modes, no need to re-create camera. By the way, size of object on display does not change when Orthogonal camera is moved closer or further - mathematically correct Orthogonal camera behavior.
    - ICameraControl - control scheme of camera
      - FirstPersonCamera
      - OrbitingCamera
- Object3D - generic object for items on scene. it contains transformations in matrix form. Also it has vertices and indices. It keeps Vertices list and may provide bound box if needed
    - SimpleObject3D - Object3D that ignores lighting on rendering. Uses `basic.frag` shader to simply convey a solid color of object upon rendering. Uses `basic.vert` to compute matrices - model, view and projection that show object on 3D scene according to camera
    - LitObject3D - uses Phong shader and requires normals for rendering, uses two shaders `phong.frag` - fragment shader that calculates color of lit object. Also it uses transparency. `phong.vert` - performs matrix calculations of model, view and projection 
    - ThickLineObject3D - uses `thick_line.geom` (geometry shader that gives 4 coordinates for a quad to OpenGL conveyor from 2 line coordinates), `thick_line.frag` (fragment shader, similar as in simple object 3d), `thick_line.vert` (it makes line to look in scene). 
      It shares a lot in common with SimpleObject3D
      Both types of line - with arbitrary width and single-pixel default one - are drawn using method `GL.DrawElements(PrimitiveType.Lines, indicesCount, DrawElementsType.UnsignedInt, 0);`
- MeshConstructor3D - utility class to construct vertices and indices for cylinder, cube, sphere. It also has insight how to make your own object 
- Scene - collection of objects. While rendering inside MainRenderControl, a cycle runs over this collection to show object3d . It should have methods to remove object from scene, add created and set up Object3D to scene.
- Light - OpenGL has no concept of light, it is just a setting of shader. But for scene logic it is important to have light as object that can be translated around. 