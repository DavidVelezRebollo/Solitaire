using DVR.Classes.Cards;
using DVR.Components.Core;
using DVR.Shared;

using System.Collections;
using DG.Tweening;
using Random = UnityEngine.Random;

using UnityEngine;

namespace DVR.Components.Cards {
    public class DeckComponent : MonoBehaviour {
        [Header("Unity Fields")]
        [Tooltip("Prefab of the card")]
        [SerializeField] private GameObject CardPrefab;
        [Tooltip("Pile where the stolen cards will be created")]
        [SerializeField] private PileComponent StolenCards;
        [Tooltip("Card piles of the game")]
        [SerializeField] private PileComponent[] Piles;
        [Tooltip("Foundations of the game")]
        [SerializeField] private FoundationComponent[] Foundations;
        
        [Header("Cards Sprites")]
        [Tooltip("Sprites of the diamond cards")]
        [SerializeField] private Sprite[] DiamondSprites;
        [Tooltip("Sprites of the heart cards")]
        [SerializeField] private Sprite[] HeartSprites;
        [Tooltip("Sprites of the spade cards")]
        [SerializeField] private Sprite[] SpadesSprites;
        [Tooltip("Sprites of the club cards")]
        [SerializeField] private Sprite[] ClubsSprites;

        // Number of cards inside the deck
        private const int _CARD_NUMBER = 52;
        // Stack with the cards of the deck
        private readonly CardStack _deckCards = new CardStack();
        // Sprite Renderer component
        private SpriteRenderer _spriteRenderer;

        #region Unity Events

        private void OnEnable() {
            InitializeCards();
            ShuffleDeck();
            StartCoroutine(DealCards());
        }
        
        private void Start() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update() {
            _spriteRenderer.color = !_deckCards.HasCards() ? 
                new Color(0f, 0f, 0f, 0.79f) : new Color(1f, 1f, 1f, 1f);
        }
        
        private void OnMouseDown() {
            HandleMouseClick();
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Assign the card and add them to the deck
        /// </summary>
        private void InitializeCards() {
            CardType type = CardType.Diamonds;

            for (int i = 0; i < _CARD_NUMBER; i++) {
                if (i % 13 == 0 && i != 0)
                    type++;

                Sprite sprite = SelectCardSprite(type, i % 13);
                Card card = new Card(type, i % 13 + 1, sprite, true);
                
                _deckCards.AddCard(card);
            }
        }
        
        /// <summary>
        /// Shuffles the deck
        /// </summary>
        private void ShuffleDeck() {
            const int SHUFFLE_NUMBER = 1000;

            for (int i = 0; i < SHUFFLE_NUMBER; i++) {
                int a, b;

                do {
                    a = Random.Range(0, _CARD_NUMBER);
                    b = Random.Range(0, _CARD_NUMBER);
                } while (a == b);


                _deckCards.ChangeCards(a, b);
            }
        }

        /// <summary>
        /// Deal the cards to the table
        /// </summary>
        /// <returns>Number of seconds between card creation</returns>
        private IEnumerator DealCards() {
            int cardsNumber = 1;
            float yOffset = 0;
            
            for (int i = 0; i < Piles.Length; i++) {
                for (int j = 0; j < cardsNumber; j++) {
                    // VARIABLES
                    CardComponent instantiateCard = CardPrefab.GetComponent<CardComponent>();   
                    Card lastCard = _deckCards.GetCard();
                    Vector3 finalPosition = Piles[i].transform.position + new Vector3(0, yOffset, 0);
                    Transform parent = Piles[i].transform;

                    // CREATION OF THE CARD
                    lastCard.SetSortingOrder(j);
                    instantiateCard.SetCard(lastCard);
                    GameObject cardGo = instantiateCard.CreateCard(parent, transform.position);
                    cardGo.transform.DOMove(finalPosition, 0.1f);

                    // CARD INITIALIZATION
                    CardComponent card = cardGo.GetComponent<CardComponent>();
                    card.SetCard(lastCard);
                    card.SetPile(Piles[i]);
                    card.SetPosition(finalPosition);

                    // ADDING THE CARD
                    Piles[i].AddCard(card.GetCard(), cardGo, false);
                    if (i != 0 && j + 1 != cardsNumber) {
                        card.Flip();
                    }
                    
                    _deckCards.RemoveCard();
                    Piles[i].GetStack().IncreaseMaxSortingOrder();
                    
                    if(i > GameManager.Instance.GetMaxSortingOrder())
                        GameManager.Instance.IncreaseMaxSortingOrder();

                    yOffset -= 0.7f;
                    yield return new WaitForSeconds(0.1f);
                }
                
                cardsNumber++;
                yOffset = 0;
            }
        }
        
        /// <summary>
        /// Handles the click of the mouse
        /// </summary>
        private void HandleMouseClick() {
            EventManager.Instance.OnCardDrawInvoke();
            
            if (!_deckCards.HasCards()) {
                int cardsStolen = StolenCards.GetStack().CardCount();
                
                for (int i = 0; i < cardsStolen; i++) {
                    _deckCards.AddCard(StolenCards.GetStack().GetCard(), null);
                    StolenCards.GetStack().DecreaseMaxSortingOrder();
                    StolenCards.DestroyCard();
                }

                return;
            }
            
            CardComponent instantiateCard = CardPrefab.GetComponent<CardComponent>();
            Transform parent = StolenCards.transform;

            // CREATING THE CARD
            instantiateCard.SetCard(_deckCards.GetCard());
            GameObject cardGo = instantiateCard.CreateCard(parent, transform.position);
            cardGo.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
            cardGo.transform.DOMove(parent.position, 0.1f);
            cardGo.transform.DORotate(new Vector3(0, 0, 0), 0.1f);
            
            // INITIALIZING THE CARD
            CardComponent card = cardGo.GetComponent<CardComponent>();
            StolenCards.AddCard(_deckCards.GetCard(), cardGo, false);
            card.SetCard(_deckCards.GetCard());
            card.SetPile(StolenCards);
            card.SetPosition(StolenCards.transform.position);
            _deckCards.RemoveCard();
            
            // DISABLING COLLIDERS ON THE PREVIOUS STOLEN CARDS
            if (StolenCards.GetStack().HasCards() && StolenCards.GetStack().CardCount() >= 2)
                StolenCards.GetCardComponent(StolenCards.GetStack().CardCount() - 2).DisableCollider();
            
            // HANDLE SORTING ORDER
            StolenCards.GetStack().IncreaseMaxSortingOrder();
            card.GetCard().SetSortingOrder(StolenCards.GetStack().GetMaxSortingOrder());
        }
        
        #endregion

        #region Getters

        public FoundationComponent[] GetFoundations() {
            return Foundations;
        }

        public PileComponent[] GetPiles() {
            return Piles;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Selects a sprite depending on the values of a card
        /// </summary>
        /// <param name="type">Type of the card</param>
        /// <param name="value">Value of the card</param>
        /// <returns>The correct sprite. Null if there are a mistake</returns>
        private Sprite SelectCardSprite(CardType type, int value) {
            Sprite sprite = type switch {
                CardType.Diamonds => DiamondSprites[value],
                CardType.Hearts => HeartSprites[value],
                CardType.Spades => SpadesSprites[value],
                CardType.Clubs => ClubsSprites[value],
                _ => null
            };

            return sprite;
        }

        #endregion

    }
}
