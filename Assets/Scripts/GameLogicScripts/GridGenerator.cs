using System.Collections.Generic;
using MyUtilityScripts;
using UnityEngine;

namespace GameLogicScripts
{
    public class GridGenerator
    {
        private readonly float _maxCardHeight;
        private readonly float _cardAspectRatio;

        public GridGenerator(float maxCardHeight, float cardAspectRatio)
        {
            _maxCardHeight = maxCardHeight;
            _cardAspectRatio = cardAspectRatio;
        }

        private void GenerateGrid(RectTransform parentContainer, List<Card> cards, int rows, int cols, float verticalPadding, float horizontalPadding)
        {
            cards.Shuffle(); //Shuffle the cards randomly
            
            var totalWidth = parentContainer.rect.width;
            var totalHeight = parentContainer.rect.height;
            
            //Accounting for padding
            var availableHeight = totalHeight - (verticalPadding * (cols - 1));
            var availableWidth = totalWidth - (horizontalPadding * (rows - 1));
            
            //Account for card aspect ratio
            var cardHeight = availableWidth / cols;
            cardHeight = cardHeight > _maxCardHeight ? _maxCardHeight : cardHeight;
            var cardWidth = cardHeight * _cardAspectRatio;
            
            //Because anchor is in the center
            var halfCardWidth = cardWidth / 2; 
            var halfCardHeight = cardHeight / 2;
            
            var startPosX = ((rows * cardWidth) + (rows - 1) * horizontalPadding) * -0.5f;
            var startPosY = ((cols * cardHeight) + (cols - 1) * verticalPadding) * -0.5f;

            var k = 0;
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    var cardRect = cards[k].CardTransform;
                    cardRect.SetParent(parentContainer,false);

                    cardRect.localScale = Vector3.one;
                    cardRect.localRotation = Quaternion.identity;
                    cardRect.sizeDelta = new Vector2 (cardWidth, cardHeight); //set card size

                    var posX = startPosX + i * (cardWidth + horizontalPadding) + halfCardWidth;
                    var posY = startPosY + j * (cardHeight + verticalPadding) + halfCardHeight;
                    
                    cardRect.anchoredPosition = new Vector3 (posX, posY, 0f);
                    k = k + 1 < cards.Count ? k + 1 : 0; //reusing same elements
                }
            }
        }
    }
}