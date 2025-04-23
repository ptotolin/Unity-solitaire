using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Sprite[] cardFaces;
    [SerializeField] private Sprite cardBack;

    public Sprite CardBack => cardBack;

    public void GenerateAndShuffleToStock(DropZone stockZone)
    {
        var cards = new System.Collections.Generic.List<Card>();

        foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
        {
            for (int i = 1; i <= 13; i++)
            {
                var newCard = Instantiate(cardPrefab);
                newCard.Initialize(suit, (Rank)i, GetFaceSprite(suit, (Rank)i), cardBack, false);
                cards.Add(newCard);
            }
        }

        Shuffle(cards);

        // Move cards to Stock
        foreach (var card in cards)
        {
            card.transform.SetParent(stockZone.transform);
            card.transform.position = stockZone.transform.position;
        }
    }

    private void Shuffle(System.Collections.Generic.List<Card> cards)
    {
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            var temp = cards[i];
            cards[i] = cards[rnd];
            cards[rnd] = temp;
        }
    }

    public Sprite GetFaceSprite(Suit suit, Rank rank)
    {
        int index = ((int)suit * 13) + (int)rank - 1;
        return cardFaces[index];
    }
}
