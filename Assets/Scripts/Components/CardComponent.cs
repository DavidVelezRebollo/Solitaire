using UnityEngine;
using DVR.Classes;

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
        
        public GameObject CreateCard(Transform parent, Vector3 position) {
            GameObject cardGo = Instantiate(gameObject, position, Quaternion.identity);
            cardGo.transform.parent = parent;
            cardGo.name = _card.ToString();

            return cardGo;
        }

        private void OnMouseDown() {
            Debug.Log("Card " + _card + " clicked");
        }
    }
}

