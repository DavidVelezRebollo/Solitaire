
using DVR.Classes;

namespace DVR.Interfaces {
    public interface ICardStack {
        /// <summary>
        /// Gets the last card of the stack.
        /// </summary>
        /// <returns>The last card of the stack</returns>
        public Card GetCard();

        /// <summary>
        /// Get a card inside the stack at the index position.
        /// </summary>
        /// <param name="index">The position of the card</param>
        /// <returns>A card</returns>
        public Card GetCard(int index);
        
        /// <summary>
        /// Adds a card to the stack.
        /// </summary>
        /// <param name="card">Card to be added</param>
        public void AddCard(Card card);

        /// <summary>
        /// Remove a card at the index position.
        /// </summary>
        /// <param name="index">Index of the card to be removed</param>
        public void RemoveCard(int index);

        /// <summary>
        /// Removes the last card of the stack.
        /// </summary>
        public void RemoveCard();

        /// <summary>
        /// Clear the list of cards.
        /// </summary>
        public void Clear();

        /// <summary>
        /// Return the number of cards inside the stack
        /// </summary>
        /// <returns>The number of cards inside the stack</returns>
        public int CardCount();

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
