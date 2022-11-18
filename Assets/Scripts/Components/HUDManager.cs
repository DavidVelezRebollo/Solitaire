using DVR.Classes;
using TMPro;
using UnityEngine;

namespace DVR.Components
{
    public class HUDManager : MonoBehaviour {
        public TextMeshProUGUI MovementsText;
        public TextMeshProUGUI TimerText;
        
        private int _movesNumber;
        private Timer _timer;

        private void Start() {
            MovementsText.text = "Total Moves: 0";
            _timer = new Timer();

            EventManager.Instance.OnCardMoved += IncreaseMoves;
            EventManager.Instance.OnCardDraw += IncreaseMoves;
        }

        private void Update() {
            TimerText.text = _timer.HandleTimer();
        }

        private void IncreaseMoves() {
            _movesNumber++;
            MovementsText.text = "Total Moves: " + _movesNumber;
        }

        public void OnUndoButtonClick() {
            GameManager.Instance.Commands.UndoCommand();
        }
    }
}
