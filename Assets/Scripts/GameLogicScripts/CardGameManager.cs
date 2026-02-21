using System.Collections.Generic;
using SOScripts;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogicScripts
{
    public class CardGameManager: MonoBehaviour
    {
        [Header("Card Generation Data")]
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        
        [SerializeField] private SOCardsData cardsData;
        [SerializeField] private GameObject cardPrefab;

        [Header("Grid Generation Data")]
        [SerializeField] private RectTransform cardsContainer;
        
        [SerializeField] private float verticalPadding;
        [SerializeField] private float horizontalPadding;
        
        [SerializeField] private float maxCardHeight = 350f;
        [SerializeField] private float cardAspectRatio = 1f / 1f;
        
        private CardGenerator _cardGenerator;
        private GridGenerator _gridGenerator;

        //To track open cards
        private readonly Dictionary<int, Card> _openCards = new();

        private void Start()
        {
            _cardGenerator = new CardGenerator();
            _gridGenerator = new GridGenerator(maxCardHeight, cardAspectRatio);
            var cardsList = _cardGenerator.Generate(cardsData, columns, rows, cardPrefab);
            _gridGenerator.GenerateGrid(cardsContainer, cardsList, rows, columns, verticalPadding, horizontalPadding);
            
            AssignButtonListeners(cardsList);
        }

        private void AssignButtonListeners(List<Card> cardsList)
        {
            foreach (var card in cardsList)
            {
                var button = card.CardTransform.gameObject.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => OnCardClicked(card, button));
            }
        }

        private void MatchFound(Card twin1, Card twin2)
        {
            
        }

        private void OnCardClicked(Card card, Button button)
        {
            Debug.Log($"Card {card.ID} clicked");
            if (_openCards.TryGetValue(card.ID, out var existingTwin))
            {
                MatchFound(card, existingTwin);
                return;
            }
            _openCards.Add(card.ID, card);
        }
    }
}