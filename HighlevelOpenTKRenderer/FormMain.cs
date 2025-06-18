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
            this.tableLayoutPanelMain.Controls.Add(renderControl,0,0);
        }
    }
}
