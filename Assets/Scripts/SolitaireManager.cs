using UnityEngine;

public class SolitaireManager : MonoBehaviour
{
    [SerializeField] private Deck deck;
    [SerializeField] private DropZone stockZone;
    [SerializeField] private DropZone wasteZone;
    [SerializeField] private DropZone[] tableauZones;

    void Start()
    {
        deck.GenerateAndShuffleToStock(stockZone);
        DealCardsToTableau();
    }

    public void Undo()
    {
        CommandManager.Instance.Undo();
    }

    private void DealCardsToTableau()
    {
        for (int i = 0; i < tableauZones.Length; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                Card card = stockZone.Draw();
                if (card == null)
                {
                    Debug.LogError("Not enough cards in Stock to deal.");
                    return;
                }

                tableauZones[i].AcceptCard(card);

                // Open top card
                if (j == i && !card.IsFaceUp)
                    card.Flip(deck.GetFaceSprite(card.Suit, card.Rank), deck.CardBack);
            }
        }
    }

    // Метод, который вызывается игроком, когда он берёт карту из Stock
    public void DrawCardFromStock()
    {
        Card card = stockZone.Draw();

        if (card != null)
        {
            // Переворачиваем карту лицом вверх и отправляем в Waste
            if (!card.IsFaceUp)
                card.Flip(deck.GetFaceSprite(card.Suit, card.Rank), deck.CardBack);

            wasteZone.AcceptCard(card);
        }
        else
        {
            Debug.Log("No more cards in Stock.");
            // Тут может быть логика повторного использования Waste, если нужно
        }
    }
}