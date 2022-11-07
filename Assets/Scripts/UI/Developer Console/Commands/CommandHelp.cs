using System;
using UnityEngine;

namespace ManExe.UI.Developer_Console.Commands
{

    public class CommandHelp : ConsoleCommand
    {


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
        public override void RunCommand(string[] argsArr)
        {
            
            DevCon.AddMessageToConsole("" +
                                        "Type name of the command and arguments to execute it." +
                                        "");
        }
        
        public static void HelpCommand(ConsoleCommand command)
        {
            DevCon.AddMessageToConsole("==================================");
            DevCon.AddMessageToConsole(command.Description);
            DevCon.AddMessageToConsole("----------------------------------");
            DevCon.AddMessageToConsole(command.Help);
            DevCon.AddMessageToConsole("==================================");
        }
    }
}