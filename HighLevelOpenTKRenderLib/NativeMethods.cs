using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighLevelOpenTKRenderLib
{
    internal class NativeMethods
    {

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public struct MSG
            {
                public IntPtr hwnd;
                public uint message;
                public UIntPtr wParam;
                public IntPtr lParam;
                public uint time;
                public System.Drawing.Point p;
            }

            [System.Runtime.InteropServices.DllImport("user32.dll")]
        /// PeekMessage checks if the Windows message queue is empty, which means the application is idle — it's a proven pattern used in many older OpenGL WinForms projects.
        public static extern bool PeekMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
                uint wMsgFilterMax, uint wRemoveMsg);

    }
}
