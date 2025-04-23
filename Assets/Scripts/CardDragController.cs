using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CardDragController : MonoBehaviour
{
    private Card draggingCard;
    private List<Card> draggedStack = new();

    private List<Transform> fromParents = new();
    private List<Vector3> fromPositions = new();
    private List<int> fromOrders = new();

    private Vector3 dragOffset;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryStartDrag();

        if (Input.GetMouseButton(0) && draggingCard != null)
            Drag();

        if (Input.GetMouseButtonUp(0) && draggingCard != null)
            EndDrag();

        if (Input.GetKeyDown(KeyCode.Z))
            CommandManager.Instance.Undo();
    }

    void TryStartDrag()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

        Card topCard = null;
        int topOrder = -1;

        foreach (var hit in hits)
        {
            var card = hit.collider.GetComponent<Card>();
            if (card != null && card.IsFaceUp)
            {
                int order = card.GetSortingOrder();
                if (order > topOrder)
                {
                    topCard = card;
                    topOrder = order;
                }
            }
        }

        if (topCard != null)
            StartDrag(topCard);
    }

    void StartDrag(Card card)
    {
        draggingCard = card;
        draggedStack.Clear();
        fromParents.Clear();
        fromPositions.Clear();
        fromOrders.Clear();

        draggedStack.Add(card);

        foreach (Transform child in card.transform)
        {
            Card subCard = child.GetComponent<Card>();
            if (subCard != null && subCard.IsFaceUp)
                draggedStack.Add(subCard);
            else
                break;
        }

        foreach (var c in draggedStack)
        {
            fromParents.Add(c.transform.parent);
            fromPositions.Add(c.transform.localPosition);
            fromOrders.Add(c.GetSortingOrder());

            c.transform.SetParent(null);
            c.SetSortingOrder(1000 + draggedStack.IndexOf(c));
        }

        dragOffset = card.transform.position - GetMouseWorldPos();
    }

    void Drag()
    {
        Vector3 basePos = GetMouseWorldPos() + dragOffset;

        for (int i = 0; i < draggedStack.Count; i++)
        {
            draggedStack[i].transform.position = basePos + Vector3.down * 0.3f * i;
        }
    }

    void EndDrag()
    {
        Debug.Log($"EndDrag");

        Vector2 mousePos = GetMouseWorldPos();
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

        if (hits != null)
        {
            var allCards = hits
                .Where(h => h.collider.GetComponent<Card>() != null)
                .Select(h => h.collider.GetComponent<Card>())
                .OrderByDescending(card => card.GetSortingOrder())
                .ToList();

            if (allCards.Count > 1)
            {
                int index = 0;
                while (index < allCards.Count() && draggingCard == allCards[index]) {
                    index++;
                }
                var otherCard = allCards[index];

                if (otherCard != null && otherCard.CanAcceptCard(draggingCard))
                {
                    int startIndex = otherCard.transform.parent.childCount + 1;
                    Vector3 basePos = Vector3.down * 0.3f * startIndex;

                    var cmd = new MoveStackCommand(
                        draggedStack,
                        fromParents,
                        fromPositions,
                        fromOrders,
                        otherCard.transform.parent,
                        basePos,
                        startIndex
                    );
                    CommandManager.Instance.ExecuteCommand(cmd);
                    draggingCard = null;
                    draggedStack.Clear();
                    return;
                } 
                else if (allCards.Count > 0) {
                    for (int i = 0; i < draggedStack.Count; i++)
                    {
                        var card = draggedStack[i];
                        card.transform.SetParent(fromParents[i]);
                        card.transform.localPosition = fromPositions[i];
                        card.SetSortingOrder(fromOrders[i]);
                    }

                    draggingCard = null;
                    draggedStack.Clear();
                }
            } 

            var dropZoneHit = hits.FirstOrDefault(h => h.collider.GetComponent<DropZone>());
            if (dropZoneHit != null)
            {
                var dropZone = dropZoneHit.collider.GetComponent<DropZone>();
                if (dropZone != null && dropZone.CanAcceptCard(draggingCard))
                {
                    int startIndex = dropZone.transform.childCount;
                    Vector3 basePos = Vector3.down * 0.3f * startIndex;

                    var cmd = new MoveStackCommand(
                        draggedStack,
                        fromParents,
                        fromPositions,
                        fromOrders,
                        dropZone.transform,
                        basePos,
                        startIndex
                    );
                    CommandManager.Instance.ExecuteCommand(cmd);
                    draggingCard = null;
                    draggedStack.Clear();
                    return;
                }
            }
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
