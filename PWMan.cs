namespace WindowsApp
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Security;
    using System.Windows.Forms;
    using static System.Windows.Forms.LinkLabel;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

    public partial class PWMan : Form
    {
        string user_email = "";
        public event Action<string> logOut;
        public DataTable passwordTable;
        bool isShown = false;
        private List<string> passwordList = [];

        private void InitializePasswordTable()
        {
            passwordTable = new DataTable();
            passwordTable.Columns.Add("Website", typeof(string));
            passwordTable.Columns.Add("Email/Username", typeof(string));
            passwordTable.Columns.Add("Password", typeof(string));
        }

        public bool loadPassword() 
        { 
            var folderPath = Program.projectFolderPath + "\\user_data\\"+user_email;
            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath);
                if (files.Length > 0)
                {
                    InitializePasswordTable();

                    foreach (string file in files)
                    {
                        String website;
                        String emailOrUsername;

                        string[] lines = File.ReadAllLines(file);

                        lines[0] = lines[0].Substring(lines[0].IndexOf("email or username:") + "email or username: ".Length).Trim();
                        lines[1] = lines[1].Substring(lines[1].IndexOf("password:") + "password: ".Length).Trim();

                        website = Path.GetFileName(file);
                        emailOrUsername = lines[0];
                        passwordList.Add(lines[1]);

                        passwordTable.Rows.Add(website, emailOrUsername, "******");
                    }

                    return true;
                }
                return false;
            }
            else
                throw new Exception("User does not exist");

            // email or username: krisztina
            // password: password123
        }

        public static void savePassword() { }

        public PWMan()
        {
            InitializeComponent();

            //BackColor = Color.FromArgb(30, 30, 30);
            ContentPanel.RenderMode = ToolStripRenderMode.Professional;
            menuStrip.Renderer = new DarkModeRenderer();

            foreach (ToolStripMenuItem item in menuStrip.Items)
            {
                item.ForeColor = Color.FromArgb(241, 241, 241);
                DarkModeRenderer.SetSubItemColor(item);
            }

            // search for User_data/$user_email folder
            // load all files into a table
            // The app object will be created before knowing the email. So this should go somewhere else
        }

        public void setEmail(string email)
        { 
            user_email = email; 
        }

        private void buildTable()
        {
            if (passwordTable != null)
            {
                dataGridViewPasswords.DataSource = passwordTable; // Bind the DataTable to the DataGridView
                dataGridViewPasswords.AutoResizeColumns(); // Optional: resize columns to fit content
            }

            dataGridViewPasswords.Columns["Website"].Width = 200;
            dataGridViewPasswords.Columns["Email/Username"].Width = 200;
            dataGridViewPasswords.Columns["Password"].Width = 200;

            dataGridViewPasswords.Columns["Website"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewPasswords.Columns["Email/Username"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewPasswords.Columns["Password"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            AddButtonColumn();
        }

        private void AddButtonColumn()
        {
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "ShowPassword";
            buttonColumn.Text = "👁️";
            buttonColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            buttonColumn.UseColumnTextForButtonValue = true;
            buttonColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            buttonColumn.Width = 150;
            dataGridViewPasswords.CellContentClick += dataGridViewPasswords_CellClick;
            dataGridViewPasswords.Columns.Add(buttonColumn);
        }

        private void dataGridViewPasswords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(!isShown)
            {
                string newValue = showPw(e.RowIndex);
                dataGridViewPasswords.Rows[e.RowIndex].Cells["Password"].Value = newValue;
            }
            else
            {
                dataGridViewPasswords.Rows[e.RowIndex].Cells["Password"].Value = "******";
            }
            isShown = !isShown;
        }

        public string showPw(int row)
        {
            return passwordList.ElementAt(row);
        }

        public void reload()
        {
            // Reload the data
            // there will be probably a new user
            try
            {
                loadPassword();
                buildTable();
            }
            catch
            {
                if (Directory.Exists(Program.projectFolderPath + "\\user_data") && File.Exists(Program.projectFolderPath + "\\users.txt"))
                {
                    string usersFilePath = Program.projectFolderPath + "\\users.txt";
                    string[] lines = File.ReadAllLines(usersFilePath);
                    int lineIndex = -1;

                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i];
                        bool contains = line.Contains("email:");
                        if (contains)
                        {
                            string currEmail = line.Substring(line.IndexOf("email:") + "email: ".Length).Trim();
                            if (currEmail == user_email)
                            {
                                lineIndex = i;
                                break;
                            }
                        }
                    }

                    if (lineIndex > -1)
                    {
                        var folderPath = Program.projectFolderPath + "\\user_data\\" + user_email;
                        Directory.CreateDirectory(folderPath);
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO
            // Can edit everything but wont be saved without explicit saving
            // Writing on disk maybe hard so this is not that useless
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // TODO
            // redraw the table
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logOut?.Invoke("");
        }
    }
}