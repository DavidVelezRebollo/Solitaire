using System;
using System.Collections.Generic;
using UnityEngine;
using DVR.Classes;

namespace DVR.Components {
    public class CardComponent : MonoBehaviour {
        [Tooltip("Reverse of the card sprite")]
        [SerializeField] private Sprite ReverseCard;
        
        //Sprite Renderer component attached to the card
        private SpriteRenderer _spriteRenderer;
        //Box Collider component attached to the card
        private BoxCollider2D _collider;
        // Card contained in the component
        private Card _card;
        // Pile where the card is located
        private CardPile _currentPile;
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
        private const float _CARD_SPEED = 0.3f;

        #region Unity Events

        private void OnEnable() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();
        }

        private void Update() {
            _spriteRenderer.sortingOrder = !_moving ? _card.GetSortingOrder() : GameManager.Instance.GetMaxSortingOrder() + 1;

            if (transform.position == _cardPosition) _moving = false;
            if (!_card.IsVisible() || _moving) {
                _canClick = false;
                
                if(_moving)
                    transform.position = Vector3.MoveTowards(transform.position, _cardPosition, _CARD_SPEED);
                
            }
            else _canClick = true;
            
            
            _collider.enabled = _canClick && _colEnabled;
        }
        
        private void OnMouseOver() {
            if (!Input.GetMouseButtonDown(1) || _moving) return;

            Foundation[] foundations = GameManager.Instance.Foundations;

            int i = 0;
            bool found = false;

            while (i < foundations.Length && !found) {
                if (foundations[i].GetFoundationType() == _card.GetCardType() && foundations[i].CanPlace(_card)) {
                    _cardPosition = foundations[i].transform.position;

                    if (foundations[i].GetStack().HasCards()) {
                        foundations[i].GetCardComponent().DisableCollider();
                    }
                    
                    ChangePile(foundations[i]);
                    
                    _moving = true;
                    found = true;
                }
                
                i++;
            }
        }

        private void OnMouseDown() {
            if (_moving) return;
            if (_child != null) return;

            CardPile[] piles = GameManager.Instance.Piles;

            int i = 0;
            bool found = false;

            while (i < piles.Length && !found) {
                if(piles[i].GetStack().HasCards()) {
                    if (_card.CanPlace(piles[i].GetStack().GetCard())) {
                        _cardPosition = piles[i].GetStack().GetCardGameObject().transform.position - 
                                        new Vector3(0, 0.7f, 0);

                        ChangePile(piles[i]);

                        _moving = true;
                        found = true;
                    }
                }
                else {
                    if (_card.GetCardValue() == 13) {
                        _cardPosition = piles[i].transform.position;

                        ChangePile(piles[i]);

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

        /// <summary>
        /// Gets the card which this card is attached to, if it exists
        /// </summary>
        /// <returns>The parent card. Null if it don't have a parent</returns>
        public CardComponent GetParent() {
            return _parent;
        } 

        /// <summary>
        /// Gets the child attached to the card, if it exists
        /// </summary>
        /// <returns>The card attached to this card. Null if it doesn't exist</returns>
        public CardComponent GetChild() {
            return _child;
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
        public void SetPile(CardPile newPile) {
            _currentPile = newPile;
        }

        /// <summary>
        /// Sets the card parent
        /// </summary>
        /// <param name="parent">The new parent card</param>
        public void SetParent(CardComponent parent) {
            _parent = parent;
        }

        /// <summary>
        /// Sets a child card
        /// </summary>
        /// <param name="child">The new child card</param>
        public void SetChild(CardComponent child) {
            _child = child;
        }
        
        /// <summary>
        /// Disable the box collider of the card
        /// </summary>
        public void DisableCollider() {
            _colEnabled = false;
        }

        /// <summary>
        /// Enables the collider of the card
        /// </summary>
        public void EnableCollider() {
            _colEnabled = true;
        }
        
        #endregion

        #region Methods
        
        /// <summary>
        /// Change between the pile of the card and a pile objective
        /// </summary>
        /// <param name="newPile">The pile to be replaced</param>
        private void ChangePile(CardPile newPile) {
            CardPile lastPile = _currentPile;
            CardComponent parentCard = null;

            if (newPile.GetStack().HasCards()) parentCard = newPile.GetCardComponent();

            // ADDING THE CARD TO THE NEW PILE AND REMOVING FROM THE LAST PILE
            newPile.AddCard(_card, gameObject);
            _currentPile.GetStack().RemoveCard();
            _currentPile = newPile;

                // ATTACH THE CARD
            if(parentCard != null) AttachCard(parentCard);

            // FLIP THE CARD, IF THERE IS A CARD, OF THE LAST PILE
            if (lastPile.GetStack().HasCards()) {
                if (lastPile.GetType() != typeof(Foundation)) {
                    if (!lastPile.GetStack().GetCard().IsVisible())
                        lastPile.GetCardComponent().Flip();
                }
                else {
                    lastPile.GetCardComponent().EnableCollider();
                }
            }
            
            EventManager.Instance.CardMoved();

            // INCREASE AND DECREASE OF THE SORTING ORDER
            lastPile.GetStack().DecreaseMaxSortingOrder();
            _currentPile.GetStack().IncreaseMaxSortingOrder();
            _card.SetSortingOrder(_currentPile.GetStack().GetMaxSortingOrder());

            // DEBUG - TO DELETE
            Debug.Log(lastPile.GetStack().ToString());
            Debug.Log(_currentPile.GetStack().ToString());
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

        /// <summary>
        /// Attaches this card to a card parent
        /// </summary>
        /// <param name="parent">The card to attach this card</param>
        public void AttachCard(CardComponent parent) {
            parent.SetChild(this);
            SetParent(parent);
        }

        #endregion
    }
}

