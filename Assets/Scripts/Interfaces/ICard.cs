using DVR.Classes;
using DVR.Shared;
using UnityEngine;

namespace DVR.Interfaces {
    public interface ICard {

        #region Getters

        /// <summary>
        /// Gets the type of the card
        /// </summary>
        /// <returns>The type of the card</returns>
        public CardType GetCardType();

        /// <summary>
        /// Gets the numeric value of the card
        /// </summary>
        /// <returns>The value of the card</returns>
        public int GetCardValue();

        /// <summary>
        /// Gets the sprite of the card
        /// </summary>
        /// <returns>The sprite of the card</returns>
        public Sprite GetCardSprite();

        /// <summary>
        /// Gets the sorting order of the card
        /// </summary>
        /// <returns>The sorting order of the card</returns>
        public int GetSortingOrder();
        
        /// <summary>
        /// Checks if the card face can be seen
        /// </summary>
        /// <returns>True if the card face can be seen. False otherwise</returns>
        public bool IsVisible();

        #endregion

        #region Setters

        /// <summary>
        /// Sets the sorting order of the card
        /// </summary>
        /// <param name="sortingOrder">New sorting order of the card</param>
        public void SetSortingOrder(int sortingOrder);

        /// <summary>
        /// Sets if the card is visible or not
        /// </summary>
        /// <param name="visible">The visibility state of the card</param>
        public void SetVisible(bool visible);
        
        #endregion

        #region Methods

        /// <summary>
        /// Checks if the card can be placed on top of another card
        /// </summary>
        /// <param name="card">The card where the current card is wanted to be placed</param>
        /// <returns>True if the card can be placed. False otherwise</returns>
        public bool CanPlace(Card card);
        
        #endregion
    }
}
