using DVR.Classes;
using UnityEngine;

namespace DVR.Components
{
    public class StolenCards : MonoBehaviour {
        public readonly CardStack Cards = new CardStack();

        public void RemoveCard() {
            Destroy(Cards.GetCardGameObject(Cards.CardCount()));
            Cards.RemoveCard();
        }
    }
}
