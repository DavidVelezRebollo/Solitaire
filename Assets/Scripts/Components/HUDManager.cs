using System;
using TMPro;
using UnityEngine;

namespace DVR.Components
{
    public class HUDManager : MonoBehaviour {
        public TextMeshProUGUI MovementsText;
        
        private int _movesNumber;

        private void Start() {
            MovementsText.text = "Total Moves: 0";
            
            EventManager.Instance.OnCardMoved += IncreaseMoves;
            EventManager.Instance.OnCardDraw += IncreaseMoves;
        }

        private void IncreaseMoves() {
            _movesNumber++;
            MovementsText.text = "Total Moves: " + _movesNumber;
        }
    }
}
