using DVR.Shared;
using UnityEngine;

namespace DVR.Classes.Cards {
    public class Card {
        //Type of the card (Heart, Diamond, Spade or Club)
        private readonly CardType _type;
        // Color of the card (Red or black)
        private readonly CardColor _color;
        // Value of the card. A (1) - K (13)
        private readonly int _value;
        // Sprite of the card
        private readonly Sprite _sprite;
        // Sorting Order of the card
        private int _sortingOrder;
        // The card is visible
        private bool _visible;

        #region Constructor
        
        public Card(CardType type, int value, Sprite sprite, bool isVisible) {
            _type = type;
            _value = value;
            _sprite = sprite;
            _visible = isVisible;

            _sortingOrder = 0;

            if (_type == CardType.Diamonds || _type == CardType.Hearts)
                _color = CardColor.Red;
            else
                _color = CardColor.Black;
        }
        
        #endregion

        #region Getters

        public CardType GetCardType() {
            return _type;
        }
        
        public int GetCardValue() {
            return _value;
        }

        public Sprite GetCardSprite() {
            return _sprite;
        }

        public int GetSortingOrder() {
            return _sortingOrder;
        }

        public bool IsVisible() {
            return _visible;
        }

        /// <summary>
        /// Gets the color of the card
        /// </summary>
        /// <returns>The color of the card</returns>
        private CardColor GetCardColor() {
            return _color;
        }
        
        #endregion

        #region Setters

        public void SetSortingOrder(int sortingOrder) {
            _sortingOrder = sortingOrder;
        }
        
        public void SetVisible(bool visible) {
            _visible = visible;
        }

        #endregion
        
        #region Override Methods

        public override string ToString() {
                     string sOut = _type switch {
                         CardType.Diamonds => "Diamond",
                         CardType.Hearts => "Heart",
                         CardType.Spades => "Spade",
                         CardType.Clubs => "Club",
                         _ => "ERROR"
                     };
         
                     sOut += _value switch {
                         1 => "_A",
                         11 => "_J",
                         12 => "_Q",
                         13 => "_K",
                         _ => "_" + _value
                     };
         
                     return sOut;
                 }

        #endregion

        #region Methods

        public bool CanPlace(Card card) {
            return _value == card.GetCardValue() - 1
                && _color != card.GetCardColor();
        }

        #endregion

    }
}
