using System;

namespace NovaScreenSaver
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            base.OnStartup(e);
            try { ScreensaverRunner.Run(e.Args); }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Nova Screensaver",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                this.Shutdown();
            }
        }
    }
}
