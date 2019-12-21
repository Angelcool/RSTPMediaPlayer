namespace videoPlayer
{
    partial class frmMain
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

        private System.ComponentModel.ComponentResourceManager resources;

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.panel2 = new System.Windows.Forms.Panel();
            this.videoPlayerParentPanel = new System.Windows.Forms.Panel();
            this.play_list_panel = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.PName = new System.Windows.Forms.Label();
            this.topPanel = new System.Windows.Forms.Panel();
            this.screen_sixteen = new System.Windows.Forms.Button();
            this.promptBox = new System.Windows.Forms.Label();
            this.to_split = new System.Windows.Forms.Button();
            this.playList = new System.Windows.Forms.Button();
            this.screen_nine = new System.Windows.Forms.Button();
            this.screen_six = new System.Windows.Forms.Button();
            this.screen_four = new System.Windows.Forms.Button();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.forword = new System.Windows.Forms.Button();
            this.winMin = new System.Windows.Forms.Button();
            this.winMax = new System.Windows.Forms.Button();
            this.winClose = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PlayListBrowser = new AxSHDocVw.AxWebBrowser();
            this.panel2.SuspendLayout();
            this.videoPlayerParentPanel.SuspendLayout();
            this.play_list_panel.SuspendLayout();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayListBrowser)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(57)))), ((int)(((byte)(65)))));
            this.panel2.Controls.Add(this.videoPlayerParentPanel);
            this.panel2.Location = new System.Drawing.Point(0, 46);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1336, 720);
            this.panel2.TabIndex = 1;
            // 
            // videoPlayerParentPanel
            // 
            this.videoPlayerParentPanel.BackColor = System.Drawing.Color.Black;
            this.videoPlayerParentPanel.Controls.Add(this.play_list_panel);
            this.videoPlayerParentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoPlayerParentPanel.ForeColor = System.Drawing.Color.Transparent;
            this.videoPlayerParentPanel.Location = new System.Drawing.Point(0, 0);
            this.videoPlayerParentPanel.Margin = new System.Windows.Forms.Padding(0);
            this.videoPlayerParentPanel.Name = "videoPlayerParentPanel";
            this.videoPlayerParentPanel.Size = new System.Drawing.Size(1336, 720);
            this.videoPlayerParentPanel.TabIndex = 3;
            this.videoPlayerParentPanel.SizeChanged += new System.EventHandler(this.videoPlayerParentPanel_SizeChanged);
            // 
            // play_list_panel
            // 
            this.play_list_panel.Controls.Add(this.PlayListBrowser);
            this.play_list_panel.Location = new System.Drawing.Point(1033, 0);
            this.play_list_panel.Name = "play_list_panel";
            this.play_list_panel.Size = new System.Drawing.Size(303, 720);
            this.play_list_panel.TabIndex = 1;
            this.play_list_panel.Visible = false;
            this.play_list_panel.Paint += new System.Windows.Forms.PaintEventHandler(this.Play_list_panel_Paint);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "PDT播放器";
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // PName
            // 
            this.PName.AutoSize = true;
            this.PName.Font = new System.Drawing.Font("微软雅黑 Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PName.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.PName.Location = new System.Drawing.Point(31, 7);
            this.PName.Name = "PName";
            this.PName.Size = new System.Drawing.Size(96, 28);
            this.PName.TabIndex = 0;
            this.PName.Text = "融合通信";
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.topPanel.Controls.Add(this.screen_sixteen);
            this.topPanel.Controls.Add(this.promptBox);
            this.topPanel.Controls.Add(this.to_split);
            this.topPanel.Controls.Add(this.playList);
            this.topPanel.Controls.Add(this.screen_nine);
            this.topPanel.Controls.Add(this.screen_six);
            this.topPanel.Controls.Add(this.screen_four);
            this.topPanel.Controls.Add(this.logoPictureBox);
            this.topPanel.Controls.Add(this.PName);
            this.topPanel.Controls.Add(this.forword);
            this.topPanel.Controls.Add(this.winMin);
            this.topPanel.Controls.Add(this.winMax);
            this.topPanel.Controls.Add(this.winClose);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Margin = new System.Windows.Forms.Padding(0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1336, 46);
            this.topPanel.TabIndex = 0;
            this.topPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // screen_sixteen
            // 
            this.screen_sixteen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.screen_sixteen.BackColor = System.Drawing.Color.Transparent;
            this.screen_sixteen.BackgroundImage = global::videoPlayer.Properties.Resources.screen_sixteen;
            this.screen_sixteen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.screen_sixteen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.screen_sixteen.FlatAppearance.BorderSize = 0;
            this.screen_sixteen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.screen_sixteen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.screen_sixteen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.screen_sixteen.Location = new System.Drawing.Point(1142, 11);
            this.screen_sixteen.Name = "screen_sixteen";
            this.screen_sixteen.Size = new System.Drawing.Size(27, 18);
            this.screen_sixteen.TabIndex = 11;
            this.screen_sixteen.TabStop = false;
            this.screen_sixteen.UseVisualStyleBackColor = false;
            this.screen_sixteen.Visible = false;
            this.screen_sixteen.Click += new System.EventHandler(this.Screen_sixteen_Click);
            // 
            // promptBox
            // 
            this.promptBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.promptBox.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.promptBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.promptBox.Location = new System.Drawing.Point(143, 14);
            this.promptBox.Name = "promptBox";
            this.promptBox.Size = new System.Drawing.Size(147, 22);
            this.promptBox.TabIndex = 10;
            this.promptBox.Visible = false;
            this.promptBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // to_split
            // 
            this.to_split.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.to_split.BackColor = System.Drawing.Color.Transparent;
            this.to_split.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("to_split.BackgroundImage")));
            this.to_split.Cursor = System.Windows.Forms.Cursors.Hand;
            this.to_split.FlatAppearance.BorderSize = 0;
            this.to_split.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.to_split.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.to_split.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.to_split.Location = new System.Drawing.Point(1218, 8);
            this.to_split.Name = "to_split";
            this.to_split.Size = new System.Drawing.Size(23, 24);
            this.to_split.TabIndex = 9;
            this.to_split.TabStop = false;
            this.to_split.UseVisualStyleBackColor = false;
            this.to_split.Visible = false;
            this.to_split.Click += new System.EventHandler(this.forword_Click);
            // 
            // playList
            // 
            this.playList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.playList.BackColor = System.Drawing.Color.Transparent;
            this.playList.BackgroundImage = global::videoPlayer.Properties.Resources.playlist;
            this.playList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.playList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.playList.FlatAppearance.BorderSize = 0;
            this.playList.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.playList.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.playList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.playList.Location = new System.Drawing.Point(1205, 11);
            this.playList.Name = "playList";
            this.playList.Size = new System.Drawing.Size(27, 18);
            this.playList.TabIndex = 8;
            this.playList.TabStop = false;
            this.playList.UseVisualStyleBackColor = false;
            this.playList.Visible = false;
            this.playList.Click += new System.EventHandler(this.PlayList_Click);
            // 
            // screen_nine
            // 
            this.screen_nine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.screen_nine.BackColor = System.Drawing.Color.Transparent;
            this.screen_nine.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("screen_nine.BackgroundImage")));
            this.screen_nine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.screen_nine.Cursor = System.Windows.Forms.Cursors.Hand;
            this.screen_nine.FlatAppearance.BorderSize = 0;
            this.screen_nine.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.screen_nine.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.screen_nine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.screen_nine.Location = new System.Drawing.Point(1092, 11);
            this.screen_nine.Name = "screen_nine";
            this.screen_nine.Size = new System.Drawing.Size(27, 18);
            this.screen_nine.TabIndex = 8;
            this.screen_nine.TabStop = false;
            this.screen_nine.UseVisualStyleBackColor = false;
            this.screen_nine.Visible = false;
            this.screen_nine.Click += new System.EventHandler(this.Screen_nine_Click);
            // 
            // screen_six
            // 
            this.screen_six.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.screen_six.BackColor = System.Drawing.Color.Transparent;
            this.screen_six.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("screen_six.BackgroundImage")));
            this.screen_six.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.screen_six.Cursor = System.Windows.Forms.Cursors.Hand;
            this.screen_six.FlatAppearance.BorderSize = 0;
            this.screen_six.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.screen_six.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.screen_six.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.screen_six.Location = new System.Drawing.Point(1037, 11);
            this.screen_six.Name = "screen_six";
            this.screen_six.Size = new System.Drawing.Size(33, 18);
            this.screen_six.TabIndex = 7;
            this.screen_six.TabStop = false;
            this.screen_six.UseVisualStyleBackColor = false;
            this.screen_six.Visible = false;
            this.screen_six.Click += new System.EventHandler(this.Screen_six_Click);
            // 
            // screen_four
            // 
            this.screen_four.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.screen_four.BackColor = System.Drawing.Color.Transparent;
            this.screen_four.BackgroundImage = global::videoPlayer.Properties.Resources.screen_four_active;
            this.screen_four.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.screen_four.Cursor = System.Windows.Forms.Cursors.Hand;
            this.screen_four.FlatAppearance.BorderSize = 0;
            this.screen_four.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.screen_four.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.screen_four.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.screen_four.Location = new System.Drawing.Point(982, 11);
            this.screen_four.Name = "screen_four";
            this.screen_four.Size = new System.Drawing.Size(33, 18);
            this.screen_four.TabIndex = 6;
            this.screen_four.TabStop = false;
            this.screen_four.UseVisualStyleBackColor = false;
            this.screen_four.Visible = false;
            this.screen_four.Click += new System.EventHandler(this.Screen_four_Click);
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.BackgroundImage = global::videoPlayer.Properties.Resources.videoPcLogo;
            this.logoPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.logoPictureBox.Location = new System.Drawing.Point(13, 13);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(13, 18);
            this.logoPictureBox.TabIndex = 5;
            this.logoPictureBox.TabStop = false;
            // 
            // forword
            // 
            this.forword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.forword.BackColor = System.Drawing.Color.Transparent;
            this.forword.BackgroundImage = global::videoPlayer.Properties.Resources.forword_1;
            this.forword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.forword.FlatAppearance.BorderSize = 0;
            this.forword.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.forword.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.forword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.forword.Location = new System.Drawing.Point(929, 11);
            this.forword.Name = "forword";
            this.forword.Size = new System.Drawing.Size(15, 18);
            this.forword.TabIndex = 2;
            this.forword.TabStop = false;
            this.forword.UseVisualStyleBackColor = false;
            this.forword.Click += new System.EventHandler(this.forword_Click);
            // 
            // winMin
            // 
            this.winMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.winMin.BackColor = System.Drawing.Color.Transparent;
            this.winMin.BackgroundImage = global::videoPlayer.Properties.Resources.min;
            this.winMin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.winMin.FlatAppearance.BorderSize = 0;
            this.winMin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.winMin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.winMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.winMin.Location = new System.Drawing.Point(1257, 11);
            this.winMin.Name = "winMin";
            this.winMin.Size = new System.Drawing.Size(11, 11);
            this.winMin.TabIndex = 2;
            this.winMin.TabStop = false;
            this.winMin.UseVisualStyleBackColor = false;
            this.winMin.Click += new System.EventHandler(this.button3_Click);
            // 
            // winMax
            // 
            this.winMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.winMax.BackColor = System.Drawing.Color.Transparent;
            this.winMax.BackgroundImage = global::videoPlayer.Properties.Resources.max;
            this.winMax.Cursor = System.Windows.Forms.Cursors.Hand;
            this.winMax.FlatAppearance.BorderSize = 0;
            this.winMax.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.winMax.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.winMax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.winMax.Location = new System.Drawing.Point(1287, 11);
            this.winMax.Name = "winMax";
            this.winMax.Size = new System.Drawing.Size(11, 11);
            this.winMax.TabIndex = 1;
            this.winMax.TabStop = false;
            this.winMax.UseVisualStyleBackColor = false;
            this.winMax.Click += new System.EventHandler(this.button2_Click);
            // 
            // winClose
            // 
            this.winClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.winClose.BackColor = System.Drawing.Color.Transparent;
            this.winClose.BackgroundImage = global::videoPlayer.Properties.Resources.close1;
            this.winClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.winClose.FlatAppearance.BorderSize = 0;
            this.winClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.winClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.winClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.winClose.Location = new System.Drawing.Point(1313, 11);
            this.winClose.Name = "winClose";
            this.winClose.Size = new System.Drawing.Size(11, 11);
            this.winClose.TabIndex = 3;
            this.winClose.TabStop = false;
            this.winClose.UseVisualStyleBackColor = false;
            this.winClose.Click += new System.EventHandler(this.button1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // PlayListBrowser
            // 
            this.PlayListBrowser.Enabled = true;
            this.PlayListBrowser.Location = new System.Drawing.Point(3, 3);
            this.PlayListBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("PlayListBrowser.OcxState")));
            this.PlayListBrowser.Size = new System.Drawing.Size(300, 720);
            this.PlayListBrowser.TabIndex = 0;
            this.PlayListBrowser.Enter += new System.EventHandler(this.PlayListBrowser_Enter);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(57)))), ((int)(((byte)(65)))));
            this.ClientSize = new System.Drawing.Size(1336, 766);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.topPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PDT播放器";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.panel2.ResumeLayout(false);
            this.videoPlayerParentPanel.ResumeLayout(false);
            this.play_list_panel.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlayListBrowser)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Panel videoPlayerParentPanel;
        private System.Windows.Forms.Button winClose;
        private System.Windows.Forms.Button winMax;
        private System.Windows.Forms.Button winMin;
        private System.Windows.Forms.Label PName;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Button forword;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Button screen_nine;
        private System.Windows.Forms.Button screen_six;
        private System.Windows.Forms.Button screen_four;
        private AxSHDocVw.AxWebBrowser PlayListBrowser;
        private System.Windows.Forms.Button playList;
        private System.Windows.Forms.Panel play_list_panel;
        private System.Windows.Forms.Button to_split;
        private System.Windows.Forms.Label promptBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button screen_sixteen;
    }
}

