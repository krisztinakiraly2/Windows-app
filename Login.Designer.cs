using System.Windows.Forms;

namespace WindowsApp
{
    partial class Login
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
            ContentPanel = new ToolStripContentPanel();
            log_in = new Button();
            sign_up = new Button();
            splitContainer1 = new SplitContainer();
            error_li = new Label();
            label3 = new Label();
            password_li = new TextBox();
            email_li = new TextBox();
            label2 = new Label();
            label1 = new Label();
            checkBox = new CheckBox();
            error_su = new Label();
            password_su = new TextBox();
            label8 = new Label();
            username_su = new TextBox();
            label7 = new Label();
            email_su = new TextBox();
            label6 = new Label();
            name_su = new TextBox();
            label5 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // ContentPanel
            // 
            ContentPanel.Size = new Size(775, 450);
            // 
            // log_in
            // 
            log_in.BackColor = SystemColors.ControlDarkDark;
            log_in.FlatAppearance.MouseDownBackColor = Color.Gray;
            log_in.FlatAppearance.MouseOverBackColor = Color.Gray;
            log_in.ForeColor = SystemColors.ButtonFace;
            log_in.Location = new Point(85, 285);
            log_in.Name = "log_in";
            log_in.Size = new Size(105, 50);
            log_in.TabIndex = 0;
            log_in.Text = "Log in";
            log_in.UseVisualStyleBackColor = false;
            log_in.Click += log_in_Click;
            // 
            // sign_up
            // 
            sign_up.BackColor = SystemColors.ControlDarkDark;
            sign_up.FlatAppearance.MouseDownBackColor = Color.Silver;
            sign_up.FlatAppearance.MouseOverBackColor = Color.Silver;
            sign_up.ForeColor = SystemColors.ButtonFace;
            sign_up.Location = new Point(217, 285);
            sign_up.Name = "sign_up";
            sign_up.Size = new Size(105, 50);
            sign_up.TabIndex = 1;
            sign_up.Text = "Sign up";
            sign_up.UseVisualStyleBackColor = false;
            sign_up.Click += sign_up_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(error_li);
            splitContainer1.Panel1.Controls.Add(label3);
            splitContainer1.Panel1.Controls.Add(password_li);
            splitContainer1.Panel1.Controls.Add(email_li);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(checkBox);
            splitContainer1.Panel1.Controls.Add(log_in);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(error_su);
            splitContainer1.Panel2.Controls.Add(password_su);
            splitContainer1.Panel2.Controls.Add(label8);
            splitContainer1.Panel2.Controls.Add(username_su);
            splitContainer1.Panel2.Controls.Add(label7);
            splitContainer1.Panel2.Controls.Add(email_su);
            splitContainer1.Panel2.Controls.Add(label6);
            splitContainer1.Panel2.Controls.Add(name_su);
            splitContainer1.Panel2.Controls.Add(label5);
            splitContainer1.Panel2.Controls.Add(label4);
            splitContainer1.Panel2.Controls.Add(sign_up);
            splitContainer1.Size = new Size(836, 347);
            splitContainer1.SplitterDistance = 275;
            splitContainer1.TabIndex = 3;
            // 
            // error_li
            // 
            error_li.AutoSize = true;
            error_li.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            error_li.ForeColor = Color.Red;
            error_li.Location = new Point(13, 57);
            error_li.Name = "error_li";
            error_li.Size = new Size(44, 20);
            error_li.TabIndex = 7;
            error_li.Text = "Error";
            error_li.Visible = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label3.ForeColor = SystemColors.ControlLight;
            label3.Location = new Point(85, 9);
            label3.Name = "label3";
            label3.Size = new Size(96, 37);
            label3.TabIndex = 6;
            label3.Text = "Log in";
            // 
            // password_li
            // 
            password_li.BackColor = SystemColors.ControlDarkDark;
            password_li.ForeColor = SystemColors.ScrollBar;
            password_li.Location = new Point(13, 183);
            password_li.Name = "password_li";
            password_li.Size = new Size(248, 27);
            password_li.TabIndex = 5;
            password_li.Text = "password";
            password_li.UseSystemPasswordChar = true;
            password_li.Click += password_li_Del_OnClick;
            // 
            // email_li
            // 
            email_li.BackColor = SystemColors.ControlDarkDark;
            email_li.ForeColor = SystemColors.ScrollBar;
            email_li.Location = new Point(14, 106);
            email_li.Name = "email_li";
            email_li.Size = new Size(248, 27);
            email_li.TabIndex = 4;
            email_li.Text = "email or username";
            email_li.Click += email_li_Del_OnClick;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ControlLight;
            label2.Location = new Point(12, 154);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 3;
            label2.Text = "Password";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.ControlLight;
            label1.Location = new Point(13, 77);
            label1.Name = "label1";
            label1.Size = new Size(134, 20);
            label1.TabIndex = 2;
            label1.Text = "Email or Username";
            // 
            // checkBox
            // 
            checkBox.AutoSize = true;
            checkBox.BackColor = Color.Transparent;
            checkBox.Checked = true;
            checkBox.CheckState = CheckState.Checked;
            checkBox.ForeColor = SystemColors.ControlLight;
            checkBox.Location = new Point(14, 218);
            checkBox.Name = "checkBox";
            checkBox.Size = new Size(158, 24);
            checkBox.TabIndex = 1;
            checkBox.Text = "Keep me logged in";
            checkBox.UseVisualStyleBackColor = false;
            // 
            // error_su
            // 
            error_su.AutoSize = true;
            error_su.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            error_su.ForeColor = Color.Red;
            error_su.Location = new Point(14, 57);
            error_su.Name = "error_su";
            error_su.Size = new Size(44, 20);
            error_su.TabIndex = 8;
            error_su.Text = "Error";
            error_su.Visible = false;
            // 
            // password_su
            // 
            password_su.BackColor = SystemColors.ControlDarkDark;
            password_su.ForeColor = SystemColors.ScrollBar;
            password_su.Location = new Point(297, 183);
            password_su.Name = "password_su";
            password_su.Size = new Size(248, 27);
            password_su.TabIndex = 16;
            password_su.Text = "Password";
            password_su.UseSystemPasswordChar = true;
            password_su.Click += password_su_Del_OnClick;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = SystemColors.ControlLight;
            label8.Location = new Point(296, 154);
            label8.Name = "label8";
            label8.Size = new Size(70, 20);
            label8.TabIndex = 15;
            label8.Text = "Password";
            // 
            // username_su
            // 
            username_su.BackColor = SystemColors.ControlDarkDark;
            username_su.ForeColor = SystemColors.ScrollBar;
            username_su.Location = new Point(14, 183);
            username_su.Name = "username_su";
            username_su.Size = new Size(248, 27);
            username_su.TabIndex = 14;
            username_su.Text = "Optional Username";
            username_su.Click += username_su_Del_OnClick;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ForeColor = SystemColors.ControlLight;
            label7.Location = new Point(13, 154);
            label7.Name = "label7";
            label7.Size = new Size(137, 20);
            label7.TabIndex = 13;
            label7.Text = "Optional Username";
            // 
            // email_su
            // 
            email_su.BackColor = SystemColors.ControlDarkDark;
            email_su.ForeColor = SystemColors.ScrollBar;
            email_su.Location = new Point(297, 106);
            email_su.Name = "email_su";
            email_su.Size = new Size(248, 27);
            email_su.TabIndex = 12;
            email_su.Text = "Email";
            email_su.Click += email_su_Del_OnClick;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = SystemColors.ControlLight;
            label6.Location = new Point(296, 77);
            label6.Name = "label6";
            label6.Size = new Size(46, 20);
            label6.TabIndex = 11;
            label6.Text = "Email";
            label6.Click += email_su_Del_OnClick;
            // 
            // name_su
            // 
            name_su.BackColor = SystemColors.ControlDarkDark;
            name_su.ForeColor = SystemColors.ScrollBar;
            name_su.Location = new Point(14, 106);
            name_su.Name = "name_su";
            name_su.Size = new Size(248, 27);
            name_su.TabIndex = 10;
            name_su.Text = "Name";
            name_su.Click += name_su_Del_OnClick;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = SystemColors.ControlLight;
            label5.Location = new Point(13, 77);
            label5.Name = "label5";
            label5.Size = new Size(119, 20);
            label5.TabIndex = 9;
            label5.Text = "Displayed Name";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label4.ForeColor = SystemColors.ControlLight;
            label4.Location = new Point(223, 9);
            label4.Name = "label4";
            label4.Size = new Size(113, 37);
            label4.TabIndex = 8;
            label4.Text = "Sign up";
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(836, 347);
            Controls.Add(splitContainer1);
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private ToolStripContentPanel ContentPanel;
        private Button log_in;
        private Button sign_up;
        private SplitContainer splitContainer1;
        private TextBox email_li;
        private Label label2;
        private Label label1;
        private CheckBox checkBox;
        private Label label3;
        private TextBox password_li;
        private Label error_li;
        private TextBox name_su;
        private Label label5;
        private Label label4;
        private TextBox email_su;
        private Label label6;
        private Label error_su;
        private TextBox password_su;
        private Label label8;
        private TextBox username_su;
        private Label label7;
    }
}