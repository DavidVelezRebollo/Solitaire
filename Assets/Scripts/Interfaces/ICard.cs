using DVR.Classes;

namespace DVR.Interfaces {
    public interface ICard {
        /// <summary>
        /// Checks if the card face can be seen
        /// </summary>
        /// <returns>True if the card face can be seen. False otherwise</returns>
        public bool IsVisible();

        /// <summary>
        /// Checks if the card can be placed on top of another card
        /// </summary>
        /// <param name="card">The card where the current card is wanted to be placed</param>
        /// <returns>True if the card can be placed. False otherwise</returns>
        public bool CanPlace(Card card);
    }
}
