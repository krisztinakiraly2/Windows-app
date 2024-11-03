using System;
using System.IO;
using System.Windows.Forms;

namespace WindowsApp
{
    internal class App
    {
        private PWMan PWman;
        private Login loginForm;
        private Form hiddenForm;

        public App()
        {
            InitializeComponents();
            RunApplication();
        }

        private void InitializeComponents()
        {
            PWman = new PWMan();
            loginForm = new Login();

            PWman.logOut += LogOut;
            loginForm.LoginSuccessful += OnLoginSuccessful;

            hiddenForm = new Form
            {
                ShowInTaskbar = false,
                Opacity = 0,
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                Size = new System.Drawing.Size(1, 1),
                Location = new System.Drawing.Point(-10000, -10000)
            };

            PWman.FormClosed += (sender, e) => Application.Exit();
            loginForm.FormClosed += (sender, e) => Application.Exit();
            hiddenForm.FormClosed += (sender, e) => Application.Exit();
        }

        private void RunApplication()
        {
            string filePath = Program.projectFolderPath + "\\defaultuser.txt";

            if (File.Exists(filePath))
            {
                string user_email = App.LoadUserEmail(filePath);
                OnLoginSuccessful(user_email);
            }
            else
            {
                loginForm.hideErrors();
                loginForm.Show();
            }

            Application.Run(hiddenForm);
        }

        private static string LoadUserEmail(string filePath)
        {
            string user_email = "";
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    user_email = line.Substring(line.IndexOf("email:") + "email: ".Length).Trim();
                    break;
                }
            }
            return user_email;
        }

        private void OnLoginSuccessful(string userEmail)
        {
            PWman.setEmail(userEmail);
            PWman.reload();
            PWman.Show();
            loginForm.Hide();
        }

        private void LogOut(string s)
        {
            PWman.Hide();
            loginForm.hideErrors();
            loginForm.Show();

            string filePath = Program.projectFolderPath + "\\defaultuser.txt";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
