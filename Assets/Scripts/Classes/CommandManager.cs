using System.Collections.Generic;
using DVR.Interfaces;

namespace DVR.Classes
{
    public class CommandManager {
        private readonly List<ICommand> _commands = new List<ICommand>();
        private int _lastExecutedCommand = -1;

        public void ExecuteCommand(ICommand command) {
            command.Execute();

            if (_lastExecutedCommand < _commands.Count - 1) {
                for(int i = _commands.Count - 1; i > _lastExecutedCommand; i--)
                    _commands.RemoveAt(i);
            }
            
            _commands.Add(command);
            _lastExecutedCommand = _commands.Count - 1;
        }

        public void UndoCommand() {
            if (_lastExecutedCommand <= -1) return;

            ICommand lastCommand = _commands[_lastExecutedCommand];
            lastCommand.Undo();
            _lastExecutedCommand -= 1;
        }
    }
}
