using DVR.Interfaces.Commands;

namespace DVR.Classes
{
    public class Draw : ICommand
    {
        public void Execute() {
            // TODO - Draw a card and create it. Or fill again the deck
        }

        public void Undo() {
            // TODO - Put back a card inside the deck. Or empty the deck.
        }
    }
}
