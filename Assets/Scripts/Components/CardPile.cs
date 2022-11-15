using DVR.Classes;
using UnityEngine;

namespace DVR.Components
{
    public class CardPile : MonoBehaviour {
        public readonly CardStack CardStack  = new CardStack();

        /// <summary>
        ///  Adds a card to the last position of the pile
        /// </summary>
        /// <param name="card">Card proprieties</param>
        /// <param name="cardGameObject">Game object where the card is instantiated</param>
        public void AddCard(Card card, GameObject cardGameObject) {
            CardStack.AddCard(card, cardGameObject);
            card.SetCardStack(CardStack);

            cardGameObject.transform.parent = transform;
        }

        /// <summary>
        /// Gets the last card component of the stack
        /// </summary>
        /// <returns>The last card component of the stack</returns>
        public CardComponent GetCardComponent(int index) {
            return CardStack.GetCardGameObject(index).GetComponent<CardComponent>();
        }
    }
}
