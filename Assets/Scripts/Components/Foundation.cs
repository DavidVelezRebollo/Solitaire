using DVR.Classes;
using DVR.Shared;
using UnityEngine;

namespace DVR.Components
{
    public class Foundation : CardPile {
        [Tooltip("Type of the cards inside foundation")]
        [SerializeField] private CardType FoundationType;

        #region Setters

        /// <summary>
        /// Gets the type of cards inside the foundation
        /// </summary>
        /// <returns>The type of cards inside the foundation</returns>
        public CardType GetFoundationType() {
            return FoundationType;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Checks if a card can be placed inside the foundation
        /// </summary>
        /// <param name="card">Card to be placed</param>
        /// <returns>True if the card can be placed. False otherwise</returns>
        public bool CanPlace(Card card) {
            if (CardStack.HasCards() && card.GetCardValue() == CardStack.GetCard().GetCardValue() + 1)
                return true;

            return card.GetCardValue() == 1;
        }

        #endregion

    }
}
