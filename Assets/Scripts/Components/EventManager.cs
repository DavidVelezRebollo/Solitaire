using System;
using UnityEngine;

namespace DVR.Components
{
    public class EventManager : MonoBehaviour
    {
        #region Singleton

        public static EventManager Instance;

        private void Awake() {
            if (Instance != null) return;

            Instance = this;
        }

        #endregion
        
        public event Action OnCardMoved;
        public event Action OnCardDraw;

        #region Events Invokes

        public void CardMoved() {
            OnCardMoved?.Invoke();
        }

        public void CardDraw() {
            OnCardDraw?.Invoke();
        }

        #endregion
    }
}
