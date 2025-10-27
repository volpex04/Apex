using System;
using System.Threading;
using System.Windows.Forms;

namespace ColorBotWinForms
{
    public class Logic
    {
        public int Smoothness { get; set; } = 5;     // 1–10 speed slider
        public int Tolerance { get; set; } = 20;     // Color tolerance
        public int FOVRadius { get; set; } = 200;    // Size of FOV
        public bool ShowFOV { get; set; } = true;    // Toggle FOV circle
        public Keys Hotkey { get; set; } = Keys.XButton2;

        private Thread scanThread;
        private bool scanning = false;

        public event Action<string> OnStatusUpdate; // For Form1 to display status

        public void StartScanning()
        {
            if (scanning) return;
            scanning = true;

            scanThread = new Thread(ScanLoop);
            scanThread.IsBackground = true;
            scanThread.Start();

            OnStatusUpdate?.Invoke("Scanning...");
        }

        public void StopScanning()
        {
            scanning = false;
            OnStatusUpdate?.Invoke("Stopped");
        }

        private void ScanLoop()
        {
            // Example scanning simulation
            while (scanning)
            {
                // Instead of controlling anything, we just simulate work.
                Thread.Sleep(1000 / Smoothness);

                // Example debug output (you can replace this later)
                Console.WriteLine($"Scanning with tolerance {Tolerance}, FOV {FOVRadius}, smoothness {Smoothness}");
            }
        }
    }
}
