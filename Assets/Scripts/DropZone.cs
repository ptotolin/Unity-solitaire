using UnityEngine;

public enum ZoneType { Tableau, Foundation, Waste, Stock }

public class DropZone : MonoBehaviour
{
    public ZoneType zoneType;

    public bool CanAcceptCard(Card card)
    {
        // TODO: Make some logic

        return true;
    }

    public void AcceptCard(Card card)
    {
        card.transform.SetParent(transform);

        // Считаем только прямых детей-карт (не вложенных в другие карты)
        int existingCards = 0;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Card>() != null)
                existingCards++;
        }

        Vector3 offset = Vector3.zero;

        if (zoneType == ZoneType.Tableau)
        {
            offset = Vector3.down * 0.3f * existingCards;
        }

        card.transform.localPosition = offset;
        card.SetSortingOrder(existingCards);
    }

    // Draw реализуем только для Stock-зоны:
    public Card Draw()
    {
        if (zoneType != ZoneType.Stock || transform.childCount == 0)
            return null;

        var topCardTransform = transform.GetChild(transform.childCount - 1);
        topCardTransform.SetParent(null);
        return topCardTransform.GetComponent<Card>();
    }
}
