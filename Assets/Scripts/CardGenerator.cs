using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SOScripts;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class CardGenerator
{
    public List<Card> Generate(SOCardsData cardsData, int column, int row, GameObject cardPrefab)
    {
        var totalItems = row * column;
        var uniqueItemsNeeded = (int)(totalItems * 0.5f);

        var cardsList = cardsData.GetCards();
        if(cardsList.Count  < uniqueItemsNeeded) throw new RuntimeWrappedException("Not enough data to generate cards");

        List<Card> cards = new();
        for (var i = 0; i < uniqueItemsNeeded; i++)
        {
            var card = Object.Instantiate(cardPrefab);
            var rectTransform = card.GetComponent<RectTransform>();
            var imageComponents = card.GetComponentsInChildren<Image>();
            
            cards.Add(new Card(cardsList[i].id, cardsList[i].cardName, rectTransform));

            //For front and back
            if (imageComponents.Length != 2) continue;
            
            imageComponents[0].sprite = cardsList[0].cardFront;
            imageComponents[1].sprite = cardsList[1].cardBack;
        }
        return cards;
    }
}