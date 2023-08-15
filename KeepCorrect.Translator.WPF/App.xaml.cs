using System.Windows;

namespace KeepCorrect.Translator.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            // Process command line args
            /*var isAutoStart = false;
            for (int i = 0; i != e.Args.Length; ++i)
            {
                if (e.Args[i] == "/AutoStart")
                {
                    isAutoStart = true;
                }
            }*/

            // Create main application window, starting minimized if specified
            var mainWindow = new MainWindow();
            //mainWindow.WindowState = WindowState.Minimized;
            mainWindow.OnAutoStart();
        }
    }
}