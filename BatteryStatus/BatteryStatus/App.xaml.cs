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
            _ = new MainTray();
        }
    }
}