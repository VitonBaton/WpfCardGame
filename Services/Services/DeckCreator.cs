using Domain.Models;
using Services.Interfaces;

namespace Services.Services;

public class DeckCreator : IDeckCreator
{
    public IEnumerable<Card> CreateDeck()
    {
        var ranks = Enum.GetValues(typeof(Card.CardRank)).Cast<Card.CardRank>();
        var suits = Enum.GetValues(typeof(Card.CardSuit)).Cast<Card.CardSuit>();
        
        var deck = ranks.SelectMany(_ => suits, (rank, suit) => new Card(rank,suit))
            .ToList();
        
        deck.Shuffle();

        return deck;
    }
}

public static class ListExtension
{
    private static readonly Random Rng = new Random();  

    public static void Shuffle<T>(this IList<T> list)  
    {  
        var n = list.Count;  
        while (n > 1) {
            n--;
            var k = Rng.Next(n + 1);  
            (list[k], list[n]) = (list[n], list[k]);
        }  
    }
}