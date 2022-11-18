using System.Collections.Generic;
using UnityEngine;

namespace DVR.Classes.Cards {
    public class CardStack {
        // Cards inside the stack
        private readonly List<Card> _cards = new List<Card>();
        // Game Object attached to the stack cards
        private readonly List<GameObject> _cardsGameObjects = new List<GameObject>();
        // Maximum sorting order of the stack
        private int _maxSortingOrder = -1;

        #region Getters

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

        public int GetMaxSortingOrder() {
            return _maxSortingOrder;
        }
        
        public int CardCount() {
            return _cards.Count;
        }

        #endregion

        #region Setters
        
        public void AddCard(Card card, GameObject cardGameObject = null) {
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
            _cardsGameObjects.RemoveAt(_cardsGameObjects.Count - 1);
        }

        public void RemoveCard(Card card, GameObject gameObject) {
            _cards.Remove(card);
            _cardsGameObjects.Remove(gameObject);
        }

        public void IncreaseMaxSortingOrder() {
            _maxSortingOrder++;
        }

        public void DecreaseMaxSortingOrder() {
            _maxSortingOrder--;
        }

        #endregion

        #region Functions

        public bool HasCards() {
            return _cards.Count != 0;
        }

        #endregion

        #region Methods
        
        public void Clear() {
            _cards.Clear();
        }

        public void ChangeCards(int a, int b) {
            (_cards[a], _cards[b]) = (_cards[b], _cards[a]);
        }

        #endregion

        #region Override Methods

        public override string ToString() {
            string result = "";

            result += "Max Sorting Order: " + _maxSortingOrder + "\n";
            result += "Cards in the stack: " + CardCount() + "\n Cards: ";

            for (int i = 0; i < CardCount(); i++) {
                result += GetCard(i) + " ";
            }

            result += "\nCards Game Objects in the stack: ";
            
            for (int i = 0; i < CardCount(); i++) {
                result += GetCardGameObject(i) + " ";
            }

            return result;
        }

        #endregion
    }
}
