using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DVR.Classes;

namespace DVR.Components {
    public class Deck : MonoBehaviour {
        [Header("Unity Fields")]
        public GameObject CardPrefab;
        [Header("Cards Sprites")]
        public Sprite[] DiamondSprites;
        public Sprite[] HeartSprites;
        public Sprite[] SpadesSprites;
        public Sprite[] ClubsSprites;
        [Header("Card Columns")] 
        public GameObject[] Columns;
        
        private readonly List<Card> _deckCards = new List<Card>();

        private const int _CARD_NUMBER = 52;

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
                Card card = new Card(type, i % 13 + 1, sprite, false);
                
                _deckCards.Add(card);
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
                
                
                (_deckCards[a], _deckCards[b]) = (_deckCards[b], _deckCards[a]);
            }
        }

        private IEnumerator DealCards() {
            int columnNumber = 0, cardsNumber = 1;
            float yOffset = 0;

            for (int i = 0; i < Columns.Length; i++) {
                for (int j = 0; j < cardsNumber; j++) {
                    int lastCard = _deckCards.Count - 1;
                    CardComponent card = CardPrefab.GetComponent<CardComponent>();

                    card.SetCard(_deckCards[lastCard]);
                    _deckCards.Remove(_deckCards[lastCard]);

                    if (columnNumber == 0 || j + 1 == cardsNumber) {
                        card.Flip();
                    }

                    GameObject cardGo = Instantiate(CardPrefab,
                        Columns[columnNumber].transform.position + new Vector3(0, yOffset, 0), 
                        Quaternion.identity);
                    cardGo.transform.parent = Columns[columnNumber].transform;

                    yOffset -= 0.7f;

                    yield return new WaitForSeconds(0.2f);
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
    }
}
