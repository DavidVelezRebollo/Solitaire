using DVR.Interfaces.Commands;
using UnityEngine;

namespace DVR.Classes.Commands
{
    public class MoveCard : ICommand {
        public MoveCard() {
            
        }
        
        public void Execute() {
            // TODO - Move the card to the new position
        }

        public void Undo() {
            // TODO - Undo card to the previous position
        }
    }
}
