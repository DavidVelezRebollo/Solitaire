namespace DVR.Interfaces
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
