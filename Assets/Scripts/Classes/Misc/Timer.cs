using System.Text;
using UnityEngine;

namespace DVR.Classes.Miscellaneous
{
    public class Timer {
        private float _timer;
        private float _secondsCount;
        private float _minutesCount;

        public string HandleTimer() {
            StringBuilder builder = new StringBuilder();

            _timer += Time.deltaTime;
            _minutesCount = Mathf.FloorToInt(_timer / 60);
            _secondsCount = Mathf.FloorToInt(_timer % 60);

            builder.Append("Time: ");
            builder.Append($"{_minutesCount:00}:{_secondsCount:00}");

            return builder.ToString();
        }
    }
}
