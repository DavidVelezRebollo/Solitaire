using DVR.Classes.Commands;
using UnityEngine;

namespace DVR.Components.Core {
    public class GameManager : MonoBehaviour {
        #region Singleton

        public static GameManager Instance;
        
        private void Awake() {
            if (Instance != null) return;

            Instance = this;
        }

        #endregion
        
        // Commands of the game
        public CommandManager Commands { get; private set; }
        // Main Camera of the game
        [SerializeField] private Camera MainCamera;
        // Max sorting order among the card piles
        private int _maxSortingOrder;

        #region Unity Events

        private void Start() {
            Commands = new CommandManager();
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

        public Vector3 GetMousePosition() {
            Vector3 temp = MainCamera.ScreenToWorldPoint(Input.mousePosition);

            return new Vector3(temp.x, temp.y);
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
        /// Sets the maximum sorting order
        /// </summary>
        /// <param name="sortingOrder">The new maximum sorting order</param>
        public void SetMaxSortingOrder(int sortingOrder) {
            _maxSortingOrder = sortingOrder;
        }

        #endregion
    }
}
