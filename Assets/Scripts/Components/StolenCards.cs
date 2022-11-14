using System.Collections.Generic;
using DVR.Classes;
using UnityEngine;

namespace DVR.Components
{
    public class StolenCards : MonoBehaviour {
        public readonly CardStack Cards = new CardStack();
        private readonly List<GameObject> _cardsGameObjects = new List<GameObject>();

        public void AddCardGo(GameObject cardGo) {
            _cardsGameObjects.Add(cardGo);
        }

        public void RemoveCardGo() {
            Destroy(_cardsGameObjects[_cardsGameObjects.Count - 1]);
            _cardsGameObjects.RemoveAt(_cardsGameObjects.Count - 1);
        }
    }
}
