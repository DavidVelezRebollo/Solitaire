using DVR.Interfaces.Commands;
using DVR.Classes.Cards;
using UnityEngine;

namespace DVR.Classes.Commands
{
    public class MoveCard : ICommand {
        private readonly Card _cardToMove;
        private readonly GameObject _cardGameObject;
        private readonly CardStack _stackToMove;
        private readonly CardStack _previousStack;
        
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
