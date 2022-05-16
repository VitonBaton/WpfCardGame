using System.Windows;

namespace View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            // var gameProvider = new GameProvider();
            // var deckCreator = new DeckCreator();
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}