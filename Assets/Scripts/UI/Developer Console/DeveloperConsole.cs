using System;
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
        private int clipboardSize = 5;

        private List<string> _clipboard;

        private int _clipboardIndexer = 0;

        private int  _clipboardCursor = 0;
        
        
        [SerializeField] private CursorManager cursorManager;
        
        #region Colors

        public static readonly string ErrorColor = "#db6f63";

        public static readonly string OptionalColor = "#11bf68";

        public static readonly string WarningColor = "#bf9c0d";

        public static readonly string ExecutedColor = "#1ca32e";
        
        public static readonly string ConsoleColor = "#445794";
        
        

        #endregion

        #region Typical Console Messages
        
        private static readonly string ConsolePretext = $"<color={ConsoleColor}>>>> </color>";
        
        private static readonly string CommandPretext = $"<color={ConsoleColor}><<< </color>";
        

        public static readonly string NotRecognized = ConsolePretext + $"Command not <color={ErrorColor}>recognized</color>.";

        public static readonly string ExecutedSuccessfully = ConsolePretext + $"Command executed <color={ExecutedColor}>successfully</color>.";
        
        private static readonly string CommandLengthIsZero = ConsolePretext + "Command length is zero.";
        
        private static readonly string CommandHasBeenAddedToTheConsole = ConsolePretext + $"<color={ExecutedColor}>{{0}}</color> command has been added to the console.";
        
        public static readonly string ArgumentIgnored = ConsolePretext + $"Unknown argument - '<color={WarningColor}>{{0}}</color>', it was ignored.";


        
        
        #endregion
        
        // MonoBehavior methods
        private void Awake()
        {
            if (Instance != null)
            {
                return; // Probably has to destroy itself if there is an instance already to avoid duplicates 
            }

            Instance = this;
            Commands = new Dictionary<string, ConsoleCommand>();
            _inputReader = Resources.Load<InputReader>("Input/Default Input Reader");   
        }

        private void Start()
        {
            _clipboard = new List<string>();
            _clipboard.Capacity = clipboardSize;
            consolePanel.gameObject.SetActive(false);
            var primary = "#F9F0E6";
            var secondary = "#B3E6F9";

            consoleText.text = "---------------------------------------------------------------------------------\n" +
                                $"<size=60><color={primary}>Starting Developer Console</color></size> \n" +
                                "---------------------------------------------------------------------------------\n\n" +
                                "Type <color=orange>help</color> for list of available commands. \n" +
                                "Type <color=orange><command> -help </color> for command details. \n" +
                                $"<color={secondary}>Loading commands...</color>\n \n \n";
            CreateCommands();
        }
        
        private void OnEnable()
        {
            _inputReader.ConsoleVisibilityEvent += ManageConsoleWindow;
            _inputReader.ConfirmEvent += EnterCommand;
            _inputReader.MoveSelectionEvent += ClipboardMove;
            Application.logMessageReceived += HandleLog;
        }

        

        private void OnDisable()
        {
            _inputReader.ConsoleVisibilityEvent -= ManageConsoleWindow;
            _inputReader.ConfirmEvent -= EnterCommand;
            _inputReader.MoveSelectionEvent -= ClipboardMove;
            Application.logMessageReceived -= HandleLog;
        }

        //=========================================
        // Event handlers 
        private void ManageConsoleWindow()
        {
            consolePanel.gameObject.SetActive(!consolePanel.gameObject.activeInHierarchy);
            if (consolePanel.gameObject.activeInHierarchy)
            {
                _inputReader.EnableMenuInput();
                cursorManager.UnlockCursor();
                consoleInput.ActivateInputField();
                consoleInput.Select();
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
                StoreCommandToClipboard(consoleInput.text);
                consoleInput.text = "";
                consoleInput.ActivateInputField();
                consoleInput.Select();
            }
            else
            {
                //Debug.LogError("Error. Empty command.");
            }
        }

        private void ClipboardMove(Vector2 moveDir)
        {
            if(clipboardSize == 0 || _clipboardIndexer == 0) return;
            
            if (moveDir == Vector2.up)
            {
                if(_clipboardCursor>=_clipboardIndexer-1) return;
                _clipboardCursor++;
                consoleInput.text = _clipboard[_clipboardCursor];
                consoleInput.caretPosition = consoleInput.text.Length;
            }
            else if (moveDir == Vector2.down)
            {
                if(_clipboardCursor<=0) return;
                _clipboardCursor--;
                
                consoleInput.text = _clipboard[_clipboardCursor];
                consoleInput.caretPosition = consoleInput.text.Length;
            }
            
            
        }

        private void HandleLog(string logMessage, string stackTrace, LogType type)
        {
            string message = "[" + type.ToString() + "] " + logMessage;
            AddMessageToConsole(message);
        }
        //================================
        // Methods
        private void CreateCommands()
        {
            AddCommandToConsole(new CommandQuit());
            AddCommandToConsole(new CommandCreate());
            AddCommandToConsole(new CommandClear());
            AddCommandToConsole(new CommandHelp());
            AddCommandToConsole(new CommandGive());
            AddCommandToConsole(new CommandVoxel());    
            
        }
        private void StoreCommandToClipboard(string command)
        {
            if(clipboardSize == 0) return;
            _clipboard.Insert(0, command); 
            _clipboardCursor = -1;
            if(_clipboard.Count >= clipboardSize +1)
            {
                _clipboard = _clipboard.GetRange(1,clipboardSize);
            }
            else
            {
                _clipboardIndexer++;
            }

            // var clipboard1 = _clipboard.ToArray();
            // Debug.Log("-------------------");
            // for (int i = 0; i < clipboard1.Length; i++)
            // {
            //     Debug.Log(clipboard1[i]);
            // }
            // Debug.Log("-------------------");
        }
        public void AddCommandToConsole( ConsoleCommand command)
        {
            if (!Commands.ContainsKey(command.Command))
            {
                Commands.Add(command.Command, command);
                
                AddMessageToConsole(String.Format(CommandHasBeenAddedToTheConsole,command.Name));
            }
        }
        
        public void ClearConsole()
        {
            consoleText.text = "";
        }
        
        public void AddMessageToConsole(string msg)
        {
            consoleText.text += msg + "\n";
        }
        
        private void  ParseInput(string inputRaw)
        {
            inputRaw = inputRaw.TrimEnd();
            string[] input = inputRaw.Split(' ');
            if (input.Length == 0)
            {
                AddMessageToConsole(CommandLengthIsZero);
                return;
            }
            if (Commands.ContainsKey(input[0]) == false)
            {
                AddMessageToConsole(NotRecognized);
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

        
        
    }
}
