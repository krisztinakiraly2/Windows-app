namespace WinFormsApp
{
    public partial class Game : Form
    {
        private Rectangle rect;
        private int score;
        private Color currentColor;
        private int defaultTime;
        private int defaultSize;
        StringFormat format;

        public Game()
        {
            InitializeComponent();

            rect = new Rectangle();
            defaultSize = 100;
            defaultTime = 1000;
            score = 0;

            rect.Width = rect.Height = defaultSize;
            timer1.Interval = defaultTime;
            chooseRandomPlace();

            currentColor = Color.DarkBlue;

            format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Near;
            format.FormatFlags = StringFormatFlags.NoWrap;

            timer1.Start();
        }

        private void kilépToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void újJátékToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chooseRandomPlace();
            currentColor = Color.DarkBlue;
            MouseDown += MainForm_MouseDown;
            Invalidate();
            timer1.Interval = defaultTime;
            rect.Width = rect.Height = defaultSize;
            score = 0;
            timer1.Start();
        }

        private void toolStripContainer_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private void chooseRandomPlace()
        {
            Size window = ClientSize;
            Random random = new Random();
            int random_x = random.Next(window.Width - rect.Width);
            int random_y = random.Next(25,window.Height - rect.Height);
            rect.X = random_x;
            rect.Y = random_y;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle window = new Rectangle(10, 30, ClientSize.Width, ClientSize.Height);
            e.Graphics.DrawString("Pontszám: "+score, this.Font, new SolidBrush(Color.Black), window, format);
            e.Graphics.FillRectangle(new SolidBrush(currentColor), rect.X, rect.Y, rect.Width, rect.Height);

        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.X>=rect.X && e.X<rect.X+rect.Width && e.Y>=rect.Y && e.Y<rect.Y+rect.Height) 
            {
                timer1.Stop();
                currentColor = Color.Green;
                ++score;
                Invalidate();
                timer2.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            currentColor = Color.Red;
            MouseDown -= MainForm_MouseDown;
            Invalidate();
            MessageBox.Show("Vége a játéknak!");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            currentColor = Color.DarkBlue;
            chooseRandomPlace();
            timer1.Interval -= 10;
            rect.Width -= 2;
            rect.Height -= 2;
            Invalidate();
            timer1.Start();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(e.KeyCode == Keys.S)
            {
                if (timer1.Enabled)
                {
                    timer1.Stop();
                    MouseDown -= MainForm_MouseDown;
                }
                else
                {
                    MouseDown += MainForm_MouseDown;
                    timer1.Start();
                }
            }
        }
    }
}