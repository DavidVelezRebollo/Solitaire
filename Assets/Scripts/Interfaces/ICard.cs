using DVR.Classes;

namespace DVR.Interfaces {
    public interface ICard {
        public bool IsVisible();

        public bool CanPlace(Card card);
    }
}
