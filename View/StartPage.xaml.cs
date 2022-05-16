using System.Windows;
using Services.Interfaces;
using Services.Services;

namespace View;

public partial class StartPage
{
    public StartPage()
    {
        InitializeComponent();
    }

    private void StartGamePage(IDeckCreator deckCreator, IGameProvider gameProvider)
    {
        NavigationService?.Navigate(new CardGamePage(gameProvider, deckCreator));
    }
    
    private void Default_OnClick(object sender, RoutedEventArgs e)
    {
        StartGamePage(new DeckCreator(), new GameProvider());
    }
    
    private void SureWin_OnClick(object sender, RoutedEventArgs e)
    {
        StartGamePage(new SureWinDeckCreator(), new GameProvider());
    }
    
    private void SureDefeat_OnClick(object sender, RoutedEventArgs e)
    {
        StartGamePage(new SureDefeatDeckCreator(), new GameProvider());
    }
}