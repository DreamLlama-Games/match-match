using System.Collections.Generic;
using SOScripts;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace GameLogicScripts
{
    public class CardGenerator
    {
        public List<Card> Generate(SOCardsData cardsData, int column, int row, GameObject cardPrefab)
        {
            var totalItems = row * column;
            var uniqueItemsNeeded = (int)(totalItems * 0.5f);

            var cardsList = cardsData.GetCards();
            if(cardsList.Count  < uniqueItemsNeeded) throw new System.Exception("Not enough data to generate cards");

            List<Card> cards = new();
            for (var i = 0; i < uniqueItemsNeeded; i++)
            {
                var data = cardsList[i];
                var twin1 = CreateCard(cardPrefab, data.cardFront, data.cardBack, data.cardName + data.id);
                var twin2 = CreateCard(cardPrefab, data.cardFront, data.cardBack, data.cardName + data.id);
                
                cards.Add(new Card(data.id, data.cardName, twin1));
                cards.Add(new Card(data.id, data.cardName, twin2));
            }
            return cards;
        }

        private RectTransform CreateCard(GameObject cardPrefab, Sprite front, Sprite back, string name)
        {
            var card = Object.Instantiate(cardPrefab);
            card.name = name;
            
            if(!card.TryGetComponent<RectTransform>(out var rectTransform)) throw new System.Exception("Not a RectTransform");
            var imageComponents = card.GetComponentsInChildren<Image>();
            
            //For front and back
            if (imageComponents.Length < 2) return rectTransform;

            imageComponents[0].sprite = back;
            imageComponents[1].sprite = front;
            return rectTransform;
        }
    }
}