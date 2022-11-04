using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ManExe.UI.Developer_Console
{
    public abstract class ConsoleCommand
    {
        public abstract string Name { get; protected set; }
        public abstract string Command { get; protected set; }
        public abstract string Description { get; protected set; }
        public abstract string Help { get; protected set; }
        
        private DeveloperConsole Console = DeveloperConsole.Instance;
        
        protected static readonly DeveloperConsole DevCon = DeveloperConsole.Instance;
        public abstract void RunCommand(string[] args);

        public T ParseArgs<T>(Func<string[],T,T> func,string[] argsArr, T args)
        {
            for (int i = 0; i < argsArr.Length; i++)
            {
                string argument = argsArr[i];
                string[] argSplit = Regex.Split(argument, @"\="); // Split every argument in two parts at '=' sign.

                args = func(argSplit, args);
            }
            return args;
        }
    }
}