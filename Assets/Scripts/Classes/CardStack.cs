using System.Collections.Generic;
using DVR.Interfaces;

namespace DVR.Classes {
    public class CardStack : ICardStack {
        private readonly List<Card> _cards = new List<Card>();

        public Card GetCard() {
            return _cards[_cards.Count - 1];
        }

        public Card GetCard(int index) {
            return _cards[index];
        }

        public void AddCard(Card card) {
            _cards.Add(card);
        }

        public void RemoveCard() {
            _cards.RemoveAt(_cards.Count - 1);
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
