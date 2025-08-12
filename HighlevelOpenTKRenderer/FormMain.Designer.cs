namespace HighlevelOpenTKRenderer
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanelMain = new TableLayoutPanel();
            textBox1 = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            checkBoxPicking = new CheckBox();
            labelPicking = new Label();
            tableLayoutPanelMain.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 1;
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.Controls.Add(textBox1, 0, 2);
            tableLayoutPanelMain.Controls.Add(flowLayoutPanel1, 0, 1);
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tableLayoutPanelMain.Location = new Point(0, 0);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 3;
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 75F));
            tableLayoutPanelMain.RowStyles.Add(new RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanelMain.Size = new Size(521, 359);
            tableLayoutPanelMain.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Fill;
            textBox1.Font = new Font("Lucida Console", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBox1.Location = new Point(3, 278);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(515, 78);
            textBox1.TabIndex = 0;
            textBox1.Text = "Here I test a Winforms control for 3D.\r\nControls - A, W, S, D to move; Space to go Up, Shift to go down, \r\np to toggle orthographic perspective camera\r\n Rightclick and hold to look around";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(checkBoxPicking);
            flowLayoutPanel1.Controls.Add(labelPicking);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(1, 252);
            flowLayoutPanel1.Margin = new Padding(1);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(519, 22);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // checkBoxPicking
            // 
            checkBoxPicking.AutoSize = true;
            checkBoxPicking.Checked = true;
            checkBoxPicking.CheckState = CheckState.Checked;
            checkBoxPicking.Location = new Point(1, 1);
            checkBoxPicking.Margin = new Padding(1);
            checkBoxPicking.Name = "checkBoxPicking";
            checkBoxPicking.Size = new Size(98, 19);
            checkBoxPicking.TabIndex = 0;
            checkBoxPicking.Text = "Allow Picking";
            checkBoxPicking.UseVisualStyleBackColor = true;
            checkBoxPicking.CheckedChanged += checkBoxPicking_CheckedChanged;
            // 
            // labelPicking
            // 
            labelPicking.AutoSize = true;
            labelPicking.Location = new Point(103, 0);
            labelPicking.Name = "labelPicking";
            labelPicking.Padding = new Padding(0, 2, 0, 0);
            labelPicking.Size = new Size(91, 17);
            labelPicking.TabIndex = 1;
            labelPicking.Text = "picking enabled";
            labelPicking.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(521, 359);
            Controls.Add(tableLayoutPanelMain);
            Name = "FormMain";
            Text = "Here is testing of OpenGL control";
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanelMain;
        private TextBox textBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        private CheckBox checkBoxPicking;
        private Label labelPicking;
    }
}
