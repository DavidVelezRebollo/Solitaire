using UnityEngine;

namespace DVR.Interfaces {
    public interface ICard {
        public abstract bool IsFlipped();
        public abstract void Flip();
    }
}
