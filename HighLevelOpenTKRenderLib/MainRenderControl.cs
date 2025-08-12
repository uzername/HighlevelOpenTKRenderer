using HighLevelOpenTKRenderLib.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HighLevelOpenTKRenderLib
{
    public partial class MainRenderControl : UserControl
    {
        public Scene CurrentScene;
        public bool performPicking;
        /// <summary>
        /// object has been picked on scene - event triggered from OnClick
        /// </summary>
        public event Action<String?> onObjectPicked;

        private Shader backgroundShader;
        private Shader simpleobjectShader;
        private Shader phongobjectShader;
        private Shader thicklineobjectShader;

        private int vaoBackground;

        public MainRenderControl()
        {
            InitializeComponent();

            // Hook up event handlers
            /* render code */
            glControlMain.Paint += glControlMain_Paint;
            /* re-process aspect ratio */
            glControlMain.Resize += glControlMain_Resize;
            /* init */
            glControlMain.Load += glControlMain_Load;
            /* move further or closer with mouse scroll. Or change field of view for Orthogonal camera */
            glControlMain.MouseWheel += OnEvent_MouseWheel;
            // Set up continuous rendering
            Application.Idle += Application_Idle;
        }



        private bool initialized = false;
        private void glControlMain_Load(object sender, EventArgs e)
        {
            if (initialized) return;
            glControlMain.MakeCurrent();
            // this one is for transparency
            // alpha value affects how "fragment" from fragment shader is combined with what's already in the framebuffer.
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // https://stackoverflow.com/a/35780405
            // You can't read Resource Files with File.ReadAllText.
            // Instead you need to open a Resource Stream with Assembly.GetManifestResourceStream.
            //== setup background ==
            using (var streamvert = Assembly.GetExecutingAssembly().GetManifestResourceStream("HighLevelOpenTKRenderLib.Shaders.background.vert"))
            using (var streamfrag = Assembly.GetExecutingAssembly().GetManifestResourceStream("HighLevelOpenTKRenderLib.Shaders.background.frag"))
            {
                TextReader textReadVert = new StreamReader(streamvert);
                TextReader textReadFrag = new StreamReader(streamfrag);
                backgroundShader = new Shader(textReadVert, textReadFrag);
            }
            vaoBackground = GL.GenVertexArray();
            //== setup camera and simple shader ==
            using (var streambasicvert = Assembly.GetExecutingAssembly().GetManifestResourceStream("HighLevelOpenTKRenderLib.Shaders.basic.vert"))
            using (var streambasicfrag = Assembly.GetExecutingAssembly().GetManifestResourceStream("HighLevelOpenTKRenderLib.Shaders.basic.frag"))
            {
                TextReader textReadVert = new StreamReader(streambasicvert);
                TextReader textReadFrag = new StreamReader(streambasicfrag);
                simpleobjectShader = new Shader(textReadVert, textReadFrag);
            }
            using (var streamphongvert = Assembly.GetExecutingAssembly().GetManifestResourceStream("HighLevelOpenTKRenderLib.Shaders.phong.vert"))
            using (var streamphongfrag = Assembly.GetExecutingAssembly().GetManifestResourceStream("HighLevelOpenTKRenderLib.Shaders.phong.frag"))
            {
                TextReader textReadVert = new StreamReader(streamphongvert);
                TextReader textReadFrag = new StreamReader(streamphongfrag);
                phongobjectShader = new Shader(textReadVert, textReadFrag);
            }
            using (var streamthicklinevert = Assembly.GetExecutingAssembly().GetManifestResourceStream("HighLevelOpenTKRenderLib.Shaders.thick_line.vert"))
            using (var streamthicklinefrag = Assembly.GetExecutingAssembly().GetManifestResourceStream("HighLevelOpenTKRenderLib.Shaders.thick_line.frag"))
            using (var streamthicklinegeom = Assembly.GetExecutingAssembly().GetManifestResourceStream("HighLevelOpenTKRenderLib.Shaders.thick_line.geom"))
            {
                TextReader textReadVert = new StreamReader(streamthicklinevert);
                TextReader textReadFrag = new StreamReader(streamthicklinefrag);
                TextReader textReadGeom = new StreamReader(streamthicklinegeom);
                thicklineobjectShader = new Shader (textReadVert, textReadFrag, textReadGeom);
            }
            CurrentScene = new Scene();
            //CurrentScene.camera = new OrbitingCamera(new Vector3(0, 0, 0), 5.0f, (float)glControlMain.ClientSize.Width / glControlMain.ClientSize.Height, 60);
            CurrentScene.camera = new FirstPersonCamera(new Vector3(0, 0, 5), (float)glControlMain.ClientSize.Width / glControlMain.ClientSize.Height, 60);
            CurrentScene.AddTestObject2();
            CurrentScene.SceneLights.Add(new Light { Color = new Vector3(1, 1, 1), Position = new Vector3(3, 3, 3) });

            glControlMain.TabStop = true;
            glControlMain.Focus();
            this.ParentForm.Shown += (s, e) =>
            {
                glControlMain.Focus();
            };
            glControlMain_Resize(this, null);
            initialized = true;
        }

        private void glControlMain_Resize(object sender, EventArgs e)
        {
            if ((glControlMain.ClientSize.Height == 0) || (CurrentScene == null)) return; // avoid division by zero
            if (!glControlMain.Context.IsCurrent)
                glControlMain.MakeCurrent();

            GL.Viewport(0, 0, glControlMain.Width, glControlMain.Height);
            // Update aspect ratio in camera
            if (CurrentScene.camera is CameraMk2 cam)
            {
                float aspectRatio = (float)glControlMain.ClientSize.Width / glControlMain.ClientSize.Height;
                // changing aspect ration will recalculate projection matrix
                
                cam.AspectRatio = aspectRatio;
            }

            glControlMain.Invalidate(); // optionally force redraw
        }

        private void glControlMain_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }
        private void Application_Idle(object sender, EventArgs e)
        {
            // use a while loop that waits until the message queue is empty — the classic WinForms loop trick
            while (IsApplicationIdle())
            {
                glControlMain.Invalidate();
            }
        }
        private bool IsApplicationIdle()
        {
            NativeMethods.PeekMessage(out var msg, IntPtr.Zero, 0, 0, 0);
            return msg.message == 0;
        }
        private void Render()
        {
            void prepareRenderObj(Object3D obj)
            {
                if (obj is ThickLineObject3D)
                {
                    thicklineobjectShader.Use();

                    var view = CurrentScene.camera.GetViewMatrix();
                    var projection = CurrentScene.camera.GetProjectionMatrix();

                    GL.UniformMatrix4(thicklineobjectShader.GetUniformLocation("uModel"), false, ref obj.Transform);
                    GL.UniformMatrix4(thicklineobjectShader.GetUniformLocation("uView"), false, ref view);
                    GL.UniformMatrix4(thicklineobjectShader.GetUniformLocation("uProjection"), false, ref projection);

                    // support color
                    GL.Uniform4(thicklineobjectShader.GetUniformLocation("uColor"), (obj as ThickLineObject3D).SimpleColor);

                    // support thickness
                    thicklineobjectShader.SetFloat("uThickness", (obj as ThickLineObject3D).thicknessLine);
                    // transfer the dimensions of display
                    double hght = glControlMain.ClientSize.Height;
                    double wdth = glControlMain.ClientSize.Width;
                    thicklineobjectShader.SetVector2("uViewportSize", new Vector2((float) wdth, (float) hght));
                } else
                if (obj is SimpleObject3D)
                {
                    simpleobjectShader.Use();
                    // Setup uniforms
                    var view = CurrentScene.camera.GetViewMatrix();
                    var projection = CurrentScene.camera.GetProjectionMatrix();
                    GL.UniformMatrix4(simpleobjectShader.GetUniformLocation("model"), false, ref obj.Transform);
                    GL.UniformMatrix4(simpleobjectShader.GetUniformLocation("view"), false, ref view);
                    GL.UniformMatrix4(simpleobjectShader.GetUniformLocation("projection"), false, ref projection);

                    GL.Uniform4(simpleobjectShader.GetUniformLocation("color"), (obj as SimpleObject3D).SimpleColor);
                }
                else if (obj is LitObject3D)
                {

                    phongobjectShader.Use();
                    // Setup uniforms
                    var view = CurrentScene.camera.GetViewMatrix();
                    var projection = CurrentScene.camera.GetProjectionMatrix();
                    GL.UniformMatrix4(phongobjectShader.GetUniformLocation("model"), false, ref obj.Transform);
                    GL.UniformMatrix4(phongobjectShader.GetUniformLocation("view"), false, ref view);
                    GL.UniformMatrix4(phongobjectShader.GetUniformLocation("projection"), false, ref projection);

                    phongobjectShader.SetVector3("lightPos", CurrentScene.SceneLights[0].Position);
                    phongobjectShader.SetVector3("lightColor", CurrentScene.SceneLights[0].Color);

                    phongobjectShader.SetVector3("viewPos", CurrentScene.camera.Position);

                    phongobjectShader.SetVector4("materialDiffuse", (obj as LitObject3D).LitMaterial.DiffuseColor);
                    phongobjectShader.SetVector3("materialSpecular", (obj as LitObject3D).LitMaterial.SpecularColor);
                    phongobjectShader.SetFloat("materialShininess", (obj as LitObject3D).LitMaterial.Shininess);
                }
            }
            void renderOpaqueObjects()
            {
                foreach (var objjj in CurrentScene.SceneObjects)
                {
                    if (((objjj is SimpleObject3D) || objjj is ThickLineObject3D || ((objjj is LitObject3D) && (objjj as LitObject3D).LitMaterial.DiffuseColor[3] == 1))&& objjj.IsShown)
                    {
                        prepareRenderObj(objjj);
                        objjj.Draw();
                    }
                }
            }
            void renderTransparentObjects()
            {
                List<LitObject3D> TransparentObjects = new List<LitObject3D>();
                foreach (var objjj in CurrentScene.SceneObjects)
                {
                    
                    if ( (((objjj is LitObject3D) && (objjj as LitObject3D).LitMaterial.DiffuseColor[3] == 1) == false)&& objjj.IsShown )
                    {
                        if (objjj is LitObject3D)
                            TransparentObjects.Add(objjj as LitObject3D);
                    }
                    
                }
                if (TransparentObjects.Count != 0)
                {
                    // This sorts objects back to front relative to the camera, so transparency blends correctly and obj.Transform.Row3.Xyz represents position
                    TransparentObjects.OrderByDescending(obj => (CurrentScene.camera.Position - obj.Transform.Row3.Xyz).LengthSquared);
                }
                foreach (var objjj1 in TransparentObjects) {
                    prepareRenderObj(objjj1);
                    objjj1.Draw();
                }
            }

            if (!glControlMain.Context.IsCurrent)
                glControlMain.MakeCurrent();

            // Clear screen
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Disable(EnableCap.DepthTest); // Avoid depth interference

            backgroundShader.Use();
            GL.BindVertexArray(vaoBackground);

            // Set colors from scene
            GL.Uniform4(GL.GetUniformLocation(backgroundShader.Handle, "color1"), CurrentScene.color1);
            GL.Uniform4(GL.GetUniformLocation(backgroundShader.Handle, "color2"), CurrentScene.color2);
            // Draw fullscreen background
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.Enable(EnableCap.DepthTest); // Re-enable for 3D scene

            //  Draw scene here. I cannot move selection of shaders and setting uniforms to their appropriate Draw() method because
            //  they use camera and light source which is related to scene
            
            // Render not transparent objects
            GL.Disable(EnableCap.Blend);
            renderOpaqueObjects();

            // Render transparent objects
            GL.Enable(EnableCap.Blend);
            GL.DepthMask(false); // important for transparency!
            renderTransparentObjects();
            GL.DepthMask(true);
            /*
            foreach (var obj in CurrentScene.SceneObjects)
            {
                prepareRenderObj(obj);
                obj.Draw();
            }
            */
            glControlMain.SwapBuffers();
        }
        private void OnEvent_MouseWheel(object? sender, MouseEventArgs e)
        {
            CurrentScene.camera.ProcessMouseScroll(e.Delta);
        }
        private void glControlMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    { // forward
                        CurrentScene.camera.MoveForward();
                        break;
                    }
                case Keys.S:
                    { // backwards
                        CurrentScene.camera.MoveBackward();
                        break;
                    }
                case Keys.A:
                    { // left
                        CurrentScene.camera.MoveLeft();
                        break;
                    }
                case Keys.D:
                    { //right
                        CurrentScene.camera.MoveRight();
                        break;
                    }
                case Keys.Space:
                    { // up
                        CurrentScene.camera.MoveUp();
                        break;
                    }
                case Keys.ShiftKey:
                    { // down
                        CurrentScene.camera.MoveDown();
                        break;
                    }
                default: break;
            }


        }

        private void glControlMain_MouseMove(object sender, MouseEventArgs e)
        {
            // mouse was moved while right button is held
            if (e.Button == MouseButtons.Right)   {
                CurrentScene.camera.ProcessMouseInputLook(e.X, e.Y);
            }
        }

        private void glControlMain_MouseDown(object sender, MouseEventArgs e)
        {
            this.Focus();
            if ((performPicking == false)|| (e.Button != MouseButtons.Left)) return;
            int mouseX = e.X;    int mouseY = e.Y;
            int viewportHeight = glControlMain.ClientSize.Height;
            int viewportWidth = glControlMain.ClientSize.Width;

            var proj = CurrentScene.camera.GetProjectionMatrix();
            var view = CurrentScene.camera.GetViewMatrix();
            var camPos = CurrentScene.camera.Position;
            Vector3 origin = new Vector3(); Vector3 dir = new Vector3();
            if (CurrentScene.camera.isPerspective)  {
                var (origin0, dir0) = Picking.ScreenPointToRay(e.X, e.Y, viewportWidth, viewportHeight, proj, view, camPos);
                origin = origin0; dir = dir0;
            } else
            {
                var (origin0, dir0) = Picking.ScreenPointToRayOrtho(e.X, e.Y, viewportWidth, viewportHeight, proj, view, camPos);
                origin = origin0; dir = dir0;
            }
                // Do intersection tests with your scene (bounding boxes, triangles, etc.)
                onObjectPicked?.Invoke($"Ray origin={origin}, dir={dir}");

        }

        private void glControlMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                CurrentScene.camera.firstMove = true;
            }
        }

        private void glControlMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar=='p')  {
                bool newPerspCamera = !CurrentScene.camera.isPerspective;
                    CurrentScene.camera.SetPerspectiveCamera(newPerspCamera);
                
            }
        }

    }
}
