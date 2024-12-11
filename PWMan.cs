namespace WindowsApp
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Net;
    using System.Security;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using static System.Windows.Forms.LinkLabel;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
    using System.Net.Http;
    using System.Threading.Tasks;

    public partial class PWMan : Form
    {
        string user_email = "";
        public event Action<string> logOut;
        public DataTable passwordTable;

        string Website = "Website";
        string EmailOrUsername = "Email/Username";
        string Password = "Password";
        string RealPassword = "Real Password";
        string IsShown = "Is Shown";
        string IsSelected = "Is Selected";
        bool button_already_added = false;

        static string selectedEmailOrUsername = "";
        static string selectedPassword = "";

        public static string SelectedEmailOrUsername
        { get { return selectedEmailOrUsername; } }

        public static string SelectedPassword
        { get { return selectedPassword; } }


        public PWMan()
        {
            InitializeComponent();

            ContentPanel.RenderMode = ToolStripRenderMode.Professional;
            menuStrip.Renderer = new DarkModeRenderer();

            foreach (ToolStripMenuItem item in menuStrip.Items)
            {
                item.ForeColor = Color.FromArgb(241, 241, 241);
                DarkModeRenderer.SetSubItemColor(item);
            }

            dataGridViewPasswords.RowValidating += DataGridView_RowValidating;
        }

        private void InitializePasswordTable()
        {
            passwordTable = new DataTable();
            passwordTable.Columns.Add(Website, typeof(string));
            passwordTable.Columns.Add(EmailOrUsername, typeof(string));
            passwordTable.Columns.Add(Password, typeof(string));
            passwordTable.Columns.Add(RealPassword, typeof(string));
            passwordTable.Columns.Add(IsShown, typeof(bool));
            passwordTable.Columns.Add(IsSelected, typeof(bool));
        }

        public void loadPassword()
        {
            var folderPath = Program.projectFolderPath + "\\user_data\\" + user_email;
            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath);
                InitializePasswordTable();

                if (files.Length > 0)
                {

                    foreach (string file in files)
                    {
                        String website;
                        String emailOrUsername;

                        string[] lines = File.ReadAllLines(file);

                        lines[0] = lines[0].Substring(lines[0].IndexOf("email or username:") + "email or username: ".Length).Trim();
                        lines[1] = lines[1].Substring(lines[1].IndexOf("password:") + "password: ".Length).Trim();

                        website = Path.GetFileNameWithoutExtension(file).Replace('+', '/');
                        emailOrUsername = lines[0];

                        passwordTable.Rows.Add(website, emailOrUsername, "******", lines[1], false, false);
                    }
                }
            }
            else
                throw new Exception("User does not exist");
        }

        public void savePassword()
        {
            var folderPath = Program.projectFolderPath + "\\user_data\\" + user_email;
            List<string> myfiles = [];

            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath);

                if (files.Length > 0)
                {

                    foreach (string file in files)
                    {
                        myfiles.Add(Path.GetFileNameWithoutExtension(file));
                    }
                }
            }

            foreach (DataGridViewRow row in dataGridViewPasswords.Rows)
            {
                if (row.IsNewRow) continue;

                string website = (row.Cells[Website]?.Value?.ToString() ?? string.Empty).Replace('/', '+');
                string emailOrUsername = row.Cells[EmailOrUsername]?.Value?.ToString() ?? string.Empty;
                string password = row.Cells[RealPassword]?.Value?.ToString() ?? string.Empty;

                string filePath = Program.projectFolderPath + "\\user_data\\" + user_email + "\\" + website +".txt";
                if (File.Exists(filePath))
                {
                    myfiles?.Remove(website);
                    using StreamWriter writer = new StreamWriter(filePath);
                    writer.Write("email or username: " + emailOrUsername + "\npassword: " + password);
                    writer.Flush();
                    writer.Close();
                }
                else
                {
                    File.Create(filePath).Close();
                    using StreamWriter writer = new StreamWriter(filePath);
                    writer.Write("email or username: " + emailOrUsername + "\npassword: " + password);
                    writer.Flush();
                    writer.Close();
                }
            }

            if (myfiles.Count != 0)
            {
                foreach (var item in myfiles)
                {
                    string filePath = Program.projectFolderPath + "\\user_data\\" + user_email + "\\" + item +".txt";
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            savePassword(); 
            e.Cancel = false;
        }

        public void setEmail(string email)
        {
            user_email = email;
        }

        private void buildTable()
        {
            if (passwordTable != null)
            {
                dataGridViewPasswords.DataSource = passwordTable;
                dataGridViewPasswords.AutoResizeColumns();
            }

            dataGridViewPasswords.Columns[Website].Width = 200;
            dataGridViewPasswords.Columns[EmailOrUsername].Width = 200;
            dataGridViewPasswords.Columns[Password].Width = 200;

            dataGridViewPasswords.Columns[Website].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewPasswords.Columns[EmailOrUsername].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewPasswords.Columns[Password].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewPasswords.Columns[IsSelected].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridViewPasswords.Columns[RealPassword].Visible = false;
            dataGridViewPasswords.Columns[IsShown].Visible = false;

            if(!button_already_added)
                AddButtonColumn();
        }

        private void AddButtonColumn()
        {
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "ShowPassword";
            buttonColumn.Name = "ShowPassword";
            buttonColumn.Text = "👁️";
            buttonColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            buttonColumn.UseColumnTextForButtonValue = true;
            buttonColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            buttonColumn.Width = 150;
            dataGridViewPasswords.CellContentClick += dataGridViewPasswords_CellClick;
            dataGridViewPasswords.Columns.Add(buttonColumn);
            button_already_added = true;
        }

        private void dataGridViewPasswords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if(dataGridViewPasswords.Columns[e.ColumnIndex].Name == "ShowPassword")
                {
                    if ((bool)dataGridViewPasswords.Rows[e.RowIndex].Cells[IsShown].Value == true)
                    {
                        dataGridViewPasswords.Rows[e.RowIndex].Cells[Password].Value = "******";
                        dataGridViewPasswords.Rows[e.RowIndex].Cells[IsShown].Value = false;
                    }
                    else
                    {
                        string newValue = dataGridViewPasswords.Rows[e.RowIndex].Cells[RealPassword].Value.ToString();
                        dataGridViewPasswords.Rows[e.RowIndex].Cells[Password].Value = newValue;
                        dataGridViewPasswords.Rows[e.RowIndex].Cells[IsShown].Value = true;
                    }
                }
            }

            if (dataGridViewPasswords != null && dataGridViewPasswords.Columns[e.ColumnIndex].Name == Password && e.RowIndex >= 0)
            {
                dataGridViewPasswords.Rows[e.RowIndex].Cells[Password].Value = "";
            }

            if (dataGridViewPasswords.Columns[e.ColumnIndex].Name == IsSelected && e.RowIndex >= 0)
            {
                // Commit the edit (to reflect the change in the cell's value)
                dataGridViewPasswords.CommitEdit(DataGridViewDataErrorContexts.Commit);

                // Get the current state of the clicked cell
                bool isSelected = (bool)dataGridViewPasswords.Rows[e.RowIndex].Cells[IsSelected].Value;

                if (isSelected)
                {
                    // Unselect all other rows
                    foreach (DataGridViewRow row in dataGridViewPasswords.Rows)
                    {
                        if (row.Index != e.RowIndex)
                        {
                            row.Cells[IsSelected].Value = false;
                        }
                    }

                    selectedEmailOrUsername = dataGridViewPasswords.Rows[e.RowIndex].Cells[EmailOrUsername].Value.ToString();
                    selectedPassword = dataGridViewPasswords.Rows[e.RowIndex].Cells[RealPassword].Value.ToString();

                }
            }
        }

        public string showPw(int row)
        {
            if (row > 0)
                return passwordTable.Rows[row][RealPassword].ToString();
            else
                return "";
        }

        public void reload()
        {
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savePassword();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logOut?.Invoke("");
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridViewPasswords.AllowUserToAddRows = true;
            dataGridViewPasswords.UserAddedRow += DataGridView_UserAddedRow;
        }

        private void DataGridView_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            dataGridViewPasswords.AllowUserToAddRows = false;
            dataGridViewPasswords.UserAddedRow -= DataGridView_UserAddedRow;
        }

        private void DataGridView_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewRow row = dataGridViewPasswords.Rows[e.RowIndex];

            if (!IsRowFullyFilled(row))
            {
                MessageBox.Show("Please fill in all the fields before leaving the row.", "Incomplete Row", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                e.Cancel = true;
                return;
            }

            string website = row.Cells[Website].Value.ToString();
            string password = row.Cells[Password].Value.ToString();

            if (!PWMan.IsValidWebsite(website))
            {
                MessageBox.Show("Please enter a valid website URL (e.g. example.com).", "Invalid Website", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }

            if (row.Cells[Password].Value.ToString().CompareTo("******") != 0)
            {
                row.Cells[Password].Value = "******";
                row.Cells[RealPassword].Value = password;
                row.Cells[IsShown].Value = false;
            }
        }

        private static bool IsValidWebsite(string website)
        {
            return website.Contains(".com") || website.Contains(".hu");
        }

        private bool IsRowFullyFilled(DataGridViewRow row)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                string columnName = row.DataGridView.Columns[cell.ColumnIndex].Name;

                if (columnName == RealPassword || columnName == IsShown || columnName == IsSelected)
                {
                    continue;
                }

                if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()))
                {
                    return false;
                }
            }
            return true;
        }
    }
}