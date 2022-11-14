using System.Collections.Generic;
using DVR.Classes;
using DVR.Shared;
using UnityEngine;

namespace DVR.Components
{
    public class Foundation : MonoBehaviour {
        public CardType FoundationType;
        
        private readonly CardStack _cardStack  = new CardStack();
        private readonly List<GameObject> _cardGameObjects = new List<GameObject>();
        
        public void AddCard(Card card, GameObject cardGameObject) {
            _cardStack.AddCard(card);
            _cardGameObjects.Add(cardGameObject);

            cardGameObject.transform.parent = transform;
        }

        public void RemoveCard() {
            _cardStack.RemoveCard(); 
            _cardGameObjects.RemoveAt(_cardGameObjects.Count - 1);
        }

        public bool CanPlace(Card card) {
            if (_cardStack.HasCards() && card.GetCardValue() == _cardStack.GetCard().GetCardValue() + 1)
                return true;

            return card.GetCardValue() == 1;
        }
    }
}
