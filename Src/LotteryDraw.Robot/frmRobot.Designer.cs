namespace RevealTest
{
    partial class frmRobot
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRobot));
            this.btnCreateData = new System.Windows.Forms.Button();
            this.btnOpenLottery = new System.Windows.Forms.Button();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.btnStopReveal = new System.Windows.Forms.Button();
            this.plContainerTop = new System.Windows.Forms.Panel();
            this.plContainerBottom = new System.Windows.Forms.Panel();
            this.ni = new System.Windows.Forms.NotifyIcon(this.components);
            this.ctmTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctmShowMain = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmExitSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.plContainerTop.SuspendLayout();
            this.plContainerBottom.SuspendLayout();
            this.ctmTray.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCreateData
            // 
            this.btnCreateData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateData.Location = new System.Drawing.Point(431, 8);
            this.btnCreateData.Name = "btnCreateData";
            this.btnCreateData.Size = new System.Drawing.Size(76, 61);
            this.btnCreateData.TabIndex = 0;
            this.btnCreateData.Text = "构造数据";
            this.btnCreateData.UseVisualStyleBackColor = true;
            this.btnCreateData.Visible = false;
            this.btnCreateData.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOpenLottery
            // 
            this.btnOpenLottery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenLottery.Location = new System.Drawing.Point(12, 8);
            this.btnOpenLottery.Name = "btnOpenLottery";
            this.btnOpenLottery.Size = new System.Drawing.Size(76, 61);
            this.btnOpenLottery.TabIndex = 1;
            this.btnOpenLottery.Text = "启动开奖";
            this.btnOpenLottery.UseVisualStyleBackColor = true;
            this.btnOpenLottery.Click += new System.EventHandler(this.btnOpenLottery_Click);
            // 
            // txtInfo
            // 
            this.txtInfo.AcceptsReturn = true;
            this.txtInfo.AcceptsTab = true;
            this.txtInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfo.Location = new System.Drawing.Point(0, 0);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInfo.Size = new System.Drawing.Size(524, 247);
            this.txtInfo.TabIndex = 2;
            // 
            // btnStopReveal
            // 
            this.btnStopReveal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStopReveal.Location = new System.Drawing.Point(102, 8);
            this.btnStopReveal.Name = "btnStopReveal";
            this.btnStopReveal.Size = new System.Drawing.Size(76, 61);
            this.btnStopReveal.TabIndex = 3;
            this.btnStopReveal.Text = "停止开奖";
            this.btnStopReveal.UseVisualStyleBackColor = true;
            this.btnStopReveal.Click += new System.EventHandler(this.btnStopReveal_Click);
            // 
            // plContainerTop
            // 
            this.plContainerTop.Controls.Add(this.btnCreateData);
            this.plContainerTop.Controls.Add(this.btnStopReveal);
            this.plContainerTop.Controls.Add(this.btnOpenLottery);
            this.plContainerTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.plContainerTop.Location = new System.Drawing.Point(0, 0);
            this.plContainerTop.Name = "plContainerTop";
            this.plContainerTop.Size = new System.Drawing.Size(524, 77);
            this.plContainerTop.TabIndex = 4;
            // 
            // plContainerBottom
            // 
            this.plContainerBottom.Controls.Add(this.txtInfo);
            this.plContainerBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plContainerBottom.Location = new System.Drawing.Point(0, 77);
            this.plContainerBottom.Name = "plContainerBottom";
            this.plContainerBottom.Size = new System.Drawing.Size(524, 247);
            this.plContainerBottom.TabIndex = 5;
            // 
            // ni
            // 
            this.ni.ContextMenuStrip = this.ctmTray;
            this.ni.Icon = ((System.Drawing.Icon)(resources.GetObject("ni.Icon")));
            this.ni.Text = "notifyIcon1";
            this.ni.Visible = true;
            this.ni.DoubleClick += new System.EventHandler(this.ni_DoubleClick);
            // 
            // ctmTray
            // 
            this.ctmTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctmShowMain,
            this.ctmExitSystem});
            this.ctmTray.Name = "ctmTray";
            this.ctmTray.Size = new System.Drawing.Size(152, 48);
            // 
            // ctmShowMain
            // 
            this.ctmShowMain.Name = "ctmShowMain";
            this.ctmShowMain.Size = new System.Drawing.Size(151, 22);
            this.ctmShowMain.Text = "显示主窗口(&S)";
            this.ctmShowMain.Click += new System.EventHandler(this.ctmShowMain_Click);
            // 
            // ctmExitSystem
            // 
            this.ctmExitSystem.Name = "ctmExitSystem";
            this.ctmExitSystem.Size = new System.Drawing.Size(151, 22);
            this.ctmExitSystem.Text = "退出系统(&X)";
            this.ctmExitSystem.Click += new System.EventHandler(this.ctmExitSystem_Click);
            // 
            // frmRobot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 324);
            this.Controls.Add(this.plContainerBottom);
            this.Controls.Add(this.plContainerTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmRobot";
            this.Text = "开奖机器人";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRobot_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.plContainerTop.ResumeLayout(false);
            this.plContainerBottom.ResumeLayout(false);
            this.plContainerBottom.PerformLayout();
            this.ctmTray.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreateData;
        private System.Windows.Forms.Button btnOpenLottery;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.Button btnStopReveal;
        private System.Windows.Forms.Panel plContainerTop;
        private System.Windows.Forms.Panel plContainerBottom;
        private System.Windows.Forms.NotifyIcon ni;
        private System.Windows.Forms.ContextMenuStrip ctmTray;
        private System.Windows.Forms.ToolStripMenuItem ctmShowMain;
        private System.Windows.Forms.ToolStripMenuItem ctmExitSystem;
    }
}

