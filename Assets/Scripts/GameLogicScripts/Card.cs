using UnityEngine;

namespace GameLogicScripts
{
    public struct Card
    {
        public int ID;
        public string CardName;
        public readonly RectTransform CardTransform;

        public Card(int id, string cardName, RectTransform cardTransform)
        {
            ID = id;
            CardName = cardName;
            CardTransform = cardTransform;
        }
    }
}