using System.Collections.Generic;
using UnityEngine;

public class MoveStackCommand : ICommand
{
    private readonly List<Card> cards;
    private readonly List<Transform> fromParents;
    private readonly List<Vector3> fromPositions;
    private readonly List<int> fromOrders;

    private readonly Transform toParent;
    private readonly Vector3 baseToPosition;
    private readonly int baseToOrder;

    public MoveStackCommand(
        List<Card> cards,
        List<Transform> fromParents,
        List<Vector3> fromPositions,
        List<int> fromOrders,
        Transform toParent,
        Vector3 baseToPosition,
        int baseToOrder)
    {
        this.cards = new List<Card>(cards);
        this.fromParents = new List<Transform>(fromParents);
        this.fromPositions = new List<Vector3>(fromPositions);
        this.fromOrders = new List<int>(fromOrders);

        this.toParent = toParent;
        this.baseToPosition = baseToPosition;
        this.baseToOrder = baseToOrder;
    }

    public void Execute()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            card.transform.SetParent(toParent);
            card.transform.localPosition = baseToPosition + Vector3.down * 0.3f * i;
            card.SetSortingOrder(baseToOrder + i);
        }
    }

    public void Undo()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            card.transform.SetParent(fromParents[i]);
            card.transform.localPosition = fromPositions[i];
            card.SetSortingOrder(fromOrders[i]);
        }
    }
}