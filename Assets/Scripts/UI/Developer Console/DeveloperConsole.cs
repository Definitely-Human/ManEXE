using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ManExe.Scriptable_Objects;
using ManExe.UI.Developer_Console.Commands;
using System.Linq;

namespace ManExe.UI.Developer_Console
{
    public class DeveloperConsole : MonoBehaviour
    {
        public static DeveloperConsole Instance { get; private set; }
        public Dictionary<string, ConsoleCommand> Commands { get; private set; }
        private InputReader _inputReader;

        [Header("Ui Component")] 
        public CanvasRenderer consolePanel;
        public ScrollRect scrollRect;
        public TextMeshProUGUI consoleText;
        public TextMeshProUGUI inputText;
        public TMP_InputField consoleInput;
        
        [SerializeField]
        [Tooltip("Define how many commands can be hold in the clipboard. If set to 0, clipboard will be off.")]
        private int clipboardSize;

        private string[] _clipboard;

        private int _clipboardIndexer = 0;

        private int  _clipboardCursor = 0;
        
        
        [SerializeField] private CursorManager cursorManager;
        private const string ConsolePretext = ">>> ";
        private const string CommandPretext = "<<< ";

        private void Awake()
        {
            if (Instance != null)
            {
                return; // Probably has to destroy itself if there is an instance already to avoid duplicates 
            }

            Instance = this;
            Commands = new Dictionary<string, ConsoleCommand>();
            _inputReader = Resources.Load<InputReader>("Input/Default Input Reader");// Loading multiple times may cause problem with reading input
        }

        private void Start()
        {
            consolePanel.gameObject.SetActive(false);
            CreateCommands();
        }
        
        private void OnEnable()
        {
            _inputReader.ConsoleVisibilityEvent += ManageConsoleWindow;
            _inputReader.ConfirmEvent += EnterCommand;
            Application.logMessageReceived += HandleLog;
        }
        private void OnDisable()
        {
            _inputReader.ConsoleVisibilityEvent -= ManageConsoleWindow;
            _inputReader.ConfirmEvent -= EnterCommand;
            Application.logMessageReceived -= HandleLog;
        }

        private void CreateCommands()
        {
            AddCommandToConsole(new CommandQuit());
            AddCommandToConsole(new CommandCreate());
            AddCommandToConsole(new CommandClear());
            AddCommandToConsole(new CommandHelp());
            
        }

        public void AddCommandToConsole( ConsoleCommand command)
        {
            if (!Commands.ContainsKey(command.Command))
            {
                Commands.Add(command.Command, command);
                string addMessage = " command has been added to the console";
                AddMessageToConsole(ConsolePretext + command.Name + addMessage);
            }
        }

        public void ClearConsole()
        {
            consoleText.text = "";
        }

        private void ManageConsoleWindow()
        {
            consolePanel.gameObject.SetActive(!consolePanel.gameObject.activeInHierarchy);
            if (consolePanel.gameObject.activeInHierarchy)
            {
                _inputReader.EnableMenuInput();
                cursorManager.UnlockCursor();
            }
            else
            {
                _inputReader.EnablePlayerInput();
                cursorManager.LockCursor();
            }
        }
        private void EnterCommand()
        {
            if (consoleInput.text != "")
            {
                AddMessageToConsole(CommandPretext + consoleInput.text);
                ParseInput(consoleInput.text);
                consoleInput.text = "";
            }
            else
            {
                //Debug.LogError("Error. Empty command.");
            }
        }


        public void AddMessageToConsole(string msg)
        {
            consoleText.text += msg + "\n";
        }
        
        private void  ParseInput(string inputRaw)
        {
            string[] input = inputRaw.Split(' ');
            if (input.Length == 0)
            {
                AddMessageToConsole(ConsolePretext + "Command length is zero.");
                return;
            }
            if (Commands.ContainsKey(input[0]) == false)
            {
                AddMessageToConsole(ConsolePretext + "Command not recognized.");
            }
            else
            {
                
                var args = input.Skip(1).ToArray();
                if (args.Contains("-help"))
                {
                    CommandHelp.HelpCommand(Commands[input[0]]);
                    return;
                }
                Commands[input[0]].RunCommand(args);
            }
            
        }

        private void HandleLog(string logMessage, string stackTrace, LogType type)
        {
            string message = "[" + type.ToString() + "] " + logMessage;
            AddMessageToConsole(message);
        }
        
    }
}
