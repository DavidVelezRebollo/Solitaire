using UnityEngine;
using DVR.Classes.Cards;
using DVR.Components.Core;

namespace DVR.Components.Cards {
    public class CardComponent : MonoBehaviour {
        [Tooltip("Reverse of the card sprite")]
        [SerializeField] private Sprite ReverseCard;
        
        //Sprite Renderer component attached to the card
        private SpriteRenderer _spriteRenderer;
        //Box Collider component attached to the card
        private BoxCollider2D _collider;
        // Deck of the game containing the piles and the foundations
        private DeckComponent _deck;
        // Initial size of the collider
        private Vector2 _initialColliderSize;
        // Card contained in the component
        private Card _card;
        // Pile where the card is located
        private PileComponent _currentPile;
        // Previous pile of the card
        private PileComponent _previousPile;
        // Parent card
        private CardComponent _parent;
        // Child card
        private CardComponent _child;
        // Position of the card
        private Vector3 _cardPosition;
        // Determines if the card is moving
        private bool _moving;
        // Determines if the card can be clicked
        private bool _canClick;
        // Determines if the collider is enabled
        private bool _colEnabled = true;
        // Speed at which the card moves
        private const float _CARD_SPEED = 35f;

        #region Unity Events

        private void OnEnable() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();

            // Initial Collider size
            _initialColliderSize = _collider.size;
            
            // Getting the game deck
            _deck = GameObject.FindWithTag("Deck").GetComponent<DeckComponent>();
            
            // Moving method
            EventManager.Instance.OnCardMoved += CardMoved;
        }

        private void Update() {
            _spriteRenderer.sortingOrder = !_moving ? _card.GetSortingOrder() : GameManager.Instance.GetMaxSortingOrder() + 1;

            if (transform.position == _cardPosition) {
                _moving = false;
            }
            if (!_card.IsVisible() || _moving) {
                _canClick = false;
                
                if(_moving)
                    transform.position = Vector3.MoveTowards(
                        transform.position, _cardPosition, _CARD_SPEED * Time.deltaTime);
                
            }
            else _canClick = true;

            if (_child != null) {
                _collider.size = new Vector2(2.1f, 0.7f);
                _collider.offset = new Vector2(0f, 1.15f);
            }
            else {
                _collider.size = _initialColliderSize;
                _collider.offset = Vector2.zero;
            }
            
            _collider.enabled = _canClick && _colEnabled;
        }
        
        private void OnMouseOver() {
            if (!Input.GetMouseButtonDown(1) || _moving || _child != null) return;

            FoundationComponent[] foundations = _deck.GetFoundations();

            int i = 0;
            bool found = false;

            while (i < foundations.Length && !found) {
                if (foundations[i].GetFoundationType() == _card.GetCardType() && foundations[i].CanPlace(_card)) {
                    _cardPosition = foundations[i].transform.position;

                    if (foundations[i].GetStack().HasCards()) {
                        foundations[i].GetCardComponent().DisableCollider();
                    }
                    
                    ChangePile(foundations[i], false);

                    _moving = true;
                    found = true;
                }
                
                i++;
            }
        }

        private void OnMouseDown() {
            if (_moving) return;

            PileComponent[] piles = _deck.GetPiles();

            int i = 0;
            bool found = false;

            while (i < piles.Length && !found) {
                if(piles[i].GetStack().HasCards()) {
                    if (_card.CanPlace(piles[i].GetStack().GetCard())) {
                        _cardPosition = piles[i].GetStack().GetCardGameObject().transform.position - 
                                        new Vector3(0, 0.7f, 0);

                        ChangePile(piles[i], true);

                        _moving = true;
                        found = true;
                    }
                }
                else {
                    if (_card.GetCardValue() == 13) {
                        _cardPosition = piles[i].transform.position;

                        ChangePile(piles[i], false);

                        _moving = true;
                        found = true;
                    }
                }

                i++;
            }
        }
        
        #endregion

        #region Getters

        /// <summary>
        /// Gets the card contained inside the component
        /// </summary>
        /// <returns>The card contained inside the component</returns>
        public Card GetCard() {
            return _card;
        }

        #endregion

        #region Setters

        /// <summary>
        /// Sets the component's card
        /// </summary>
        /// <param name="card">The card to be set</param>
        public void SetCard(Card card) {
            _card = card;
            
            if(_spriteRenderer != null)
                _spriteRenderer.sprite = _card.IsVisible() ?  card.GetCardSprite() : ReverseCard;
        }

        /// <summary>
        /// Sets the pile of the card
        /// </summary>
        /// <param name="newPile">The pile to be set</param>
        public void SetPile(PileComponent newPile) {
            _currentPile = newPile;
        }
        
        public void SetPosition(Vector3 position) {
            _cardPosition = position;
        }
        
        /// <summary>
        /// Disable the box collider of the card
        /// </summary>
        public void DisableCollider() {
            _colEnabled = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Change between the pile of the card and a pile objective
        /// </summary>
        /// <param name="newPile">The pile to be replaced</param>
        /// <param name="attachCard">Checks if we want to attach the card transform at the new pile</param>
        private void ChangePile(PileComponent newPile, bool attachCard) {
            _previousPile = _currentPile;
            CardComponent parentCard = null;

            if (newPile.GetStack().HasCards()) parentCard = newPile.GetCardComponent();

            // ADDING THE CARD TO THE NEW PILE AND REMOVING FROM THE LAST PILE
            if(_child != null) {
                HandleAttachedCards(newPile, attachCard);
            }
            else {
                newPile.AddCard(_card, gameObject, attachCard);
                _currentPile.GetStack().RemoveCard();
                _currentPile = newPile;
            }

            // ATTACH THE CARD
            if(parentCard != null) AttachCard(parentCard);

            // FLIP THE CARD, IF THERE IS A CARD, OF THE LAST PILE
            HandleLastCard(_previousPile);
            
            // INCREASE AND DECREASE OF THE SORTING ORDER
            HandleSortingOrder(_previousPile);

            // EVENT INVOCATION
            EventManager.Instance.OnCardMovedInvoke();
        }

        /// <summary>
        /// Flips the card and changes between card face and card reverse.
        /// </summary>
        public void Flip() {
            _card.SetVisible(!_card.IsVisible());
            _spriteRenderer.sprite = _card.IsVisible() ?  _card.GetCardSprite() : ReverseCard;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Creates a card at a given position
        /// </summary>
        /// <param name="parent">Parent of the created card</param>
        /// <param name="position">Position where the card will be created</param>
        /// <returns>The Game Object of the new card created</returns>
        public GameObject CreateCard(Transform parent, Vector3 position) {
            GameObject cardGo = Instantiate(gameObject, position, Quaternion.identity);
            cardGo.transform.parent = parent;
            cardGo.name = _card.ToString();

            return cardGo;
        }

        #endregion

        #region Auxiliar Methods

        private static void HandleLastCard(PileComponent lastPile) {
            if (!lastPile.GetStack().HasCards()) return;
            
            if (lastPile.GetType() != typeof(FoundationComponent) && !lastPile.GetStack().GetCard().IsVisible()) 
                lastPile.GetCardComponent().Flip();
            else 
                lastPile.GetCardComponent()._colEnabled = true;
        }

        private void HandleAttachedCards(PileComponent newPile, bool attachCard) {
            newPile.AddCard(_card, gameObject, attachCard);
            _currentPile.GetStack().RemoveCard(_card, gameObject);
            _currentPile = newPile;
                
            CardComponent attachedCard = _child;
            while (attachedCard != null) {
                newPile.AddCard(attachedCard._card, attachedCard.gameObject, true);
                attachedCard._currentPile.GetStack().RemoveCard(attachedCard._card, attachedCard.gameObject);
                attachedCard._currentPile = newPile;
                    
                attachedCard = attachedCard._child;
            }
        }

        private void HandleSortingOrder(PileComponent lastPile) {
            #region No Cards Attached

            lastPile.GetStack().DecreaseMaxSortingOrder();
            _currentPile.GetStack().IncreaseMaxSortingOrder();
            _card.SetSortingOrder(_currentPile.GetStack().GetMaxSortingOrder());

            #endregion

            if (_child == null) return;

            #region Cards Attached

            CardComponent attachedCard = _child;
            while (attachedCard != null) {
                lastPile.GetStack().DecreaseMaxSortingOrder();
                attachedCard._currentPile.GetStack().IncreaseMaxSortingOrder();
                attachedCard._card.SetSortingOrder(attachedCard._currentPile.GetStack().GetMaxSortingOrder());
                
                attachedCard = attachedCard._child;
            }

            #endregion
        }
        
        private void AttachCard(CardComponent parent) {
            if (_parent != null)
                _parent._child = null;

            parent._child = this;
            _parent = parent;
        }

        #endregion

        #region Event Methods

        private void CardMoved() {
            PileComponent[] piles = _deck.GetPiles();
            GameManager manager = GameManager.Instance;

            int maxSorting = 0;

            foreach (PileComponent pile in piles)
                if (pile.GetStack().GetMaxSortingOrder() > maxSorting)
                    maxSorting = pile.GetStack().GetMaxSortingOrder();

            manager.SetMaxSortingOrder(maxSorting);
        }

        #endregion
    }
}

