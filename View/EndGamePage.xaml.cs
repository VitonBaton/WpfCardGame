using System.Windows;

namespace View;

public partial class EndGamePage
{
    public EndGamePage(string textMessage)
    {
        InitializeComponent();
        TextMessage.Text = textMessage;
    }

    private void NewGame_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new StartPage());
    }
}