using UnityEngine;
using DVR.Classes;
using UnityEngine.Assertions;

namespace DVR.Components {
    public class CardComponent : MonoBehaviour {
        public SpriteRenderer SpriteRenderer;
        public BoxCollider2D Collider;
        public Sprite ReverseCard;
        
        private Card _card;
        private Vector3 _cardPosition;
        private bool _moving;

        private void Update() {
            if (transform.position == _cardPosition) _moving = false;
            if (!_card.IsVisible() || _moving) {
                Collider.enabled = false;
                
                if(_moving)
                    transform.position = Vector3.MoveTowards(transform.position, _cardPosition, 0.5f);
                
                return;
            }
            
            Collider.enabled = true;
        }

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

        private void OnMouseOver() {
            if (!Input.GetMouseButtonDown(1) || _moving) return;

            Foundation[] foundations = GameManager.Instance.Foundations;
            Assert.IsNotNull("There is not foundations");
            
            int i = 0;
            bool found = false;

            while (i < foundations.Length && !found) {
                if (foundations[i].FoundationType == _card.GetCardType() && foundations[i].CanPlace(_card)) {
                    _cardPosition = foundations[i].transform.position;
                    foundations[i].AddCard(_card, gameObject);

                    _moving = true;
                    found = true;
                }
                
                i++;
            }
        }

        private void OnMouseDown() {
            if (_moving) return;

            CardPile[] piles = GameManager.Instance.Piles;
            Assert.IsNotNull("There is not piles");

            int i = 0;
            bool found = false;

            while (i < piles.Length && !found) {
                Card lastCard = piles[i].GetLastCard();
                GameObject lastCardGo = piles[i].GetLastCardGo();

                if (_card.CanPlace(lastCard)) {
                    _cardPosition = lastCardGo.transform.position - new Vector3(0, 0.7f, 0);
                    piles[i].RemoveCard();
                    
                    if (piles[i].GetCardStack().HasCards()) {
                        
                    }
                    
                    _moving = true;
                    found = true;
                }

                i++;
            }
        }
    }
}

