namespace Domain.Models;

public record Card (Card.CardRank Rank, Card.CardSuit Suit)
{
    public enum CardSuit
    {
        Spades,
        Hearts,
        Clubs,
        Diamonds
    }
    
    public enum CardRank
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }
}