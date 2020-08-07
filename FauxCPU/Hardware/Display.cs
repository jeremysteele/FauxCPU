using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace FauxCPU.Hardware
{
    class Display
    {
        int width = 256;
        int height = 256;
        int scaling = 4;
        Form1 output;
        CPU cpu;
        int currentFPS = 0;

        string[] hexColorPalette =
        {
            "#000000","#0000AA","#00AA00","#00AAAA","#AA0000","#AA00AA","#AA5500","#AAAAAA",
            "#555555","#5555FF","#55FF55","#55FFFF","#FF5555","#FF55FF","#FFFF55","#FFFFFF"
        };

        Color[] colorPalette = new Color[16];

        public void Initialize(CPU cpu, Form1 output)
        {
            for (int i = 0;i < hexColorPalette.Length; i++)
            {
                colorPalette[i] = System.Drawing.ColorTranslator.FromHtml(hexColorPalette[i]);
            }

            this.output = output;
            this.cpu = cpu;

            Thread t = new Thread(new ThreadStart(RefreshCycle));
            t.IsBackground = true;

            t.Start();

            System.Timers.Timer aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += FPSTimer;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        public void FPSTimer(Object source, ElapsedEventArgs e)
        {
            System.Console.WriteLine("FPS: " + currentFPS);
            currentFPS = 0;
        }

        public void RefreshCycle()
        {
            while(true)
            {
                Refresh();

                Thread.Sleep(1);
            }
        }

        public void Refresh()
        {
            Graphics g = output.CreateGraphics();

            Bitmap bmap = new Bitmap(width, height);
            Graphics fb_g = Graphics.FromImage(bmap);

            SolidBrush brush = new SolidBrush(Color.Black);
            Rectangle rect = new Rectangle(0, 0, 64, 64);

            fb_g.FillRectangle(brush, rect);

            for (int i = 0x0200; i < 0x1200;i++)
            {
                byte pixel = (byte)(cpu.Peek(i) & 0xF);
                int x = (i - 0x200) % width;
                int y = (i - 0x200) / width;

                bmap.SetPixel(x, y, colorPalette[pixel]);
            }

            Rectangle src = new Rectangle(0, 0, 64, 64);
            Rectangle desc = new Rectangle(0, 0, 256, 256);

            //Point zeroPoint = new Point(0, 0);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(bmap, desc, src, GraphicsUnit.Pixel);

            currentFPS++;
        }
    }
}
