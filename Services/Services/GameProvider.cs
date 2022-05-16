using Domain.Models;
using Services.Interfaces;

namespace Services.Services;

public class GameProvider : IGameProvider
{
    private Stack<Card> Deck { get; set; } = new ();
    public List<Card> Layout { get; private set; } = new();
    public List<bool> AvailableTurns { get; private set; } = new();
    public bool IsTurnAvailable => AvailableTurns.Any(t => t);
    
    public Card? GetCardFromDeck()
    {
        return Deck.TryPop(out var card) ? card : null;
    }

    public Card? CheckCardFromDeck()
    {
        return Deck.TryPeek(out var card) ? card : null;
    }

    public void PlaceCardToDeck(Card card)
    {
        Deck.Push(card);
    }
    
    public void StartGame(IDeckCreator deckCreator)
    {
        Deck = new Stack<Card>(deckCreator.CreateDeck());
        Layout = new List<Card>();
        AvailableTurns = new List<bool>();
        for (var i = 0; i < 4; i++)
        {
            Layout.Add(Deck.Pop() ?? throw new InvalidOperationException());
            AvailableTurns.Add(false);
        }
        EvaluateAvailableTurns();
    }
    
    public List<int> GetAvailableTurns()
    {
        var result = new List<int>();
        for (var i = 0; i < AvailableTurns.Count; i++)
        {
            if (AvailableTurns[i])
            {
                result.Add(i);
            }
        }

        return result;
    }

    public bool TryTurn(int index, Card newCard)
    {
        if (!AvailableTurns[index])
        {
            return false;
        }

        Layout[index] = newCard;
        AvailableTurns[index] = false;
        EvaluateAvailableTurns();
        return true;
    }

    private void EvaluateAvailableTurns()
    {
        for (var i = 0; i < Layout.Count; i++)
        {
            var card = Layout[i];
            if (Layout.Count(c => c.Suit.Equals(card.Suit)) > 1)
            {
                AvailableTurns[i] = true;
            }
        }
    }
}

