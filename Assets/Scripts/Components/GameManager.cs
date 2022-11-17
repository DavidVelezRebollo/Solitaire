using UnityEngine;

namespace DVR.Components {
    public class GameManager : MonoBehaviour {
        #region Singleton

        public static GameManager Instance;
        
        private void Awake() {
            if (Instance != null) return;

            Instance = this;
        }

        #endregion

        [Tooltip("Deck of the game")]
        public Deck Deck;
        [Tooltip("Card piles of the game")]
        public CardPile[] Piles;
        [Tooltip("Foundations of the game")]
        public Foundation[] Foundations;

        // Max sorting order among the card piles
        private int _maxSortingOrder;

        #region Unity Events

        private void Start() {
            Deck.Initialize();

            EventManager.Instance.OnCardMoved += CardMoved;
        }

        #endregion

        #region Getters

        /// <summary>
        /// Gets the maximum sorting order on the table
        /// </summary>
        /// <returns>The maximum sorting order on the table</returns>
        public int GetMaxSortingOrder() {
            return _maxSortingOrder;
        }

        #endregion

        #region Setters

        /// <summary>
        /// Increase the maximum sorting order by 1
        /// </summary>
        public void IncreaseMaxSortingOrder() {
            _maxSortingOrder++;
        }

        #endregion

        #region Event Methods

        private void CardMoved() {
            foreach (CardPile pile in Piles) {
                int maxSorting = pile.GetStack().GetMaxSortingOrder();

                if (_maxSortingOrder < maxSorting)
                    _maxSortingOrder = maxSorting;
            }
        }

        #endregion
    }
}
