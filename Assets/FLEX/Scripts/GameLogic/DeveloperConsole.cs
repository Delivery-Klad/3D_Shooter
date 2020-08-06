using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Reflection;
using UnityEngine.Rendering.PostProcessing;

public class DeveloperConsole : MonoBehaviour
{
    [HideInInspector] public static DeveloperConsole instance;
    public GameObject ConsolePanel;
    public InputField InputCommand;
    public Text ConsoleText;
    public string[] command;
    public string[] CMD;
    public string myLog;
    public int CurrentCommandID = -1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            DestroyImmediate(this.gameObject);
        }
    }

    void Start()
    {
        ConsoleText.text = "";
        ConsolePanel.SetActive(false);
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void Update()
    {
        if (Input.GetKeyDown(PlayerInputManager.instance.OpenConsole) && !PlayerInputManager.instance.Pause)
        {
            if (!PlayerInputManager.instance.Console)
            {
                PlayerInputManager.instance.FreezePlayerControls = true;
                PlayerInputManager.instance.Console = true;
                ConsolePanel.SetActive(true);
                InputCommand.Select();
                GameManager.instance.SetCursorLock(false);
            }
            else
            {
                if (GameManager.instance.IsAlive)
                {
                    GameManager.instance.SetCursorLock(true);
                }
                PlayerInputManager.instance.FreezePlayerControls = false;
                PlayerInputManager.instance.Console = false;
                ConsolePanel.SetActive(false);
            }
        }
        if (PlayerInputManager.instance.Pause)
        {
            PlayerInputManager.instance.Console = false;
            ConsolePanel.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Return) && ConsolePanel.activeInHierarchy)
        {
            AddConsoleMessage(InputCommand.text, "grey");
            UpdateLastCommands(InputCommand.text);
            CMD = InputCommand.text.Split(' ');
            CommandsHandler(CMD);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && ConsolePanel.activeInHierarchy)
        {
            if (CurrentCommandID < command.Length - 1)
            {
                CurrentCommandID += 1;
            }
            if (command[CurrentCommandID] != "")
            {
                InputCommand.Select();
                InputCommand.text = command[CurrentCommandID];
            }
            else
            {
                CurrentCommandID -= 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && ConsolePanel.activeInHierarchy)
        {
            if(CurrentCommandID > 0)
            {
                CurrentCommandID -= 1;
            }
            if (command[CurrentCommandID] != "")
            {
                InputCommand.Select();
                InputCommand.text = command[CurrentCommandID];
            }
        }
    }

    void UpdateLastCommands(string cmd)
    {
        if (cmd != command[0])
        {
            string[] temp = new string[command.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = command[i];
            }
            command = new string[temp.Length + 1];
            for (int i = command.Length - 1; i > 0; i--)
            {
                command[i] = temp[i - 1];
            }
            command[0] = cmd;
        }
    }

    void CommandsHandler(string[] cmd)
    {
        if (cmd[0] == "godmode")
        {
            if (GameManager.instance.MatchActive)
            {
                if (cmd[1] == "on")
                {
                    GameManager.instance.LocalPlayer.GetComponent<PlayerStats>().GodMode = true;
                }
                else
                {
                    GameManager.instance.LocalPlayer.GetComponent<PlayerStats>().GodMode = false;
                }
                AddConsoleMessage("godmode is " + cmd[1], "green");
            }
            else
            {
                AddConsoleMessage("You are not in game", "red");
            }
        }
        else if (cmd[0] == "spawn")
        {
            if (GameManager.instance.MatchActive)
            {
                try
                {
                    PhotonNetwork.Instantiate(cmd[1], GameManager.instance.LocalPlayer.transform.position + new Vector3(-1.0f, 0.0f, 0.0f), GameManager.instance.LocalPlayer.transform.rotation, 0, null);
                    AddConsoleMessage("Object " + cmd[1] + " has been spawned", "green");
                }
                catch
                {
                    AddConsoleMessage("Object not found 404", "red");
                }
            }
            else
            {
                AddConsoleMessage("You are not in game", "red");
            }
        }
        else if (cmd[0] == "lighting_debug")
        {
            if (GameManager.instance.MatchActive)
            {
                if (GameManager.instance.LocalPlayer.GetComponent<PlayerStats>().isAlive)
                {
                    GameObject Camera = GameObject.Find("PlayerCamera");
                    if (cmd[1] == "on")
                    {
                        Camera.GetComponent<PostProcessDebug>().enabled = true;
                    }
                    else
                    {
                        Camera.GetComponent<PostProcessDebug>().enabled = false;
                    }
                }
            }
            else
            {
                AddConsoleMessage("You are not in game", "red");
            }
        }
        else if (cmd[0] == "kill")
        {
            if (GameManager.instance.MatchActive)
            {
                GameManager.instance.LocalPlayer.SendMessage("Console", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                AddConsoleMessage("You are not in game", "red");
            }
        }
        else if (cmd[0] == "clear" || cmd[0] == "clr")
        {
            clear();
        }
        else if (cmd[0] == "version")
        {
            AddConsoleMessage("Game version: " + PhotonNetworkManager.instance.GameVersion, "white");
        }
        else
        {
            AddConsoleMessage("Wrond input", "red");
        }
        CurrentCommandID = -1;
        InputCommand.Select();
        InputCommand.text = "";
    }

    public void AddConsoleMessage(string message, string color)
    {
        ConsoleText.text = ConsoleText.text + string.Format("\n<color={0}>{1}</color>", color, message);
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string color = "";
        myLog = logString;
        string newString = "[" + type + "] : " + myLog;
        if (type == LogType.Error)
        {
            color = "red";
        }
        else if (type == LogType.Warning)
        {
            color = "yellow";
        }
        else
        {
            color = "white";
        }
        if (type != LogType.Warning) //временно
            AddConsoleMessage(newString, color);
    }

    void clear()
    {
        ConsoleText.text = "";
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }
}
