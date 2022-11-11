namespace DVR.Classes {
    public class Card {
        private readonly CardType _type;
        private readonly CardColor _color;
        private readonly int _value;

        // The card is flipped
        private bool _flipped;

        public Card(CardType type, int value) {
            _type = type;
            _value = value;

            if (_type == CardType.Diamonds || _type == CardType.Hearts)
                _color = CardColor.Red;
            else
                _color = CardColor.Black;
        }

        public CardType GetCardType() {
            return _type;
        }

        public CardColor GetCardColor() {
            return _color;
        }

        public int GetCardValue() {
            return _value;
        }

        public bool IsFlipped() {
            return _flipped;
        }

        public void Flip() {
            _flipped = !_flipped;
        }

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
    }
}
