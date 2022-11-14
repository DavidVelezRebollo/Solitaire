using DVR.Interfaces;
using DVR.Shared;
using UnityEngine;

namespace DVR.Classes {
    public class Card : ICard {
        //Type of the card (Heart, Diamond, Spade or Club)
        private readonly CardType _type;
        // Color of the card (Red or black)
        private readonly CardColor _color;
        // Value of the card. A (1) - K (13)
        private readonly int _value;
        // Sprite of the card
        private readonly Sprite _sprite;
        // The card is visible
        private bool _visible;

        #region Constructor

        public Card(CardType type, int value, Sprite sprite, bool isVisible) {
            _type = type;
            _value = value;
            _sprite = sprite;
            _visible = isVisible;

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

        public CardColor GetCardColor() {
            return _color;
        }

        public int GetCardValue() {
            return _value;
        }

        public Sprite GetCardSprite() {
            return _sprite;
        }
        
        public bool IsVisible() {
            return _visible;
        }

        #endregion

        #region Setters

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
