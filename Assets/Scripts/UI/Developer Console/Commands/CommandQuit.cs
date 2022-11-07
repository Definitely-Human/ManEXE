using UnityEngine;

namespace ManExe.UI.Developer_Console.Commands
{

    public class CommandQuit : ConsoleCommand
    {
    	

        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }
        
        public CommandQuit()
        {
            Name = "Quit";
            Command = "quit";
            Description = "Quits the application";
            Help = "Use this command to force quit the application.";
            
        }
        public override void RunCommand(string[] argsArr)
        {
            
            if (Application.isEditor)
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
            }
            else
            {
                Application.Quit();
            }
        }

        
    }
}
