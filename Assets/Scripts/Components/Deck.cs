using System.Collections;
using UnityEngine;
using DVR.Classes;
using DVR.Shared;
using Random = UnityEngine.Random;

namespace DVR.Components {
    public class Deck : MonoBehaviour {
        [Header("Unity Fields")]
        public GameObject CardPrefab;
        public StolenCards StolenCards;
        [Header("Cards Sprites")]
        public Sprite[] DiamondSprites;
        public Sprite[] HeartSprites;
        public Sprite[] SpadesSprites;
        public Sprite[] ClubsSprites;
        [Header("Card Columns")] 
        public CardPile[] Columns;

        private readonly CardStack _deckCards = new CardStack();
        private const int _CARD_NUMBER = 52;
        private SpriteRenderer _spriteRenderer;

        private void Start() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update() {
            _spriteRenderer.color = !_deckCards.HasCards() ? 
                new Color(0f, 0f, 0f, 0.79f) : new Color(1f, 1f, 1f, 1f);
        }

        public void Initialize() {
            InitializeCards();
            ShuffleDeck();
            StartCoroutine(DealCards());
        }
        
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

        private IEnumerator DealCards() {
            int columnNumber = 0, cardsNumber = 1;
            float yOffset = 0;

            for (int i = 0; i < Columns.Length; i++) {
                for (int j = 0; j < cardsNumber; j++) {
                    // VARIABLES
                    CardComponent instantiateCard = CardPrefab.GetComponent<CardComponent>();   
                    Card lastCard = _deckCards.GetCard();
                    Vector3 position = Columns[columnNumber].transform.position + new Vector3(0, yOffset, 0);
                    Transform parent = Columns[columnNumber].transform;

                    // CREATION OF THE CARD
                    instantiateCard.SetCard(lastCard);
                    GameObject cardGo = instantiateCard.CreateCard(parent, position);

                    // CARD INITIALIZATION
                    CardComponent card = cardGo.GetComponent<CardComponent>();
                    card.SetCard(lastCard);
                    Columns[columnNumber].AddCard(lastCard, cardGo);
                    
                    if (columnNumber != 0 && j + 1 != cardsNumber) {
                        card.Flip();
                    }
                    
                    _deckCards.RemoveCard();
                    
                    yOffset -= 0.7f;

                    yield return new WaitForSeconds(0.05f);
                }

                columnNumber++;
                cardsNumber++;
                yOffset = 0;
            }
        }

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

        private void HandleMouseClick() {
            if (!_deckCards.HasCards()) {
                int cardsStolen = StolenCards.Cards.CardCount();
                
                for (int i = 0; i < cardsStolen; i++) {
                    _deckCards.AddCard(StolenCards.Cards.GetCard());
                    StolenCards.RemoveCardGo();
                    StolenCards.Cards.RemoveCard();
                }
                
                return;
            }
            
            CardComponent instantiateCard = CardPrefab.GetComponent<CardComponent>();
            Transform parent = StolenCards.transform;
            StolenCards.Cards.AddCard(_deckCards.GetCard());
            
            instantiateCard.SetCard(_deckCards.GetCard());
            GameObject cardGo = instantiateCard.CreateCard(parent, parent.position);
            StolenCards.AddCardGo(cardGo);
            
            CardComponent card = cardGo.GetComponent<CardComponent>();
            card.SetCard(_deckCards.GetCard());
            _deckCards.RemoveCard();
            
        }

        private void OnMouseDown() {
            HandleMouseClick();
        }
    }
}
