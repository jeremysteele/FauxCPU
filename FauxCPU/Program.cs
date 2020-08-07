using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FauxCPU
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Hardware.CPU cpu = new Hardware.CPU();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            cpu.Initialize();

            cpu.Poke(0, 0x1F);
            cpu.Poke(0x0507, 0x17);

            cpu.Poke(0x1200, 0x10);
            cpu.Poke(0x1201, 0x23);
            cpu.Poke(0x1202, 0xC1);

            cpu.Poke(0x1210, 0x17);

            cpu.Poke(0xC123, 0xFA);

            cpu.Bootup();

            Form1 mainForm = new Form1();

            Hardware.Display d = new Hardware.Display();
            d.Initialize(cpu, mainForm);
            
            Application.Run(mainForm);
        }
    }
}
