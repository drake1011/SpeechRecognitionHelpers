using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DictationBoxMSP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var processes = Process.GetProcessesByName("DictationBoxMSP");
            foreach (var process in processes)
            {
                var currentProcess = Process.GetCurrentProcess();
                if (process.Id != currentProcess.Id)
                {
                    currentProcess.Kill();
                }
            }
            Application.Run(new DictationBoxForm());
        }
    }
}
