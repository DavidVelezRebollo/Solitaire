using System.Collections.Generic;
using DVR.Classes;
using UnityEngine;

namespace DVR.Components
{
    public class CardPile : MonoBehaviour {
        protected readonly CardStack CardStack  = new CardStack();
        private readonly List<GameObject> _cardGameObjects = new List<GameObject>();

        public void AddCard(Card card, GameObject cardGameObject) {
            CardStack.AddCard(card);
            _cardGameObjects.Add(cardGameObject);
            card.SetCardStack(CardStack);

            cardGameObject.transform.parent = transform;
        }

        public void RemoveCard() {
            CardStack.RemoveCard(); 
            _cardGameObjects.RemoveAt(_cardGameObjects.Count - 1);
        }

        public Card GetLastCard() {
            return CardStack.GetCard();
        }

        public GameObject GetLastCardGo() {
            return _cardGameObjects[_cardGameObjects.Count - 1];
        }

        public CardStack GetCardStack() {
            return CardStack;
        }
    }
}
