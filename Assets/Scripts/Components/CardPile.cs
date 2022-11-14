using System.Collections.Generic;
using DVR.Classes;
using UnityEngine;

namespace DVR.Components
{
    public class CardPile : MonoBehaviour {
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

        public Card GetLastCard() {
            return _cardStack.GetCard();
        }
    }
}
