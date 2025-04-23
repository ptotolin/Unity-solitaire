using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Card : MonoBehaviour
{
    public Suit Suit { get; private set; }
    public Rank Rank { get; private set; }
    public bool IsFaceUp { get; private set; }

    private SpriteRenderer spriteRenderer;
    private Vector3 dragOffset;
    private Vector3 originalPosition;
    private int originalSortingOrder;
    private Transform originalParent;
    private bool isDragging;

    public bool CanAcceptCard(Card other)
    {
        if (!this.IsFaceUp || !other.IsFaceUp)
            return false;
        
        bool differentColor = IsRed(Suit) != IsRed(other.Suit);
        bool oneLess = true;//(int)other.Rank == (int)this.Rank - 1;

        return differentColor && oneLess;
    }

    private bool IsRed(Suit suit)
    {
        return suit == Suit.Hearts || suit == Suit.Diamonds;
    }
    
    public void Initialize(Suit suit, Rank rank, Sprite faceSprite, Sprite backSprite, bool faceUp = false)
    {
        Suit = suit;
        Rank = rank;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = faceUp ? faceSprite : backSprite;
        IsFaceUp = faceUp;
    }

    public int GetSortingOrder()
    {
        return spriteRenderer.sortingOrder;
    }
    
    public void SetSortingOrder(int order)
    {
        if (spriteRenderer != null)
            spriteRenderer.sortingOrder = order;
    }

    public void Flip(Sprite faceSprite, Sprite backSprite)
    {
        IsFaceUp = !IsFaceUp;
        spriteRenderer.sprite = IsFaceUp ? faceSprite : backSprite;
    }
}
