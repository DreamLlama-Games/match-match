using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SOScripts
{
    [Serializable]
    public class Card
    {
        public Sprite cardBack;
        public Sprite cardFront;
        public string cardName;
        public int id;
    }
    
    [CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/Card Data", order = 0)]
    public class SOCardData : ScriptableObject
    {
        [SerializeField] private List<Card> cards = new();
        public List<Card> GetCards() => cards;
        
        private readonly Dictionary<int, Card> _cardDictionary = new();

        private void OnValidate()
        {
            foreach (var card in cards) _cardDictionary.Add(card.id, card);
        }

        public Card GetCard(int id)
        {
            return !_cardDictionary.TryGetValue(id, out var card) ? 
                throw new RuntimeWrappedException("Card Not Found With Valid Id: " + id) : 
                card;
        }
    }
}