using System.Collections.Generic;
using DVR.Classes;
using DVR.Shared;
using UnityEngine;

namespace DVR.Components
{
    public class Foundation : CardPile {
        public CardType FoundationType;

        public bool CanPlace(Card card) {
            if (CardStack.HasCards() && card.GetCardValue() == CardStack.GetCard().GetCardValue() + 1)
                return true;

            return card.GetCardValue() == 1;
        }
    }
}
