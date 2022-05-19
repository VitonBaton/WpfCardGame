using Domain.Models;

namespace Services.Interfaces;

public interface IGameProvider
{
    public bool IsWin { get; }
    public bool IsTurnAvailable { get;}
    public List<Card> Layout { get;}
    public List<bool> AvailableTurns { get; }
    public List<int> GetAvailableTurns();
    public Card? GetCardFromDeck();
    public Card? CheckCardFromDeck();
    public void PlaceCardToDeck(Card card);
    public void StartGame(IDeckCreator deckCreator);
    public bool TryTurn(int index, Card newCard);
}