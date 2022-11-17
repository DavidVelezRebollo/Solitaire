using DVR.Classes;
using UnityEngine;

namespace DVR.Interfaces {
    public interface ICardStack {
        #region Getters

        /// <summary>
        /// Gets the last card of the stack.
        /// </summary>
        /// <returns>The last card of the stack</returns>
        public Card GetCard();

        /// <summary>
        /// Gets the the last game object containing a card inside the stack
        /// </summary>
        /// <returns>A game object which contains a card</returns>
        public GameObject GetCardGameObject();

        /// <summary>
        /// Get a card inside the stack at the index position.
        /// </summary>
        /// <param name="index">The position of the card</param>
        /// <returns>A card</returns>
        public Card GetCard(int index);

        /// <summary>
        /// Gets a game object which contains a card at the index position
        /// </summary>
        /// <param name="index">The position of the game object</param>
        /// <returns>A game object which contains a card</returns>
        public GameObject GetCardGameObject(int index);

        /// <summary>
        /// Gets the sorting order of the last card inside the stack
        /// </summary>
        /// <returns>The maximum sorting order of the stack</returns>
        public int GetMaxSortingOrder();
        
        /// <summary>
        /// Return the number of cards inside the stack
        /// </summary>
        /// <returns>The number of cards inside the stack</returns>
        public int CardCount();
        
        #endregion

        #region Setters

        /// <summary>
        /// Adds a card to the stack.
        /// </summary>
        /// <param name="card">Card to be added</param>
        /// <param name="cardGameObject">Game object that contains that card</param>
        public void AddCard(Card card, GameObject cardGameObject = null);

        /// <summary>
        /// Removes the last card of the stack with its game object.
        /// </summary>
        public void RemoveCard();

        /// <summary>
        /// Remove a card at the index position with its game object.
        /// </summary>
        /// <param name="index">Index of the card to be removed</param>
        public void RemoveCard(int index);

        /// <summary>
        /// Remove a certain card from the stack
        /// </summary>
        /// <param name="card">The card to remove</param>
        /// <param name="cardGameObject">The Game Object attached to the card</param>
        public void RemoveCard(Card card, GameObject cardGameObject);

        /// <summary>
        /// Increase the maximum sorting order by 1
        /// </summary>
        public void IncreaseMaxSortingOrder();

        /// <summary>
        /// Decrease the maximum sorting order by 1
        /// </summary>
        public void DecreaseMaxSortingOrder();

        #endregion

        /// <summary>
        /// Clear the list of cards.
        /// </summary>
        public void Clear();
        
            /// <summary>
        /// Check if the stack has cards left.
        /// </summary>
        /// <returns>True if it has cards. False otherwise</returns>
        public bool HasCards();

        /// <summary>
        /// Change the position of two cards inside of the stack.
        /// </summary>
        /// <param name="a">Index of the first card</param>
        /// <param name="b">Index of the second card</param>
        public void ChangeCards(int a, int b);
    }
}
