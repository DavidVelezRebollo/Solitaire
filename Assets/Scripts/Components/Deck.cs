using System.Collections.Generic;
using UnityEngine;
using DVR.Classes;

namespace DVR.Components {
    public class Deck : MonoBehaviour {
        private List<Card> _deckCards;

        private const int _CARD_NUMBER = 52;

        public void InitializeDeck() {
            CardType type = CardType.Diamonds;
            
            for (int i = 0; i < _CARD_NUMBER; i++) {
                if (i % 13 == 0 && i != 0)
                    type++;

                Card card = new Card(type, i % 13 + 1);
                
                _deckCards.Add(card);
            }
        }
    }
}
