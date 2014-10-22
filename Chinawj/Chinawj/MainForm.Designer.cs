namespace LeShou
{
    partial class login_form
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(login_form));
            this.webser = new System.Windows.Forms.WebBrowser();
            this.btn_open = new System.Windows.Forms.Button();
            this.tbx_code = new System.Windows.Forms.TextBox();
            this.pic_code = new System.Windows.Forms.PictureBox();
            this.tbx_userid = new System.Windows.Forms.TextBox();
            this.tbx_pwd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_login = new System.Windows.Forms.Button();
            this.timer_load = new System.Windows.Forms.Timer(this.components);
            this.panel_login = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.登录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.发布ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bar_delete = new System.Windows.Forms.ProgressBar();
            this.num_up_dowm_delete = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_end = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.btn_test = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pic_code)).BeginInit();
            this.panel_login.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_up_dowm_delete)).BeginInit();
            this.SuspendLayout();
            // 
            // webser
            // 
            this.webser.Location = new System.Drawing.Point(687, 118);
            this.webser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webser.Name = "webser";
            this.webser.Size = new System.Drawing.Size(286, 255);
            this.webser.TabIndex = 0;
            this.webser.TabStop = false;
            this.webser.WebBrowserShortcutsEnabled = false;
            this.webser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webser_Navigated);
            this.webser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webser_Navigating);
            // 
            // btn_open
            // 
            this.btn_open.Location = new System.Drawing.Point(32, 127);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(44, 23);
            this.btn_open.TabIndex = 2;
            this.btn_open.Text = "刷新";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // tbx_code
            // 
            this.tbx_code.Location = new System.Drawing.Point(88, 90);
            this.tbx_code.Name = "tbx_code";
            this.tbx_code.Size = new System.Drawing.Size(75, 21);
            this.tbx_code.TabIndex = 2;
            // 
            // pic_code
            // 
            this.pic_code.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pic_code.Location = new System.Drawing.Point(169, 86);
            this.pic_code.Name = "pic_code";
            this.pic_code.Size = new System.Drawing.Size(95, 31);
            this.pic_code.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_code.TabIndex = 6;
            this.pic_code.TabStop = false;
            this.pic_code.Click += new System.EventHandler(this.pic_code_Click);
            // 
            // tbx_userid
            // 
            this.tbx_userid.Location = new System.Drawing.Point(88, 34);
            this.tbx_userid.Name = "tbx_userid";
            this.tbx_userid.Size = new System.Drawing.Size(176, 21);
            this.tbx_userid.TabIndex = 0;
            this.tbx_userid.Text = "360xxfb";
            // 
            // tbx_pwd
            // 
            this.tbx_pwd.Location = new System.Drawing.Point(88, 61);
            this.tbx_pwd.Name = "tbx_pwd";
            this.tbx_pwd.PasswordChar = '*';
            this.tbx_pwd.Size = new System.Drawing.Size(176, 21);
            this.tbx_pwd.TabIndex = 1;
            this.tbx_pwd.Text = "h60259594";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "帐号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "密码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "验证码";
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(99, 127);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(45, 23);
            this.btn_login.TabIndex = 3;
            this.btn_login.Text = "登录";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // timer_load
            // 
            this.timer_load.Interval = 1000;
            this.timer_load.Tick += new System.EventHandler(this.timer_load_Tick);
            // 
            // panel_login
            // 
            this.panel_login.Controls.Add(this.btn_test);
            this.panel_login.Controls.Add(this.label1);
            this.panel_login.Controls.Add(this.btn_login);
            this.panel_login.Controls.Add(this.btn_open);
            this.panel_login.Controls.Add(this.label3);
            this.panel_login.Controls.Add(this.tbx_code);
            this.panel_login.Controls.Add(this.label2);
            this.panel_login.Controls.Add(this.pic_code);
            this.panel_login.Controls.Add(this.tbx_userid);
            this.panel_login.Controls.Add(this.tbx_pwd);
            this.panel_login.Location = new System.Drawing.Point(12, 26);
            this.panel_login.Name = "panel_login";
            this.panel_login.Padding = new System.Windows.Forms.Padding(2);
            this.panel_login.Size = new System.Drawing.Size(296, 200);
            this.panel_login.TabIndex = 12;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.登录ToolStripMenuItem,
            this.发布ToolStripMenuItem,
            this.删除ToolStripMenuItem,
            this.删除ToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1044, 25);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 登录ToolStripMenuItem
            // 
            this.登录ToolStripMenuItem.Name = "登录ToolStripMenuItem";
            this.登录ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.登录ToolStripMenuItem.Text = "登录";
            // 
            // 发布ToolStripMenuItem
            // 
            this.发布ToolStripMenuItem.Name = "发布ToolStripMenuItem";
            this.发布ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.发布ToolStripMenuItem.Text = "发布";
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.删除ToolStripMenuItem.Text = "删除";
            // 
            // 删除ToolStripMenuItem1
            // 
            this.删除ToolStripMenuItem1.Name = "删除ToolStripMenuItem1";
            this.删除ToolStripMenuItem1.Size = new System.Drawing.Size(44, 21);
            this.删除ToolStripMenuItem1.Text = "退出";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bar_delete);
            this.panel1.Controls.Add(this.num_up_dowm_delete);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btn_end);
            this.panel1.Controls.Add(this.btn_delete);
            this.panel1.Location = new System.Drawing.Point(328, 28);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2);
            this.panel1.Size = new System.Drawing.Size(296, 200);
            this.panel1.TabIndex = 12;
            // 
            // bar_delete
            // 
            this.bar_delete.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.bar_delete.Location = new System.Drawing.Point(57, 79);
            this.bar_delete.MarqueeAnimationSpeed = 10;
            this.bar_delete.Name = "bar_delete";
            this.bar_delete.Size = new System.Drawing.Size(174, 22);
            this.bar_delete.TabIndex = 11;
            this.bar_delete.Value = 50;
            // 
            // num_up_dowm_delete
            // 
            this.num_up_dowm_delete.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.num_up_dowm_delete.InterceptArrowKeys = false;
            this.num_up_dowm_delete.Location = new System.Drawing.Point(106, 47);
            this.num_up_dowm_delete.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.num_up_dowm_delete.Name = "num_up_dowm_delete";
            this.num_up_dowm_delete.Size = new System.Drawing.Size(125, 21);
            this.num_up_dowm_delete.TabIndex = 10;
            this.num_up_dowm_delete.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.num_up_dowm_delete.ThousandsSeparator = true;
            this.num_up_dowm_delete.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(57, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "条数";
            // 
            // btn_end
            // 
            this.btn_end.Location = new System.Drawing.Point(141, 118);
            this.btn_end.Name = "btn_end";
            this.btn_end.Size = new System.Drawing.Size(61, 33);
            this.btn_end.TabIndex = 3;
            this.btn_end.Text = "结束";
            this.btn_end.UseVisualStyleBackColor = true;
            // 
            // btn_delete
            // 
            this.btn_delete.Location = new System.Drawing.Point(57, 118);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(61, 33);
            this.btn_delete.TabIndex = 2;
            this.btn_delete.Text = "删除";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_test
            // 
            this.btn_test.Location = new System.Drawing.Point(169, 130);
            this.btn_test.Name = "btn_test";
            this.btn_test.Size = new System.Drawing.Size(45, 23);
            this.btn_test.TabIndex = 12;
            this.btn_test.Text = "登录";
            this.btn_test.UseVisualStyleBackColor = true;
            this.btn_test.Click += new System.EventHandler(this.btn_test_Click);
            // 
            // login_form
            // 
            this.AcceptButton = this.btn_login;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1044, 461);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel_login);
            this.Controls.Add(this.webser);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "login_form";
            this.Text = "中国五金";
            this.Load += new System.EventHandler(this.login_form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic_code)).EndInit();
            this.panel_login.ResumeLayout(false);
            this.panel_login.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_up_dowm_delete)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webser;
        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.TextBox tbx_code;
        private System.Windows.Forms.PictureBox pic_code;
        private System.Windows.Forms.TextBox tbx_userid;
        private System.Windows.Forms.TextBox tbx_pwd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.Timer timer_load;
        private System.Windows.Forms.Panel panel_login;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 登录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 发布ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_end;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.NumericUpDown num_up_dowm_delete;
        private System.Windows.Forms.ProgressBar bar_delete;
        private System.Windows.Forms.Button btn_test;
    }
}

