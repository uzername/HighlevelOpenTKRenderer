namespace HighLevelOpenTKRenderLib
{
    partial class MainRenderControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            glControlMain = new OpenTK.GLControl.GLControl();
            SuspendLayout();
            // 
            // glControlMain
            // 
            glControlMain.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            glControlMain.APIVersion = new Version(3, 3, 0, 0);
            glControlMain.Dock = DockStyle.Fill;
            glControlMain.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            glControlMain.IsEventDriven = true;
            glControlMain.Location = new Point(0, 0);
            glControlMain.Name = "glControlMain";
            glControlMain.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            glControlMain.SharedContext = null;
            glControlMain.Size = new Size(555, 391);
            glControlMain.TabIndex = 0;
            // 
            // MainRenderControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(glControlMain);
            Name = "MainRenderControl";
            Size = new Size(555, 391);
            ResumeLayout(false);
        }

        #endregion

        private OpenTK.GLControl.GLControl glControlMain;
    }
}
