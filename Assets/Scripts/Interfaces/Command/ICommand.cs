namespace DVR.Interfaces.Commands
{
    public interface ICommand {
        /// <summary>
        /// Executes the command
        /// </summary>
        public void Execute();

        /// <summary>
        /// Undo the command
        /// </summary>
        public void Undo();
    }
}
