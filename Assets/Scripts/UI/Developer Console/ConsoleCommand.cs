namespace ManExe.UI.Developer_Console
{
    public abstract class ConsoleCommand
    {
        public abstract string Name { get; protected set; }
        public abstract string Command { get; protected set; }
        public abstract string Description { get; protected set; }
        public abstract string Help { get; protected set; }
        
        private DeveloperConsole Console = DeveloperConsole.Instance;
        

        public abstract void RunCommand(string[] args);
    }
}