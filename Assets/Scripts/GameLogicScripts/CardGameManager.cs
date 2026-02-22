using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SOScripts;
using UnityEngine;
using UnityEngine.UI;
using VisualAnimationScripts;

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
        
        [Header("Animation Data")]
        [SerializeField] private float cardFlipAnimationDuration = 0.5f;
        [SerializeField] private float cardFlashAnimationDuration = 0.5f;
        [SerializeField] private float cardMoveAnimationDuration = 0.5f;
        [SerializeField] private float cardFlashScale = 1.1f;
        [SerializeField] private float cardShrinkScale = 0.3f;
        [SerializeField] private float cardRevealDuration = 0.5f;
        [SerializeField] private RectTransform discardPileArea;
        
        private const float OpenCardRotation = 180f;
        private const float ClosedCardRotation = 0f;
        
        private float _buttonDisableTimeOutDuration = 0.5f;
        
        private CardGenerator _cardGenerator;
        private GridGenerator _gridGenerator;
        private CardAnimationHandler _cardAnimationHandler;

        //To track open cards
        private readonly Dictionary<int, Card> _openCards = new();
        private Card _lastSeenCard;

        private void Start()
        {
            //setup handlers
            _cardGenerator = new CardGenerator();
            _gridGenerator = new GridGenerator(maxCardHeight, cardAspectRatio);
            _cardAnimationHandler = new CardAnimationHandler(this,cardFlipAnimationDuration,cardMoveAnimationDuration,
                cardRevealDuration,cardFlashAnimationDuration,cardFlashScale,cardShrinkScale,OpenCardRotation,ClosedCardRotation);
            _buttonDisableTimeOutDuration = 2 * cardFlipAnimationDuration + cardRevealDuration;
            
            SetupView();
        }

        private void SetupView()
        {
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
            _cardAnimationHandler?.OnTwinFound(twin1, twin2, discardPileArea);
            _openCards.Remove(twin1.ID);
            _openCards.Remove(twin2.ID);
        }

        private IEnumerator TimeOutTask(float duration, Action onStart = null, Action onEnd = null)
        {
            onStart?.Invoke();
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            onEnd?.Invoke();
        }

        private void OnCardSelected(Card card)
        {
            _openCards.Add(card.ID, card);
            _cardAnimationHandler.OnCardSelected(card);
        }

        private void ResetCardAfterFlash(Card card, Button button)
        {
            StartCoroutine(TimeOutTask(_buttonDisableTimeOutDuration, onEnd: () => { button.interactable = true; }));
            StartCoroutine(TimeOutTask(cardFlipAnimationDuration + cardRevealDuration, onEnd: () => { _openCards.Remove(card.ID); }));
        }

        private void OnCardClicked(Card card, Button button)
        {
            RemoveOldest(card);
            button.interactable = false;

            if (_openCards.TryGetValue(card.ID, out var existingTwin))
            {
                MatchFound(card, existingTwin);
                return;
            }
            
            OnCardSelected(card);
            ResetCardAfterFlash(card, button);
        }

        private void RemoveOldest(Card card)
        {
            if (_openCards.Count == 0)
            {
                _lastSeenCard = card;
                return;
            }

            if (_openCards.Count != 2) return;
            
            _openCards.Remove(_lastSeenCard.ID);
            _lastSeenCard = _openCards.First().Value;
        }
    }
}