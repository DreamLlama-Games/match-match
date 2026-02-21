using UnityEngine;

public struct Card
{
    public int ID;
    public string CardName;
    public RectTransform CardTransform;

    public Card(int id, string cardName, RectTransform cardTransform)
    {
        ID = id;
        CardName = cardName;
        CardTransform = cardTransform;
    }
}