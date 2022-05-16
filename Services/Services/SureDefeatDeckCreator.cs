using Domain.Models;
using Services.Interfaces;

namespace Services.Services;

public class SureDefeatDeckCreator : IDeckCreator
{
    public IEnumerable<Card> CreateDeck()
    {
        var ranks = Enum.GetValues(typeof(Card.CardRank)).Cast<Card.CardRank>();
        var suits = new List<Card.CardSuit>(new[] {Card.CardSuit.Hearts});

        var otherRanks = new List<Card.CardRank>(new[] {Card.CardRank.King, Card.CardRank.Ace});
        var otherSuits = new List<Card.CardSuit>(new[] {Card.CardSuit.Clubs});
        
        var deck = ranks
            .SelectMany(_ => suits, (rank, suit) => new Card(rank,suit))
            .Concat(otherRanks.SelectMany(_ => otherSuits, (rank, suit) => new Card(rank,suit)))
            .ToList();
        
        deck.Shuffle();

        return deck;
    }
}