using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Services.Interfaces;

namespace View;

public partial class CardGamePage
{
    private readonly CardGameViewModel _cardGameViewModel;
    
    public CardGamePage(IGameProvider gameProvider, IDeckCreator deckCreator)
    {
        InitializeComponent();
        _cardGameViewModel = new CardGameViewModel(gameProvider, deckCreator);
        _cardGameViewModel.StartGame();
        DataContext = _cardGameViewModel;
    }

    private void Deck_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        var deck = _cardGameViewModel.Deck;
        var data = new DataObject();
        if (deck != null)
        {
            data.SetData("CardSource", deck.CardSource);
        }
        else
        {
            MessageBox.Show("Something went wrong");
        }

        DragDrop.DoDragDrop((Image) sender, data, DragDropEffects.All);
    }

    private void Card_OnDrop(object sender, DragEventArgs e)
    {
        if (e.Data.GetData("CardSource") is string data)
        {
            var item = (CardViewItem?) (sender as Image)?.Tag;
            if (item is not null)
            {
                var index = IcCardList.Items.IndexOf(item);
                var list = (ObservableCollection<CardViewItem>)IcCardList.ItemsSource;
                item.CardSource = data;
                list[index] = item;
            }
            
            if (_cardGameViewModel.IsWin)
            {
                EndGamePage("YOU ARE WIN!");
                return;
            }

            if (!_cardGameViewModel.IsTurnAvailable)
            {
                EndGamePage("YOU ARE LOSE!");
            }
        }
    }
    
    private void EndGamePage(string textMessage)
    {
        NavigationService?.Navigate(new EndGamePage(textMessage));
    }
}