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

        private int _maxSortingOrder;

        #region Unity Events

        private void Start() {
            Deck.Initialize();
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
        
        /// <summary>
        /// Decrease the maximum sorting order by 1
        /// </summary>
        public void DecreaseMaxSortingOrder() {
            _maxSortingOrder--;
        }

        #endregion
    }
}
