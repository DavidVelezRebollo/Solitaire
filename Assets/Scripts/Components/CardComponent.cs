using UnityEngine;
using DVR.Classes;
using UnityEngine.Assertions;

namespace DVR.Components {
    public class CardComponent : MonoBehaviour {
        public SpriteRenderer SpriteRenderer;
        public Sprite ReverseCard;

        private Card _card;

        public void Flip() {
            _card.SetVisible(!_card.IsVisible());
            SpriteRenderer.sprite = _card.IsVisible() ?  _card.GetCardSprite() : ReverseCard;
        }

        public void SetCard(Card card) {
            _card = card;
            SpriteRenderer.sprite = _card.IsVisible() ?  card.GetCardSprite() : ReverseCard;
        }

        public Card GetCard() {
            return _card;
        }
    }
}

