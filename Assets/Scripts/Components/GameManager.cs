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

        #region Unity Events

        private void Start() {
            Deck.Initialize();
        }

        #endregion
    }
}
