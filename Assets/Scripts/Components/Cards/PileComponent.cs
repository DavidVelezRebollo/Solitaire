using DVR.Classes.Cards;
using UnityEngine;

namespace DVR.Components.Cards
{
    public class PileComponent : MonoBehaviour {
        private BoxCollider2D _collider;
        protected readonly CardStack CardStack  = new CardStack();

        #region Unity Events

        protected void Start() {
            if (CompareTag("StolenCards")) return;
            
            _collider = GetComponent<BoxCollider2D>();
        }

        protected void Update() {
            if (!_collider) return;
            
            _collider.enabled = !CardStack.HasCards();
        }

        #endregion
        
        #region Getters

        /// <summary>
        /// Gets the stack of cards
        /// </summary>
        /// <returns>The stack of cards</returns>
        public CardStack GetStack() {
            return CardStack;
        }
        
        /// <summary>
        /// Gets the last card component inside the stack
        /// </summary>
        /// <returns>The last card component inside the stack</returns>
        public CardComponent GetCardComponent() {
            return CardStack.GetCardGameObject().GetComponent<CardComponent>();
        }

        /// <summary>
        /// Gets the card component at the index position inside the stack
        /// </summary>
        /// <returns>The position of the card component inside the stack</returns>
        public CardComponent GetCardComponent(int index) {
            return CardStack.GetCardGameObject(index).GetComponent<CardComponent>();
        }

        #endregion

        #region Setters

        /// <summary>
        ///  Adds a card to the last position of the pile
        /// </summary>
        /// <param name="card">Card proprieties</param>
        /// <param name="cardGameObject">Game object where the card is instantiated</param>
        /// <param name="attachCard">Checks if the card is going to be attached to another one</param>
        public void AddCard(Card card, GameObject cardGameObject, bool attachCard) {
            CardStack.AddCard(card, cardGameObject);

            cardGameObject.transform.parent =
                attachCard ? CardStack.GetCardGameObject(CardStack.CardCount() - 2).transform : transform;
        }
        
        /// <summary>
        /// Removes and destroy the last card of the stack
        /// </summary>
        public void DestroyCard() {
            Destroy(CardStack.GetCardGameObject(CardStack.CardCount() - 1));
            CardStack.RemoveCard();
        }

        #endregion
        
    }
}
