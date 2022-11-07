using UnityEngine;

namespace ManExe.UI.Developer_Console.Commands
{

    public class CommandClear : ConsoleCommand
    {
    	

        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }
        
        public CommandClear()
        {
            Name = "Clear";
            Command = "cls";
            Description = "Clears the console.";
            Help = "Use this command to clear the console." +
                   "No arguments.";
            
        }
        public override void RunCommand(string[] argsArr)
        {
            DevCon.ClearConsole();
        }
    }
}