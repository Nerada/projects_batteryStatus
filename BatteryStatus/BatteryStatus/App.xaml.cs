// -----------------------------------------------
//     Author: Ramon Bollen
//      File: BatteryStatus.App.xaml.cs
// Created on: 20201207
// -----------------------------------------------

using System.Diagnostics;
using System.Windows;

namespace BatteryStatus
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void InitApplication(object sender, StartupEventArgs e)
        {
            string procName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(procName);

            if (processes.Length > 1)
            {
                Current.Shutdown();
            }
            else
            {
                _ = new MainTray();
            }
        }
    }
}