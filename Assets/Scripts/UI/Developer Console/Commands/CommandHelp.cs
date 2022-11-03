using System;
using UnityEngine;

namespace ManExe.UI.Developer_Console.Commands
{

    public class CommandHelp : ConsoleCommand
    {
        private static readonly DeveloperConsole _developerConsole = DeveloperConsole.Instance;


        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }
        
        public CommandHelp()
        {
            Name = "Help";
            Command = "help";
            Description = "Shows help.";
            Help = "Use this command to seek help.";
            
        }
        public override void RunCommand(string[] args)
        {
            
            _developerConsole.AddMessageToConsole("" +
                                                  "Type name of the command and arguments to execute it." +
                                                  "");
        }
        
        public static void HelpCommand(ConsoleCommand command)
        {
            _developerConsole.AddMessageToConsole("==================================");
            _developerConsole.AddMessageToConsole(command.Description);
            _developerConsole.AddMessageToConsole("----------------------------------");
            _developerConsole.AddMessageToConsole(command.Help);
            _developerConsole.AddMessageToConsole("==================================");
        }
    }
}