using System;
using UnityEngine;
using System.Text.RegularExpressions;
using ManExe.Scriptable_Objects;


namespace ManExe.UI.Developer_Console.Commands
{

    public class CommandCreate : ConsoleCommand
    {
        struct CreateArgs
        {
            public int ID;
            public int Amount;
            public Vector3 Pos;
            public bool yRay;

        }
        
        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        private PlacementDatabase _placementDatabase;
        
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

        private CreateArgs FillArgs(string[] argSplit, CreateArgs args)
        {
            switch (argSplit[0])
            {
                case "-x":
                    args.Pos.x = int.Parse(argSplit[1]);
                    break;
                case "-y":
                    if (argSplit[1] == "*")
                    {
                        args.yRay = true;
                        break;
                    }
                    args.Pos.y = int.Parse(argSplit[1]);
                    break;
                case "-z":
                    args.Pos.z = int.Parse(argSplit[1]);
                    break;
                case "-amt":
                    args.Amount = int.Parse(argSplit[1]);
                    break;
                case "-id":
                    args.ID = int.Parse(argSplit[1]);
                    break;
                default:
                {
                    Debug.LogWarning(String.Format(DeveloperConsole.ArgumentIgnored,argSplit[0]));
                    break;
                }
            }

            return args;
        }
        
        public override void RunCommand(string[] argsArr)
        {
            // Default values for arguments.
            CreateArgs args = new CreateArgs();
            args.Amount = 1;
            args =  ParseArgs(FillArgs, argsArr, args);
            

            if (args.ID < 0)
            {
                Debug.LogWarning("Identificator is negative or not provided.");
                return;
            }
            if (args.Amount <= 0)
            {
                Debug.LogWarning("Amount is negative or zero.");
                return;
            }

            if (args.yRay == true)
            {
                if (!Physics.Raycast(new Vector3(args.Pos.x, short.MaxValue, args.Pos.z), Vector3.down,
                        out RaycastHit hit, Mathf.Infinity))
                {
                    Debug.Log($"Can't detect ground at location x={args.Pos.x} z={args.Pos.z}");
                    return;
                }

                args.Pos.y = hit.point.y;
            }
            if(_placementDatabase== null)
                _placementDatabase = (PlacementDatabase)Resources.Load("PlacementDatabase");
            PlacementSettings placementSettings = _placementDatabase.GetItem(args.ID);
            

            GameObject createdObj = MonoBehaviour.Instantiate(placementSettings.Prefab, new Vector3(args.Pos.x, args.Pos.y, args.Pos.z), new Quaternion());
            
            
            DevCon.AddMessageToConsole(DeveloperConsole.ExecutedSuccessfully);
        }

        
    }
}