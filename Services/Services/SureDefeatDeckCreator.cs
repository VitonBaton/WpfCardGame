using Domain.Models;
using Services.Interfaces;

namespace Services.Services;

public class SureDefeatDeckCreator : IDeckCreator
{
    public IEnumerable<Card> CreateDeck()
    {
        var suits = Enum.GetValues(typeof(Card.CardSuit)).Cast<Card.CardSuit>();
        var ranks = new List<Card.CardRank>(new[] {Card.CardRank.Ace});
        
        var otherRanks = new List<Card.CardRank>(new[] {Card.CardRank.King, Card.CardRank.Queen});
        var otherSuits = new List<Card.CardSuit>(new[] {Card.CardSuit.Spades, Card.CardSuit.Diamonds});

        var deck = ranks
            .SelectMany(_ => suits, (rank, suit) => new Card(rank, suit))
            .ToList();
        deck.Reverse();
        deck.Add(new Card(Card.CardRank.Jack, Card.CardSuit.Diamonds));
        deck.AddRange(otherRanks.SelectMany(_ => otherSuits, (rank, suit) => new Card(rank, suit)));

        return deck;
    }
}