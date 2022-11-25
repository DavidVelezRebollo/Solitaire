using System;
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
        // Time clicking the card
        private float _clickDelta;
        // Determines if the card is moving
        private bool _moving;
        // Determines if the card is being dragged
        private bool _dragged;
        // Determines if the card can be clicked
        private bool _canClick;
        // Determines if the collider is enabled
        private bool _colEnabled = true;
        // Speed at which the card moves
        private const float _CARD_SPEED = 35f;
        // Time which have to pass to drag the card
        private const float _CLICKING_TIME = 0.1f;

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
            if (_dragged) {
                _collider.enabled = false;
                return;
            }

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

        private void OnMouseDown() {
            _clickDelta = Time.time;
        }

        private void OnMouseDrag() {
            if (!(Time.time - _clickDelta > _CLICKING_TIME)) return;
            
            _dragged = true;
            _moving = true;
            transform.position = GameManager.Instance.GetMousePosition();
        }

        private void OnMouseUp() {
            if (Time.time - _clickDelta <= _CLICKING_TIME) {
                OnClick();
                return;
            }
            
            OnDrag();
            _dragged = false;
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
        /// Handles when a card is clicked without being dragged.
        /// </summary>
        private void OnClick() {
            if (_moving) return;
            
            int i = 0;
            bool found = false;

            PileComponent[] piles = _deck.GetPiles();
            FoundationComponent[] foundations = _deck.GetFoundations();
            
            // LOOKS IF THE CARD CAN BE PUT INSIDE THE FOUNDATIONS
            while (i < foundations.Length && !found) {
                if (CheckValidFoundation(foundations[i])) {
                    _moving = true;
                    found = true;
                }
                
                i++;
            }

            // If it was put, return
            if (found) return;

            i = 0;
            
            // LOOK IF THE CARD CAN BE PUT INSIDE THE PILES
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

        /// <summary>
        /// Handles when the card is dragged by the user
        /// </summary>
        private void OnDrag() {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);

            if (!hit) return;

            #region Card Hit

            if(hit.collider.CompareTag("Card")) {
                CardComponent card = hit.collider.GetComponent<CardComponent>();
                
                if (_card.CanPlace(card._card)) {
                    _cardPosition = card._currentPile.GetCardComponent().transform.position - new Vector3(0, 0.7f, 0);
                    ChangePile(card._currentPile, true);
                    
                    return;
                }
            }
            
            #endregion

            #region Pile Hit

            if (hit.collider.CompareTag("Pile")) {
                PileComponent pile = hit.collider.GetComponent<PileComponent>();
                
                if (pile.GetStack().HasCards() || _card.GetCardValue() != 13) return;

                _cardPosition = pile.transform.position; 
                ChangePile(pile, false);

                return;
            }
            
            #endregion

            #region Foundation Hit

            if (!hit.collider.CompareTag("Foundation")) return;
            FoundationComponent foundation = hit.collider.GetComponent<FoundationComponent>();

            CheckValidFoundation(foundation);
            
            #endregion
            
        }

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
        
        private bool CheckValidFoundation(FoundationComponent foundation) {
            bool valid = false;
            
            if (foundation.GetFoundationType() == _card.GetCardType() && foundation.CanPlace(_card) && !_child) {
                _cardPosition = foundation.transform.position;

                if (foundation.GetStack().HasCards()) {
                    foundation.GetCardComponent().DisableCollider();
                }
                    
                ChangePile(foundation, false);

                valid = true;
            }

            return valid;
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

