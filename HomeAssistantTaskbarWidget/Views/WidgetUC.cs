using HomeAssistantTaskbarWidget.Utils;
using System.Drawing;
using System.Windows.Forms;
using Font = HomeAssistantTaskbarWidget.Model.Settings.Font;

namespace HomeAssistantTaskbarWidget.Views
{
    public partial class WidgetUC : UserControl
    {
        private ILogger _logger;
        public WidgetUC(CSDeskBand.CSDeskBandWin deskBand, ILogger logger)
        {
            _logger = logger;

            InitializeComponent();
        }

        public void SetTooltip(string text)
        {
            labelToolTip.SetToolTip(label, text);
            labelToolTip.ShowAlways = true;
        }

        public void UpdateText(string text)
        {
            label.Text = text;
        }

        public void UpdateFont(Font font)
        {
            label.Font = new System.Drawing.Font(font.Family, font.Size.Value, FontStyle.Regular);
            label.ForeColor = Helper.HexToColor(font.Color);
        }

        public void UpdateSize(Model.Settings.Size size)
        {
            Size = new Size(size.Width.Value, size.Height.Value);
            label.Size = new Size(size.Width.Value, size.Height.Value);
        }

        private void WidgetUC_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    contextMenuStrip.Show(this, new Point(e.X, e.Y));
            //}
        }
    }
}
