using System;
using ManExe.Core;
using ManExe.World;
using UnityEngine;

namespace ManExe.UI.Developer_Console.Commands
{

    public class CommandVoxel : ConsoleCommand
    {
        struct VoxelArgs
        {
            public int ID;
            public Vector3 Pos;
            public int Size;
        }

        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        private World.World _world;
        
        public CommandVoxel()
        {
            Name = "Voxel";
            Command = "voxel";
            Description = "Fills voxels with given material at the given position.";
            Help = "Use this command fill voxels or clear them. \n" +
                   " - ID [-id=1] - int, > 0, unique ID of the object to create , REQUIRED\n" +
                   " - Position [-x=1 -y=1 -z=1] - int, position where objets will be created, default = [0,0,0]";
            
        }
        
        private VoxelArgs FillArgs(string[] argSplit, VoxelArgs args)
        {
            switch (argSplit[0])
            {
                case "-x":
                    args.Pos.x = int.Parse(argSplit[1]);
                    break;
                case "-y":
                    args.Pos.y = int.Parse(argSplit[1]);
                    break;
                case "-z":
                    args.Pos.z = int.Parse(argSplit[1]);
                    break;
                case "-id":
                    args.ID = int.Parse(argSplit[1]);
                    break;
                case "-size":
                    args.Size = int.Parse(argSplit[1]);
                    break;
                default:
                {
                    Debug.LogWarning(String.Format(DeveloperConsole.ArgumentIgnored,argSplit[0]));
                    break;
                }
            }

            return args;
        }
        
        private bool ValidateArgs(VoxelArgs args)
        {
            if (args.ID < 0)
            {
                Debug.LogWarning("Identificator is negative or not provided.");
                return false;
            }
            
            if (args.Size <= 0)
            {
                Debug.LogWarning("Size is less than 1 or not provided.");
                return false;
            }

            if (args.Pos.y < 0 || args.Pos.y > GameData.ChunkHeight)
            {
                Debug.LogWarning("Incorrect Y coordinate provided.");
                return false;
            }

            if (args.Pos.x < 0 || args.Pos.x > _world.WorldSizeInVoxelsX)
            {
                Debug.LogWarning("Incorrect X coordinate provided.");
                return false;
            }

            if (args.Pos.z < 0 || args.Pos.z > _world.WorldSizeInVoxelsZ)
            {
                Debug.LogWarning("Incorrect Z coordinate provided.");
                return false;
            }

            return true;
        }
        
        public override void RunCommand(string[] argsArr)
        {
            VoxelArgs args = new VoxelArgs();
            args.Size = 1;
            args =  ParseArgs(FillArgs, argsArr, args);

            if (_world == null)
            {
                _world = GameObject.FindGameObjectWithTag("World").GetComponent<World.World>();
            }
            if (_world == null)
            {
                Debug.LogWarning("World not found. Command terminated.");
                return;
            }

            if (ValidateArgs(args))
            {
                Chunk chunk = _world.GetChunkFromVector3Validated(args.Pos);
                if (chunk != null)
                    _world.DrawTerrain(args.Pos,new Vector3(args.Size,args.Size,args.Size));
            }
        }

        
    }
}