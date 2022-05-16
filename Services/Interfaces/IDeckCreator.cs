using Domain.Models;

namespace Services.Interfaces;

public interface IDeckCreator
{
    public IEnumerable<Card> CreateDeck();
}