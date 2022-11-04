using System;
using ManExe.Entity.Inventory;
using ManExe.Scriptable_Objects;
using UnityEngine;

namespace ManExe.UI.Developer_Console.Commands
{

    public class CommandGive : ConsoleCommand
    {
        private ItemDatabase _itemDatabase;

        struct GiveArgs
        {
            public int ID;
            public int Amount;
        }

        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }
        
        public CommandGive()
        {
            Name = "Give";
            Command = "give";
            Description = "Spawns item in players inventory.";
            Help = "Use this command to give player an item with given ID.\n" +
                   "If inventory is full item will not be given.\n" +
                   "Parameters:\n" +
                   " - Amount [-amt=1] - int, > 0, number of objects to be given, default = 1 \n" +
                   " - ID [-id=1] - int, > 0, unique ID of the item to give , REQUIRED\n";
            
        }

        private GiveArgs FillArgs(string[] argSplit, GiveArgs args)
        {
            switch (argSplit[0])
            {
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
            GiveArgs args = new GiveArgs();
            args.Amount = 1;
            args = ParseArgs(FillArgs, argsArr, args);
            
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
            
            var player = GameObject.FindWithTag("Player");
            var inventory = player.GetComponent<PlayerInventroryHolder>();
            if(_itemDatabase== null)
                _itemDatabase = (ItemDatabase)Resources.Load("ItemIdDatabase");
            var item = _itemDatabase.GetItem(args.ID);
            inventory.AddToInventory(item, args.Amount);
            DevCon.AddMessageToConsole(DeveloperConsole.ExecutedSuccessfully);
        }
        
        
    }
}