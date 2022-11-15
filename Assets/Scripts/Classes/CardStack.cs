using System.Collections.Generic;
using DVR.Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace DVR.Classes {
    public class CardStack : ICardStack {
        private readonly List<Card> _cards = new List<Card>();
        private readonly List<GameObject> _cardsGameObjects = new List<GameObject>();
        
        public Card GetCard() {
            return _cards[_cards.Count - 1];
        }

        public GameObject GetCardGameObject() {
            return _cardsGameObjects[_cardsGameObjects.Count - 1];
        }
        
        public Card GetCard(int index) {
            return _cards[index];
        }

        public GameObject GetCardGameObject(int index) {
            return _cardsGameObjects[index];
        }
        
        public void AddCard(Card card, [CanBeNull] GameObject cardGameObject) {
            _cards.Add(card);
            
            if(cardGameObject != null) _cardsGameObjects.Add(cardGameObject);
        }
        
        public void RemoveCard() {
            _cards.RemoveAt(_cards.Count - 1);
            
            if(_cardsGameObjects.Count > 0)
                _cardsGameObjects.RemoveAt(_cardsGameObjects.Count - 1);
        }
        
        public void RemoveCard(int index) {
            _cards.RemoveAt(index);
        }
        
        public void Clear() {
            _cards.Clear();
        }
        
        public int CardCount() {
            return _cards.Count;
        }
        
        public bool HasCards() {
            return _cards.Count != 0;
        }
        
        public void ChangeCards(int a, int b) {
            (_cards[a], _cards[b]) = (_cards[b], _cards[a]);
        }
    }
}
