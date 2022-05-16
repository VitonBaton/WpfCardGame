using Domain.Models;
using Services.Interfaces;

namespace Services.Services;

public class SureWinDeckCreator : IDeckCreator
{
    public IEnumerable<Card> CreateDeck()
    {
        var ranks = Enum.GetValues(typeof(Card.CardRank)).Cast<Card.CardRank>();
        var suits = new List<Card.CardSuit>(new[] {Card.CardSuit.Hearts});
        
        var deck = ranks.SelectMany(_ => suits, (rank, suit) => new Card(rank,suit))
            .ToList();
        
        deck.Shuffle();

        return deck;
    }
}