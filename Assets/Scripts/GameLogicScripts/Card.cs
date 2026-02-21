using UnityEngine;

namespace GameLogicScripts
{
    public struct Card
    {
        public int ID;
        public string CardName;
        public readonly RectTransform CardTransform;
        
        //transform
        private Vector3 _scale;
        private Vector3 _rotation;

        public Card(int id, string cardName, RectTransform cardTransform)
        {
            ID = id;
            CardName = cardName;
            CardTransform = cardTransform;
            
            _scale = CardTransform.localScale;
            _rotation = CardTransform.localEulerAngles;
        }

        public void ResetTransform()
        {
            CardTransform.localScale = _scale;
            CardTransform.localEulerAngles = _rotation;
        }

        public void SetTransform()
        {
            _scale = CardTransform.localScale;
            _rotation = CardTransform.localEulerAngles;
        }
    }
}