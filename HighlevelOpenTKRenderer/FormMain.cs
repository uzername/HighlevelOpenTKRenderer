using HighLevelOpenTKRenderLib;

namespace HighlevelOpenTKRenderer
{
    public partial class FormMain : Form
    {
        MainRenderControl renderControl;
        public FormMain()
        {
            InitializeComponent();
            // add this from code to avoid exception
            renderControl = new MainRenderControl();
            renderControl.Dock = DockStyle.Fill;
            this.tableLayoutPanelMain.Controls.Add(renderControl, 0, 0);
            renderControl.onObjectPicked += RenderControl_onObjectPicked;
            renderControl.performPicking = true;
            // cannot add objects here to scene, need to wait OpenGL initialization
            // but when Render Control is initialized then I may add test objects . renderControl already has OnLoad handler hooked, but I hook another one so they run in sequence
            renderControl.Load += RenderControl_Load;
        }

        private void RenderControl_Load(object? sender, EventArgs e)
        {
            SceneTest.AddTestObject2(renderControl.CurrentScene);
        }

        private void RenderControl_onObjectPicked(string? obj)
        {
            if (obj != null) { labelPicking.Text = obj; }
        }

        private void checkBoxPicking_CheckedChanged(object sender, EventArgs e)
        {
            renderControl.performPicking = checkBoxPicking.Checked;
            if (renderControl.performPicking) { labelPicking.Text = "picking enabled"; } else { labelPicking.Text = "picking disabled"; }
        }
    }
}
