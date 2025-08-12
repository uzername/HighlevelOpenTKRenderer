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
