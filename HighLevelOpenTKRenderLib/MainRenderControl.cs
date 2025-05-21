using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL4;

namespace HighLevelOpenTKRenderLib
{
    public partial class MainRenderControl : UserControl
    {
        public Scene CurrentScene;
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

        private void glControlMain_Load(object sender, EventArgs e)
        {
            glControlMain.MakeCurrent();

            // Basic OpenGL setup
            GL.ClearColor(CurrentScene.color1.R, CurrentScene.color1.G, CurrentScene.color1.B, CurrentScene.color1.A);
            GL.Enable(EnableCap.DepthTest);
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

            // TODO: Draw your scene here

            glControlMain.SwapBuffers();
        }

    }
}
