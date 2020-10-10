namespace WindowsFormsApp
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_message = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar_loading = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel_loading = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_back = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_forward = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_reload = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBox_cefAddress = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.panel = new System.Windows.Forms.Panel();
            this.toolStripButton_devTools = new System.Windows.Forms.ToolStripButton();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_message,
            this.toolStripProgressBar_loading,
            this.toolStripStatusLabel_loading});
            this.statusStrip.Location = new System.Drawing.Point(0, 739);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip.Size = new System.Drawing.Size(1264, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_message
            // 
            this.toolStripStatusLabel_message.Name = "toolStripStatusLabel_message";
            this.toolStripStatusLabel_message.Size = new System.Drawing.Size(972, 17);
            this.toolStripStatusLabel_message.Spring = true;
            this.toolStripStatusLabel_message.Text = "toolStripStatusLabel_message";
            this.toolStripStatusLabel_message.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBar_loading
            // 
            this.toolStripProgressBar_loading.Name = "toolStripProgressBar_loading";
            this.toolStripProgressBar_loading.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar_loading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // toolStripStatusLabel_loading
            // 
            this.toolStripStatusLabel_loading.Name = "toolStripStatusLabel_loading";
            this.toolStripStatusLabel_loading.Size = new System.Drawing.Size(173, 17);
            this.toolStripStatusLabel_loading.Text = "toolStripStatusLabel_loading";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_back,
            this.toolStripButton_forward,
            this.toolStripButton_reload,
            this.toolStripTextBox_cefAddress,
            this.toolStripSeparator1,
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripButton_devTools});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1264, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButton_back
            // 
            this.toolStripButton_back.Enabled = false;
            this.toolStripButton_back.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_back.Image")));
            this.toolStripButton_back.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_back.Name = "toolStripButton_back";
            this.toolStripButton_back.Size = new System.Drawing.Size(52, 22);
            this.toolStripButton_back.Text = "后退";
            this.toolStripButton_back.Click += new System.EventHandler(this.toolStripButton_back_Click);
            // 
            // toolStripButton_forward
            // 
            this.toolStripButton_forward.Enabled = false;
            this.toolStripButton_forward.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_forward.Image")));
            this.toolStripButton_forward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_forward.Name = "toolStripButton_forward";
            this.toolStripButton_forward.Size = new System.Drawing.Size(52, 22);
            this.toolStripButton_forward.Text = "前进";
            this.toolStripButton_forward.Click += new System.EventHandler(this.toolStripButton_forward_Click);
            // 
            // toolStripButton_reload
            // 
            this.toolStripButton_reload.Enabled = false;
            this.toolStripButton_reload.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_reload.Image")));
            this.toolStripButton_reload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_reload.Name = "toolStripButton_reload";
            this.toolStripButton_reload.Size = new System.Drawing.Size(52, 22);
            this.toolStripButton_reload.Text = "刷新";
            this.toolStripButton_reload.Click += new System.EventHandler(this.toolStripButton_reload_Click);
            // 
            // toolStripTextBox_cefAddress
            // 
            this.toolStripTextBox_cefAddress.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.toolStripTextBox_cefAddress.Name = "toolStripTextBox_cefAddress";
            this.toolStripTextBox_cefAddress.Size = new System.Drawing.Size(400, 25);
            this.toolStripTextBox_cefAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox_cefAddress_KeyDown);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Enabled = false;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(52, 22);
            this.toolStripButton4.Text = "收藏";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.Enabled = false;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(61, 22);
            this.toolStripButton5.Text = "脚本...";
            // 
            // panel
            // 
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 25);
            this.panel.Margin = new System.Windows.Forms.Padding(0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(1264, 714);
            this.panel.TabIndex = 2;
            // 
            // toolStripButton_devTools
            // 
            this.toolStripButton_devTools.Enabled = false;
            this.toolStripButton_devTools.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_devTools.Image")));
            this.toolStripButton_devTools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_devTools.Name = "toolStripButton_devTools";
            this.toolStripButton_devTools.Size = new System.Drawing.Size(109, 22);
            this.toolStripButton_devTools.Text = "开发人员工具...";
            this.toolStripButton_devTools.Click += new System.EventHandler(this.toolStripButton_devTools_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 761);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton_back;
        private System.Windows.Forms.ToolStripButton toolStripButton_forward;
        private System.Windows.Forms.ToolStripButton toolStripButton_reload;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox_cefAddress;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_message;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_loading;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar_loading;
        private System.Windows.Forms.ToolStripButton toolStripButton_devTools;
    }
}

