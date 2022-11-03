using System;
using UnityEngine;
using System.Text.RegularExpressions;


namespace ManExe.UI.Developer_Console.Commands
{

    public class CommandCreate : ConsoleCommand
    {
    	

        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }
        
        public CommandCreate()
        {
            Name = "Create";
            Command = "create";
            Description = "Creates the instance of an object";
            Help = "Use this command to create given amount of objects of a given ID at a given position.\n" +
                   "Arguments:\n" +
                   " - Amount [-amt=1] - int, > 0, number of objects to be created,  default = 1 \n" +
                   " - ID [-id=1] - int, > 0, unique ID of the object to create , REQUIRED\n" +
                   " - Position [-x=1 -y=1 -z=1] - int, position where objets will be created, default = [0,0,0]";
            
            
            
        }
        public override void RunCommand(string[] args)
        {
            // Default values for arguments.
            int amt = 1;
            int id = 1; // If ID not provided throw error.
            int x = 0, y = 0, z = 0;
            
            
            for (int i = 0; i < args.Length; i++)
            {
                string argument = args[i];
                string[] argSplit = Regex.Split(argument, @"\="); // Split every argument in two parts at '=' sign.

                switch (argSplit[0])
                {
                    case "-x":
                        x = int.Parse(argSplit[1]);
                        break;
                    case "-y":
                        y = int.Parse(argSplit[1]);
                        break;
                    case "-z":
                        z = int.Parse(argSplit[1]);
                        break;
                    case "-amt":
                        amt = int.Parse(argSplit[1]);
                        break;
                    case "-id":
                        id = int.Parse(argSplit[1]);
                        break;
                    default:
                        Debug.LogWarning("Unknown argument - '" + argSplit[0] + "', it was ignored.");
                        break; 
                }
            }

            if (id < 0)
            {
                Debug.LogWarning("Identificator is negative or not provided.");
                return;
            }
            if (amt <= 0)
            {
                Debug.LogWarning("Amount is negative or zero.");
                return;
            }

            GameObject prefab = (GameObject)Resources.Load("Environment/Pref_LP_Bush001");
            GameObject cube = MonoBehaviour.Instantiate(prefab, new Vector3(x, y, z), new Quaternion());
        }
    }
}