using System;
using System.Drawing;
using System.Windows.Forms;

namespace ColorBotWinForms
{
    public partial class Form1 : Form
    {
        ColorBot bot = new ColorBot();

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            // Initialize TrackBars
            trackSmoothness.Minimum = 1;
            trackSmoothness.Maximum = 10;
            trackSmoothness.Value = bot.Smoothness;
            lblSmoothness.Text = $"Smoothness: {trackSmoothness.Value}";
            trackSmoothness.Scroll += (s, e) =>
            {
                bot.Smoothness = trackSmoothness.Value;
                lblSmoothness.Text = $"Smoothness: {trackSmoothness.Value}";
            };

            trackTolerance.Minimum = 0;
            trackTolerance.Maximum = 50;
            trackTolerance.Value = bot.Tolerance;
            lblTolerance.Text = $"Tolerance: {trackTolerance.Value}";
            trackTolerance.Scroll += (s, e) =>
            {
                bot.Tolerance = trackTolerance.Value;
                lblTolerance.Text = $"Tolerance: {trackTolerance.Value}";
            };

            trackFOV.Minimum = 50;
            trackFOV.Maximum = 500;
            trackFOV.Value = bot.FOVRadius;
            lblFOV.Text = $"FOV Radius: {trackFOV.Value}";
            trackFOV.Scroll += (s, e) =>
            {
                bot.FOVRadius = trackFOV.Value;
                lblFOV.Text = $"FOV Radius: {trackFOV.Value}";
            };

            // Checkbox for showing FOV
            chkShowFOV.Checked = bot.ShowFOV;
            chkShowFOV.CheckedChanged += (s, e) => bot.ShowFOV = chkShowFOV.Checked;

            // Hotkey ComboBox
            cmbHotkey.Items.AddRange(new string[]
            {
                "XButton1 (M4)", "XButton2 (M5)", "Left Mouse", "Right Mouse",
                "A","B","C","D","E","F","G","H","I","J","K","L","M",
                "N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
            });
            cmbHotkey.SelectedIndex = 1; // default M5
            cmbHotkey.SelectedIndexChanged += (s, e) =>
            {
                switch (cmbHotkey.SelectedItem.ToString())
                {
                    case "XButton1 (M4)": bot.Hotkey = Keys.XButton1; break;
                    case "XButton2 (M5)": bot.Hotkey = Keys.XButton2; break;
                    case "Left Mouse": bot.Hotkey = Keys.LButton; break;
                    case "Right Mouse": bot.Hotkey = Keys.RButton; break;
                    default:
                        bot.Hotkey = (Keys)Enum.Parse(typeof(Keys), cmbHotkey.SelectedItem.ToString());
                        break;
                }
            };

            // Mouse events for scanning
            this.MouseDown += Form1_MouseDown;
            this.MouseUp += Form1_MouseUp;

            // Paint FOV circle
            this.Paint += Form1_Paint;

            // Timer to refresh FOV circle
            Timer refreshTimer = new Timer();
            refreshTimer.Interval = 30; // ~33 FPS
            refreshTimer.Tick += (s, e) => this.Invalidate();
            refreshTimer.Start();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.XButton1 && bot.Hotkey == Keys.XButton1) ||
                (e.Button == MouseButtons.XButton2 && bot.Hotkey == Keys.XButton2))
            {
                bot.StartScanning();
                lblStatus.Text = "Scanning...";
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.XButton1 && bot.Hotkey == Keys.XButton1) ||
                (e.Button == MouseButtons.XButton2 && bot.Hotkey == Keys.XButton2))
            {
                bot.StopScanning();
                lblStatus.Text = "Stopped";
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!bot.ShowFOV) return;

            using (Pen pen = new Pen(Color.White, 2))
            {
                // Center of the screen instead of cursor
                int screenCenterX = Screen.PrimaryScreen.Bounds.Width / 2;
                int screenCenterY = Screen.PrimaryScreen.Bounds.Height / 2;

                e.Graphics.DrawEllipse(pen,
                    screenCenterX - bot.FOVRadius,
                    screenCenterY - bot.FOVRadius,
                    bot.FOVRadius * 2,
                    bot.FOVRadius * 2);
            }
        }
    }
}
