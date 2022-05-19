using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Domain.Models;
using Services.Interfaces;

namespace View;

public class CardGameViewModel : INotifyPropertyChanged
{
    private readonly IGameProvider _gameProvider;
    private readonly IDeckCreator _deckCreator;

    public Card? DeckCard;
    public bool IsWin => _gameProvider.IsWin;
    public bool IsTurnAvailable => _gameProvider.IsTurnAvailable;
    public ObservableCollection<CardViewItem> Layout { get; set; }
    public DeckViewItem? Deck { get; set; }

    public CardGameViewModel(IGameProvider gameProvider, IDeckCreator deckCreator)
    {
        Layout = new ObservableCollection<CardViewItem>();
        _gameProvider = gameProvider;
        _deckCreator = deckCreator;
    }

    private void Layout_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Replace)
        {
            var item = e.NewItems?[0] as CardViewItem;
            if (item is null)
            {
                throw new ArgumentException("Nothing are replaced");
            }
            var index = e.NewStartingIndex;
            var card = GetCardFromSource(item.CardSource);
            if (!_gameProvider.TryTurn(index, card))
            {
                throw new ArgumentException("Card cannot be placed there");
            }
            
            DeckCard = _gameProvider.GetCardFromDeck();
            if (Deck is null)
            {
                throw new ArgumentNullException(nameof(Deck), "Deck isn't initialized");
            }
            Deck.TopCardVisibility = (DeckCard is not null) ? "Visible" : "Hidden";
            if (DeckCard is not null)
            {
                Deck.CardSource = GetImageSource(DeckCard);
            }

            var turns = _gameProvider.AvailableTurns;
            for (var i = 0; i < turns.Count; i++)
            {
                Layout[i].AllowDrop = turns[i];
            }
        }
    }
    
    public void StartGame()
    {
        do
        {
            _gameProvider.StartGame(_deckCreator);
        } while (!_gameProvider.IsTurnAvailable);
        
        var layout = _gameProvider.Layout;
        var availableTurns = _gameProvider.AvailableTurns;

        Layout = new ObservableCollection<CardViewItem>(layout
            .Select((c, index) => new CardViewItem()
            {
                CardSource = GetImageSource(c),
                AllowDrop = availableTurns[index],
            })
        );
        Layout.CollectionChanged += Layout_CollectionChanged;
        DeckCard = _gameProvider.GetCardFromDeck();
        Deck = CreateDeckView(DeckCard);
    }

    private static string GetImageSource(Card? card)
    {
        var imgName = "images/";

        if (card is null)
        {
            return "";
        }
        
        imgName += card.Rank switch
        {
            Card.CardRank.Ace => "ace",
            Card.CardRank.Eight => "eight",
            Card.CardRank.Five => "five",
            Card.CardRank.Four => "four",
            Card.CardRank.Jack => "jack",
            Card.CardRank.King => "king",
            Card.CardRank.Nine => "nine",
            Card.CardRank.Queen => "queen",
            Card.CardRank.Seven => "seven",
            Card.CardRank.Six => "six",
            Card.CardRank.Ten => "ten",
            Card.CardRank.Three => "three",
            _ => "two",
        };

        imgName += '_';
        
        imgName += card.Suit switch
        {
            Card.CardSuit.Spades => "spade",
            Card.CardSuit.Hearts => "heart",
            Card.CardSuit.Diamonds => "diamond",
            _ => "club",
        };

        imgName += ".png";
        return imgName;
    }

    private static Card GetCardFromSource(string source)
    {
        var cardInfo = source[7..^4].Split("_");
        var cardSuit = cardInfo[1] switch
        {
            "spade" => Card.CardSuit.Spades,
            "heart" => Card.CardSuit.Hearts,
            "diamond" => Card.CardSuit.Diamonds,
            _ => Card.CardSuit.Clubs,
        };

        var cardRank = cardInfo[0] switch
        {
            "ace" => Card.CardRank.Ace,
            "eight" => Card.CardRank.Eight,
            "five" => Card.CardRank.Five,
            "four" => Card.CardRank.Four,
            "jack" => Card.CardRank.Jack,
            "king" => Card.CardRank.King,
            "nine" => Card.CardRank.Nine,
            "queen" => Card.CardRank.Queen,
            "seven" => Card.CardRank.Seven,
            "six" => Card.CardRank.Six,
            "ten" => Card.CardRank.Ten,
            "three" => Card.CardRank.Three,
            _ => Card.CardRank.Two,
        };

        return new Card(cardRank, cardSuit);
    }

    private DeckViewItem CreateDeckView(Card? card)
    {
        var result = new DeckViewItem
        {
            TopCardVisibility = (card is not null) ? "Visible" : "Hidden"
        };

        if (card is not null)
        {
            result.CardSource = GetImageSource(card);
        }

        return result;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

public class CardViewItem: INotifyPropertyChanged
{
    private string _cardSource = null!;

    public string CardSource
    {
        get => _cardSource;
        set
        {
            _cardSource = value;
            OnPropertyChanged();
        }
    }

    private bool _allowDrop;

    public bool AllowDrop
    {
        get => _allowDrop;
        set
        {
            _allowDrop = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName]string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

public class DeckViewItem: INotifyPropertyChanged
{
    private string _cardSource = null!;

    public string CardSource
    {
        get => _cardSource;
        set
        {
            _cardSource = value;
            OnPropertyChanged();
        }
    }

    private string _topCardVisibility = "Hidden";

    public string TopCardVisibility
    {
        get => _topCardVisibility;
        set
        {
            _topCardVisibility = value;
            OnPropertyChanged();
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName]string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}