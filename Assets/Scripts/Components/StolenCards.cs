namespace DVR.Components
{
    public class StolenCards : CardPile {
        
        /// <summary>
        /// Removes and destroy the last card of the stack
        /// </summary>
        public void RemoveCard() {
            Destroy(CardStack.GetCardGameObject(CardStack.CardCount() - 1));
            CardStack.RemoveCard();
        }
    }
}
