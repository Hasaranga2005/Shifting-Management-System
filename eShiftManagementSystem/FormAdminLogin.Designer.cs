namespace eShiftManagementSystem
{
    partial class frmAdminLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdminLogin));
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblAdminIncorrectPassword = new System.Windows.Forms.Label();
            this.lblAdminUserNotFound = new System.Windows.Forms.Label();
            this.lblAdminPasswordRequired = new System.Windows.Forms.Label();
            this.lblAdminUsernameRequired = new System.Windows.Forms.Label();
            this.panelAdminPassword = new System.Windows.Forms.Panel();
            this.panelAdminUsername = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtAdminUsername = new System.Windows.Forms.TextBox();
            this.lblpassword = new System.Windows.Forms.Label();
            this.lblusername = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.btnAdminLogin = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Red;
            this.panel5.Location = new System.Drawing.Point(700, 177);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(113, 756);
            this.panel5.TabIndex = 8;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Blue;
            this.panel4.Location = new System.Drawing.Point(700, 828);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(644, 105);
            this.panel4.TabIndex = 9;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.Blue;
            this.panel7.Location = new System.Drawing.Point(1233, 177);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(116, 753);
            this.panel7.TabIndex = 10;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Red;
            this.panel3.Location = new System.Drawing.Point(700, 177);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(538, 100);
            this.panel3.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblAdminIncorrectPassword);
            this.panel1.Controls.Add(this.lblAdminUserNotFound);
            this.panel1.Controls.Add(this.lblAdminPasswordRequired);
            this.panel1.Controls.Add(this.lblAdminUsernameRequired);
            this.panel1.Controls.Add(this.panelAdminPassword);
            this.panel1.Controls.Add(this.panelAdminUsername);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.txtAdminUsername);
            this.panel1.Controls.Add(this.lblpassword);
            this.panel1.Controls.Add(this.lblusername);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pbClose);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox4);
            this.panel1.Controls.Add(this.btnAdminLogin);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.panel1.Location = new System.Drawing.Point(-15, -13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(641, 785);
            this.panel1.TabIndex = 5;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // lblAdminIncorrectPassword
            // 
            this.lblAdminIncorrectPassword.AutoSize = true;
            this.lblAdminIncorrectPassword.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdminIncorrectPassword.ForeColor = System.Drawing.Color.Red;
            this.lblAdminIncorrectPassword.Location = new System.Drawing.Point(361, 562);
            this.lblAdminIncorrectPassword.Name = "lblAdminIncorrectPassword";
            this.lblAdminIncorrectPassword.Size = new System.Drawing.Size(147, 23);
            this.lblAdminIncorrectPassword.TabIndex = 42;
            this.lblAdminIncorrectPassword.Text = "Incorrect password";
            this.lblAdminIncorrectPassword.Visible = false;
            // 
            // lblAdminUserNotFound
            // 
            this.lblAdminUserNotFound.AutoSize = true;
            this.lblAdminUserNotFound.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdminUserNotFound.ForeColor = System.Drawing.Color.Red;
            this.lblAdminUserNotFound.Location = new System.Drawing.Point(346, 452);
            this.lblAdminUserNotFound.Name = "lblAdminUserNotFound";
            this.lblAdminUserNotFound.Size = new System.Drawing.Size(162, 23);
            this.lblAdminUserNotFound.TabIndex = 41;
            this.lblAdminUserNotFound.Text = "Username not found";
            this.lblAdminUserNotFound.Visible = false;
            // 
            // lblAdminPasswordRequired
            // 
            this.lblAdminPasswordRequired.AutoSize = true;
            this.lblAdminPasswordRequired.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdminPasswordRequired.ForeColor = System.Drawing.Color.Red;
            this.lblAdminPasswordRequired.Location = new System.Drawing.Point(299, 562);
            this.lblAdminPasswordRequired.Name = "lblAdminPasswordRequired";
            this.lblAdminPasswordRequired.Size = new System.Drawing.Size(209, 23);
            this.lblAdminPasswordRequired.TabIndex = 40;
            this.lblAdminPasswordRequired.Text = "Please enter your password";
            this.lblAdminPasswordRequired.Visible = false;
            // 
            // lblAdminUsernameRequired
            // 
            this.lblAdminUsernameRequired.AutoSize = true;
            this.lblAdminUsernameRequired.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdminUsernameRequired.ForeColor = System.Drawing.Color.Red;
            this.lblAdminUsernameRequired.Location = new System.Drawing.Point(295, 452);
            this.lblAdminUsernameRequired.Name = "lblAdminUsernameRequired";
            this.lblAdminUsernameRequired.Size = new System.Drawing.Size(213, 23);
            this.lblAdminUsernameRequired.TabIndex = 34;
            this.lblAdminUsernameRequired.Text = "Please enter your username";
            this.lblAdminUsernameRequired.Visible = false;
            // 
            // panelAdminPassword
            // 
            this.panelAdminPassword.Location = new System.Drawing.Point(156, 554);
            this.panelAdminPassword.Name = "panelAdminPassword";
            this.panelAdminPassword.Size = new System.Drawing.Size(352, 2);
            this.panelAdminPassword.TabIndex = 39;
            // 
            // panelAdminUsername
            // 
            this.panelAdminUsername.Location = new System.Drawing.Point(156, 444);
            this.panelAdminUsername.Name = "panelAdminUsername";
            this.panelAdminUsername.Size = new System.Drawing.Size(352, 2);
            this.panelAdminUsername.TabIndex = 38;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.Transparent;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(449, 506);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(59, 49);
            this.button2.TabIndex = 24;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.Transparent;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(449, 506);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 49);
            this.button1.TabIndex = 23;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel8
            // 
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Location = new System.Drawing.Point(155, 558);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(353, 1);
            this.panel8.TabIndex = 20;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI Semibold", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtPassword.Location = new System.Drawing.Point(168, 526);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(281, 30);
            this.txtPassword.TabIndex = 19;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Location = new System.Drawing.Point(155, 448);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(353, 1);
            this.panel6.TabIndex = 18;
            // 
            // txtAdminUsername
            // 
            this.txtAdminUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAdminUsername.Font = new System.Drawing.Font("Segoe UI Semibold", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAdminUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtAdminUsername.Location = new System.Drawing.Point(168, 416);
            this.txtAdminUsername.Name = "txtAdminUsername";
            this.txtAdminUsername.Size = new System.Drawing.Size(362, 30);
            this.txtAdminUsername.TabIndex = 16;
            this.txtAdminUsername.TextChanged += new System.EventHandler(this.txtAdminUsername_TextChanged);
            // 
            // lblpassword
            // 
            this.lblpassword.AutoSize = true;
            this.lblpassword.Font = new System.Drawing.Font("Segoe UI Black", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblpassword.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblpassword.Location = new System.Drawing.Point(149, 492);
            this.lblpassword.Name = "lblpassword";
            this.lblpassword.Size = new System.Drawing.Size(142, 30);
            this.lblpassword.TabIndex = 15;
            this.lblpassword.Text = "PASSWORD";
            // 
            // lblusername
            // 
            this.lblusername.AutoSize = true;
            this.lblusername.Font = new System.Drawing.Font("Segoe UI Black", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblusername.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblusername.Location = new System.Drawing.Point(149, 382);
            this.lblusername.Name = "lblusername";
            this.lblusername.Size = new System.Drawing.Size(138, 30);
            this.lblusername.TabIndex = 14;
            this.lblusername.Text = "USERNAME";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureBox1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.ErrorImage")));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(14, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(65, 59);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            // 
            // pbClose
            // 
            this.pbClose.Cursor = System.Windows.Forms.Cursors.Default;
            this.pbClose.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pbClose.ErrorImage")));
            this.pbClose.Image = ((System.Drawing.Image)(resources.GetObject("pbClose.Image")));
            this.pbClose.InitialImage = ((System.Drawing.Image)(resources.GetObject("pbClose.InitialImage")));
            this.pbClose.Location = new System.Drawing.Point(576, 12);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(65, 59);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbClose.TabIndex = 3;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            this.pbClose.MouseEnter += new System.EventHandler(this.pbClose_MouseEnter);
            this.pbClose.MouseLeave += new System.EventHandler(this.pbClose_MouseLeave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Black", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(200, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 19);
            this.label1.TabIndex = 12;
            this.label1.Text = "-SHIFT";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(264, 177);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(159, 176);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 10;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Click += new System.EventHandler(this.pictureBox4_Click);
            // 
            // btnAdminLogin
            // 
            this.btnAdminLogin.BackColor = System.Drawing.Color.Red;
            this.btnAdminLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdminLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdminLogin.Font = new System.Drawing.Font("Segoe UI Black", 16F, System.Drawing.FontStyle.Bold);
            this.btnAdminLogin.ForeColor = System.Drawing.Color.White;
            this.btnAdminLogin.Location = new System.Drawing.Point(191, 626);
            this.btnAdminLogin.Name = "btnAdminLogin";
            this.btnAdminLogin.Size = new System.Drawing.Size(285, 77);
            this.btnAdminLogin.TabIndex = 8;
            this.btnAdminLogin.Text = "LOGIN";
            this.btnAdminLogin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAdminLogin.UseVisualStyleBackColor = false;
            this.btnAdminLogin.Click += new System.EventHandler(this.btnAdminLogin_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Black", 20.2F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Red;
            this.lblTitle.Location = new System.Drawing.Point(284, 94);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(264, 46);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "ADMIN LOGIN";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(143, 66);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(173, 105);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel1);
            this.panel2.ForeColor = System.Drawing.Color.PapayaWhip;
            this.panel2.Location = new System.Drawing.Point(711, 186);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(627, 737);
            this.panel2.TabIndex = 6;
            // 
            // frmAdminLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1879, 1054);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel7);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "frmAdminLogin";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormAdminLogin";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Button btnAdminLogin;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblpassword;
        private System.Windows.Forms.Label lblusername;
        private System.Windows.Forms.TextBox txtAdminUsername;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panelAdminPassword;
        private System.Windows.Forms.Panel panelAdminUsername;
        private System.Windows.Forms.Label lblAdminUserNotFound;
        private System.Windows.Forms.Label lblAdminPasswordRequired;
        private System.Windows.Forms.Label lblAdminUsernameRequired;
        private System.Windows.Forms.Label lblAdminIncorrectPassword;
    }
}