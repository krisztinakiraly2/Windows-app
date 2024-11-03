namespace WindowsApp
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DarkModeRenderer : ToolStripProfessionalRenderer
    {
        public DarkModeRenderer() : base(new DarkColorTable()) { }

        // Method to set the text color for sub-menu items
        public static void SetSubItemColor(ToolStripMenuItem menuItem)
        {
            foreach (ToolStripItem subItem in menuItem.DropDownItems)
            {
                subItem.ForeColor = Color.FromArgb(241, 241, 241);

                if (subItem is ToolStripMenuItem subMenuItem)
                {
                    DarkModeRenderer.SetSubItemColor(subMenuItem);
                }
            }
        }
    }

    public class DarkColorTable : ProfessionalColorTable
    {
        public override Color MenuStripGradientBegin => Color.FromArgb(30, 30, 30);
        public override Color MenuStripGradientEnd => Color.FromArgb(30, 30, 30);

        public override Color ToolStripDropDownBackground => Color.FromArgb(45, 45, 45);

        public override Color ImageMarginGradientBegin => Color.FromArgb(45, 45, 45);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(45, 45, 45);
        public override Color ImageMarginGradientEnd => Color.FromArgb(45, 45, 45);

        public override Color MenuItemSelected => Color.FromArgb(50, 50, 50);

        public override Color MenuItemPressedGradientBegin => Color.FromArgb(50, 50, 50);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(50, 50, 50);

        public override Color ToolStripBorder => Color.FromArgb(30, 30, 30);

        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(60, 60, 60);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(60, 60, 60);
        public override Color MenuItemBorder => Color.FromArgb(60, 60, 60);
    }
}