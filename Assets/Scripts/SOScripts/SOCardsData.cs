using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SOScripts
{
    [Serializable]
    public class IndividualCardData
    {
        public Sprite cardBack;
        public Sprite cardFront;
        public string cardName;
        public int id;
    }
    
    [CreateAssetMenu(fileName = "CardsData", menuName = "Scriptable Objects/Cards Data", order = 0)]
    public class SOCardsData : ScriptableObject
    {
        [SerializeField] private List<IndividualCardData> cards = new();
        public List<IndividualCardData> GetCards() => cards;
        
        private readonly Dictionary<int, IndividualCardData> _cardDictionary = new();

        private void OnValidate()
        {
            foreach (var card in cards) _cardDictionary.Add(card.id, card);
        }

        public IndividualCardData GetCard(int id)
        {
            return !_cardDictionary.TryGetValue(id, out var card) ? 
                throw new RuntimeWrappedException("Card Not Found With Valid Id: " + id) : 
                card;
        }
    }
}