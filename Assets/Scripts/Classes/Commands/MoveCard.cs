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
        
        public MoveCard(Card card, GameObject cardGameObject, CardStack newStack, CardStack previousStack) {
            _cardToMove = card;
            _cardGameObject = cardGameObject;
            _stackToMove = newStack;
            _previousStack = previousStack;
        }
        
        public void Execute() {
            Debug.Log("Card Moved");
            _stackToMove.AddCard(_cardToMove, _cardGameObject);
            _previousStack.RemoveCard(_cardToMove, _cardGameObject);
            
            Debug.Log("New Stack: " +  _stackToMove);
            Debug.Log("Previous Stack: " + _previousStack);
        }

        public void Undo() {
            Debug.Log("Undo card movement");
        }
    }
}
