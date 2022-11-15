using UnityEngine;
using DVR.Classes;
using UnityEngine.Assertions;

namespace DVR.Components {
    public class CardComponent : MonoBehaviour {
        [SerializeField] private SpriteRenderer SpriteRenderer;
        [SerializeField] private BoxCollider2D Collider;
        [SerializeField] private Sprite ReverseCard;
        
        private Card _card;
        private CardPile _currentPile;
        private Vector3 _cardPosition;
        private bool _moving;

        private const float _CARD_SPEED = 0.3f;

        private void Update() {
            if (transform.position == _cardPosition) _moving = false;
            if (!_card.IsVisible() || _moving) {
                Collider.enabled = false;
                
                if(_moving)
                    transform.position = Vector3.MoveTowards(transform.position, _cardPosition, _CARD_SPEED);
                
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

        public void SetPile(CardPile newPile) {
            _currentPile = newPile;
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

        private void ChangePile(CardPile newPile) {
            newPile.AddCard(_currentPile.CardStack.GetCard(), _currentPile.CardStack.GetCardGameObject());
            _currentPile.CardStack.RemoveCard();
            _currentPile = newPile;
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
                Card lastCard = piles[i].CardStack.GetCard();
                GameObject lastCardGo = piles[i].CardStack.GetCardGameObject();

                if (_card.CanPlace(lastCard)) {
                    _cardPosition = lastCardGo.transform.position - new Vector3(0, 0.7f, 0);
                    
                    if (_currentPile.CardStack.HasCards()) {
                        int previousCard = _currentPile.CardStack.CardCount() - 2;
                        
                        if(!_currentPile.CardStack.GetCard(previousCard).IsVisible())
                            _currentPile.GetCardComponent(previousCard).Flip();
                    }
                    
                    ChangePile(piles[i]);
                    
                    
                    _moving = true;
                    found = true;
                }

                i++;
            }
        }
    }
}

