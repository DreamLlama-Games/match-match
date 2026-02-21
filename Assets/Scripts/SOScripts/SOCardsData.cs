using System;
using System.Collections.Generic;
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

        public void Initialize()
        {
            foreach (var card in cards) _cardDictionary.Add(card.id, card);
        }

        public IndividualCardData GetCard(int id)
        {
            return !_cardDictionary.TryGetValue(id, out var card) ? 
                throw new System.Exception("Card Not Found With Valid Id: " + id) : 
                card;
        }
    }
}