using HighLevelOpenTKRenderLib.Common;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HighLevelOpenTKRenderLib
{
    public partial class MainRenderControl : UserControl
    {
        public Scene CurrentScene;

        private Shader backgroundShader;
        private int vaoBackground;

        public MainRenderControl()
        {
            InitializeComponent();
            CurrentScene = new Scene();
            // Hook up event handlers
            glControlMain.Paint += glControlMain_Paint;
            glControlMain.Resize += glControlMain_Resize;
            glControlMain.Load += glControlMain_Load;

            // Set up continuous rendering
            Application.Idle += Application_Idle;
        }
        private bool initialized = false;
        private void glControlMain_Load(object sender, EventArgs e)
        {
            if (initialized) return;
            glControlMain.MakeCurrent();

            // https://stackoverflow.com/a/35780405
            // You can't read Resource Files with File.ReadAllText.
            // Instead you need to open a Resource Stream with Assembly.GetManifestResourceStream.
            
            using (var streamvert = Assembly.GetExecutingAssembly().GetManifestResourceStream("HighLevelOpenTKRenderLib.Shaders.background.vert"))
            using (var streamfrag = Assembly.GetExecutingAssembly().GetManifestResourceStream("HighLevelOpenTKRenderLib.Shaders.background.frag"))
            {
                TextReader textReadVert = new StreamReader(streamvert);
                TextReader textReadFrag = new StreamReader(streamfrag);
                backgroundShader = new Shader(textReadVert, textReadFrag);
            }
            vaoBackground = GL.GenVertexArray();
            initialized = true;
        }

        private void glControlMain_Resize(object sender, EventArgs e)
        {
            if (!glControlMain.Context.IsCurrent)
                glControlMain.MakeCurrent();

            GL.Viewport(0, 0, glControlMain.Width, glControlMain.Height);
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

            // TODO: Draw your scene here

            glControlMain.SwapBuffers();
        }

    }
}
