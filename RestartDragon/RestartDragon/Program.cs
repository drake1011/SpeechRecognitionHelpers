﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestartDragon
{
    class Program
    {
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();


        static void Main(string[] args)
        {
            var name = "natspeak";
            KillAllProcesses(name);
            name = "dragonbar";
            KillAllProcesses(name);
            name = "nsbrowse";
            KillAllProcesses(name);
            name = "KBPro";
            KillAllProcesses(name);
            try
            {
                Process process = new Process();
                var filename = "C:\\Program Files(x86)\\KnowBrainer\\KnowBrainer Professional 2017\\KBPro.exe";
                if (File.Exists(filename))
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.WorkingDirectory = "C:\\Program Files (x86)\\KnowBrainer\\KnowBrainer Professional 2017\\";
                    process.StartInfo.FileName = filename;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    process.Start();

                }

                else
                {
                    IntPtr hwnd = GetForegroundWindow();
                    uint pid;
                    GetWindowThreadProcessId(hwnd, out pid);
                    Process currentProcess = Process.GetProcessById((int)pid);

                    List<string> keysKB = new List<string>(new string[] { "^+k" });
                    SendKeysCustom(null, null, keysKB, currentProcess.ProcessName);
                }
            }
            catch (Exception exception)
            {
                //Dispatcher.Invoke(() =>
                //{
                //    _logText.Text += $"An error has occurred: {exception.Message}";
                //});
            }

             void SendKeysCustom(string applicationClass, string applicationCaption, List<string> keys, string processName, string applicationToLaunch = "", int delay = 0)
            {
                // Get a handle to the application. The window class
                // and window name can be obtained using the Spy++ tool.
                IntPtr applicationHandle = IntPtr.Zero;
                while (true)
                {
                    if (applicationClass != null || applicationCaption != null)
                    {
                        applicationHandle = FindWindow(applicationClass, applicationCaption);
                    }

                    // Verify that Application is a running process.
                    if (applicationHandle == IntPtr.Zero)
                    {
                        if (applicationToLaunch != null && applicationToLaunch.Length > 0)
                        {
                            Process.Start(applicationToLaunch);
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            //       System.Windows.MessageBox.Show($"{applicationCaption} is not running.");
                            //ActivateApp(processName);
                            Process process = Process.GetProcessesByName(processName)[0];
                            applicationHandle = process.Handle;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                // Make Application the foreground application and send it 
                // a set of Keys.
                SetForegroundWindow(applicationHandle);
                foreach (var item in keys)
                {
                    Thread.Sleep(delay);
                    try
                    {
                        var temporary = item.Replace("(", "{(}");
                        temporary = temporary.Replace(")", "{)}");

                        SendKeys.SendWait(temporary);
                    }
                    catch (Exception exception)
                    {
                        //           this.WriteLine($"An error has occurred: {exception.Message}");
                    }
                }
            }

        }




        static void ActivateApp(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            //Activate the first application refined with this name
            if (processes.Count() > 0)
            {
                SetForegroundWindow(processes[0].MainWindowHandle);
            }
        }

        public static void KillAllProcesses(string name)
        {
            var processName = (name);
            if (processName.Length > 0)
            {
                foreach (var process in Process.GetProcessesByName(processName))
                {
                    try
                    {
                        process.Kill();
                    }
                    catch (Exception exception)
                    {
                        //System.Windows.MessageBox.Show(exception.Message);
                    }
                }
            }
        }
    }
}
