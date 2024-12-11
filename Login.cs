using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsApp
{
    public partial class Login : Form
    {
        public event Action<string> LoginSuccessful;

        public Login()
        {
            InitializeComponent();

            BackColor = Color.FromArgb(30, 30, 30);
            ContentPanel.RenderMode = ToolStripRenderMode.Professional;
        }

        private void log_in_Click(object sender, EventArgs e)
        {
            error_li.ForeColor = Color.Red;

            if (email_li.Text == string.Empty)
            {
                error_li.Text = "Email is not set";
                error_li.Visible = true;
                return;
            }

            if (password_li.Text == string.Empty)
            {
                error_li.Text = "Password is not set";
                error_li.Visible = true;
                return;
            }

            string email = (email_li.Text.Contains('@')) ? email_li.Text : string.Empty;
            string username = (email == string.Empty) ? email_li.Text : string.Empty;
            string password;
            string usersFilePath = Program.projectFolderPath + "\\users.txt";

            bool userFound = false;

            string[] lines = File.ReadAllLines(usersFilePath);
            int lineIndex = -1;

            if (email == string.Empty)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    bool contains = line.Contains("username:");
                    if (contains)
                    {
                        string currUserName = line.Substring(line.IndexOf("username:") + "username: ".Length).Trim();
                        if (currUserName == username)
                        {
                            lineIndex = i-1;
                            userFound = true;
                            email = lines[lineIndex].Substring(lines[lineIndex].IndexOf("email:") + "email: ".Length).Trim();
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    bool contains = line.Contains("email:");
                    if (contains)
                    {
                        string currEmail = line.Substring(line.IndexOf("email:") + "email: ".Length).Trim();
                        if (currEmail == email)
                        {
                            lineIndex = i;
                            userFound = true;
                            break;
                        }
                    }
                }
            }

            if (!userFound)
            {
                error_li.Text = "User not found!";
                error_li.Visible = true;
                return;
            }
            else
            {
                password = lines[lineIndex+2].Substring(lines[lineIndex+2].IndexOf("password:") + "password: ".Length).Trim();

                if (password != password_li.Text)
                {
                    error_li.Text = "Password is not correct";
                    error_li.Visible = true;
                    return;
                }
                else
                {
                    if (checkBox.Checked)
                    {
                        string defaultUserFilePath = Program.projectFolderPath+"\\defaultuser.txt";

                        if (!File.Exists(defaultUserFilePath))
                        {
                            using (File.Create(defaultUserFilePath)) { }
                        }

                        using StreamWriter writer = new StreamWriter(defaultUserFilePath);
                        writer.Write("email: " + email + "\n");
                        writer.Flush();
                        writer.Close();
                    }

                    LoginSuccessful?.Invoke(email);
                }
            }
        }

        private void sign_up_Click(object sender, EventArgs e)
        {
            error_su.ForeColor = Color.Red;
            
            if(name_su.Text == string.Empty)
            {
                error_su.Text = "Name is not set";
                error_su.Visible = true;
                return;
            }

            if (email_su.Text == string.Empty)
            {
                error_su.Text = "Email is not set";
                error_su.Visible = true;
                return;
            }

            if(!email_su.Text.Contains('@'))
            {
                error_su.Text = "Use valid email address";
                error_su.Visible = true;
                return;
            }

            if (password_su.Text == string.Empty || password_su.Text == "Password")
            {
                error_su.Text = "Password is not set";
                error_su.Visible = true;
                return;
            }

            bool profileExist = false;
            bool usernameExist = false;

            if (username_su.Text == "Optional Username")
                username_su.Text = string.Empty;

            string filePath = Program.projectFolderPath + "\\users.txt";

            if(File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (line.Contains("email: "+email_su.Text))
                    {
                        profileExist = true;
                        break;
                    }

                    if (username_su.Text != string.Empty)
                    {
                        if (line.Contains("username: " + username_su.Text))
                        {
                            usernameExist = true;
                            break;
                        }
                    }
                }
            }

            if (profileExist)
            {
                error_su.Text = "Email already in use. Log in instead!";
                error_su.Visible = true;
                return;
            }

            if (usernameExist)
            {
                error_su.Text = "Username already in use. Choose another!";
                error_su.Visible = true;
                return;
            }

            if (!File.Exists(filePath))
            {
                using (File.Create(filePath)) { }
            }

            // TODO: implement some encrypting so user data cant be stolen
            using (StreamWriter writer = new StreamWriter(filePath, append: true))
            {
                writer.Write("name: "+name_su.Text+"\nemail: "+email_su.Text+"\nusername: "+username_su.Text +"\npassword: "+password_su.Text +"\n\n");
                writer.Flush();
                writer.Close();
            }

            string folderPath = Program.projectFolderPath + "\\user_data";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            folderPath = folderPath + "\\" + email_su.Text;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            error_su.Text = "User created!";
            error_su.ForeColor = Color.Green;
            error_su.Visible = true;

            email_su_Del_OnClick(sender, e);
            password_su_Del_OnClick(sender, e);
            name_su_Del_OnClick(sender, e);
            username_su_Del_OnClick(sender, e);
        }

        private void email_li_Del_OnClick(object sender, EventArgs e)
        {
            email_li.Text = string.Empty;
        }

        private void email_su_Del_OnClick(object sender, EventArgs e)
        {
            email_su.Text = string.Empty;
        }

        private void password_li_Del_OnClick(object sender, EventArgs e)
        {
            password_li.Text = string.Empty;
        }

        private void password_su_Del_OnClick(object sender, EventArgs e)
        {
            password_su.Text = string.Empty;
        }

        private void name_su_Del_OnClick(object sender, EventArgs e)
        {
            name_su.Text = string.Empty;
        }

        private void username_su_Del_OnClick(object sender, EventArgs e)
        {
            username_su.Text = string.Empty;
        }

        internal void hideErrors()
        {
            error_li.Visible = false;
            error_su.Visible = false;
        }
    }
}
