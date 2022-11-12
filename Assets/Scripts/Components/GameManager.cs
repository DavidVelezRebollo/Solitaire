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

        public Deck Deck;
        
        private void Start() {
            Deck.Initialize();
        }
    }
}
