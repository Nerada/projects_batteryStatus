using System.Windows;

namespace BatteryStatus
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void InitApplication(object sender, StartupEventArgs e)
        {
            var trayApp = new MainTray();
        }
    }
}