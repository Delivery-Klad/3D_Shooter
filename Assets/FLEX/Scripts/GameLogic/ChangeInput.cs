using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeInput : MonoBehaviour
{
    public GameObject MoveForward;
    public GameObject MoveLeft;
    public GameObject MoveRight;
    public GameObject MoveBack;
    public GameObject AimButton;
    public GameObject JumpButton;
    public GameObject SprintButton;
    public GameObject ReloadButton;
    public GameObject FireButton;
    public GameObject LeanLeftButton;
    public GameObject LeanRightButton;
    public GameObject SuicideButton;
    public GameObject PauseButton;
    public GameObject ScoreButton;
    public GameObject SwitchFireButton;
    public GameObject UseButton;
    public GameObject CrouchButton;
    public GameObject ThrowButton;
    public GameObject OpenConsole;
    public GameObject ZoomButton;

    public bool Move_Forward;
    public bool Move_Left;
    public bool Move_Right;
    public bool Move_Back;
    public bool Aim_Button;
    public bool Jump_Button;
    public bool Sprint_Button;
    public bool Reload_Button;
    public bool Fire_Button;
    public bool LeanLeft_Button;
    public bool LeanRight_Button;
    public bool Suicide_Button;
    public bool Pause_Button;
    public bool Score_Button;
    public bool SwitchFire_Button;
    public bool Use_Button;
    public bool Crouch_Button;
    public bool Throw_Button;
    public bool Open_Console;
    public bool Zoom_Button;

    public KeyCode KC = KeyCode.None;

    void Start()
    {
        //MoveForward = GameObject.Find("WalkPanel");
        //MoveLeft = GameObject.Find("LeftPanel");
        //MoveRight = GameObject.Find("RightPanel");
        //MoveBack = GameObject.Find("BackPanel");
        AimButton = GameObject.Find("AIMPanel");
        JumpButton = GameObject.Find("JumpPanel");
        SprintButton = GameObject.Find("SprintPanel");
        ReloadButton = GameObject.Find("ReloadPanel");
        FireButton = GameObject.Find("FirePanel");
        LeanLeftButton = GameObject.Find("LeanLeftPanel");
        LeanRightButton = GameObject.Find("LeanRightPanel");
        SuicideButton = GameObject.Find("SuicidePanel");
        PauseButton = GameObject.Find("PausePanel");
        ScoreButton = GameObject.Find("ScorePanel");
        SwitchFireButton = GameObject.Find("SwitchPanel");
        UseButton = GameObject.Find("UsePanel");
        CrouchButton = GameObject.Find("CrouchPanel");
        ThrowButton = GameObject.Find("ThrowPanel");
        OpenConsole = GameObject.Find("OpenConsolePanel");
        ZoomButton = GameObject.Find("ZoomPanel");

        if (PlayerPrefs.HasKey("MoveForward_Keycode"))
        {
            //PlayerInputManager.instance.MoveForward = (KeyCode)PlayerPrefs.GetInt("MoveForward_Keycode");
        }
        if (PlayerPrefs.HasKey("MoveLeft_Keycode"))
        {
            //PlayerInputManager.instance.MoveLeft = (KeyCode)PlayerPrefs.GetInt("MoveLeft_Keycode");
        }
        if (PlayerPrefs.HasKey("MoveRight_Keycode"))
        {
            //PlayerInputManager.instance.MoveRight = (KeyCode)PlayerPrefs.GetInt("MoveRight_Keycode");
        }
        if (PlayerPrefs.HasKey("MoveBack_Keycode"))
        {
            //PlayerInputManager.instance.MoveBack = (KeyCode)PlayerPrefs.GetInt("MoveBack_Keycode");
        }
        if (PlayerPrefs.HasKey("Aim_Keycode"))
        {
            PlayerInputManager.instance.AimButton = (KeyCode)PlayerPrefs.GetInt("Aim_Keycode");
        }
        if (PlayerPrefs.HasKey("Jump_Keycode"))
        {
            PlayerInputManager.instance.JumpButton = (KeyCode)PlayerPrefs.GetInt("Jump_Keycode");
        }
        if (PlayerPrefs.HasKey("Sprint_Keycode"))
        {
            PlayerInputManager.instance.SprintButton = (KeyCode)PlayerPrefs.GetInt("Sprint_Keycode");
        }
        if (PlayerPrefs.HasKey("Reload_Keycode"))
        {
            PlayerInputManager.instance.ReloadButton = (KeyCode)PlayerPrefs.GetInt("Reload_Keycode");
        }
        if (PlayerPrefs.HasKey("Fire_Keycode"))
        {
            PlayerInputManager.instance.FireButton = (KeyCode)PlayerPrefs.GetInt("Fire_Keycode");
        }
        if (PlayerPrefs.HasKey("LeanLeft_Keycode"))
        {
            PlayerInputManager.instance.NaclonLeftButton = (KeyCode)PlayerPrefs.GetInt("LeanLeft_Keycode");
        }
        if (PlayerPrefs.HasKey("LeanRight_Keycode"))
        {
            PlayerInputManager.instance.NaclonRightButton = (KeyCode)PlayerPrefs.GetInt("LeanRight_Keycode");
        }
        if (PlayerPrefs.HasKey("Suicide_Keycode"))
        {
            PlayerInputManager.instance.SuicideButton = (KeyCode)PlayerPrefs.GetInt("Suicide_Keycode");
        }
        if (PlayerPrefs.HasKey("Pause_Keycode"))
        {
            PlayerInputManager.instance.PauseButton = (KeyCode)PlayerPrefs.GetInt("Pause_Keycode");
        }
        if (PlayerPrefs.HasKey("Score_Keycode"))
        {
            PlayerInputManager.instance.ScoreButton = (KeyCode)PlayerPrefs.GetInt("Score_Keycode");
        }
        if (PlayerPrefs.HasKey("Switch_Keycode"))
        {
            PlayerInputManager.instance.SwitchFireButton = (KeyCode)PlayerPrefs.GetInt("Switch_Keycode");
        }
        if (PlayerPrefs.HasKey("Use_Keycode"))
        {
            PlayerInputManager.instance.Use_Button = (KeyCode)PlayerPrefs.GetInt("Use_Keycode");
        }
        if (PlayerPrefs.HasKey("Crouch_Keycode"))
        {
            PlayerInputManager.instance.CrouchButton = (KeyCode)PlayerPrefs.GetInt("Crouch_Keycode");
        }
        if (PlayerPrefs.HasKey("Throw_Keycode"))
        {
            PlayerInputManager.instance.ThrowButton = (KeyCode)PlayerPrefs.GetInt("Throw_Keycode");
        }
        if (PlayerPrefs.HasKey("Console_Keycode"))
        {
            PlayerInputManager.instance.OpenConsole = (KeyCode)PlayerPrefs.GetInt("Console_Keycode");
        }
        if (PlayerPrefs.HasKey("Zoom_Keycode"))
        {
            PlayerInputManager.instance.ZoomButton = (KeyCode)PlayerPrefs.GetInt("Zoom_Keycode");
        }
        if (PlayerPrefs.HasKey("Voice_Keycode"))
        {
            PlayerInputManager.instance.SpeakButton = (KeyCode)PlayerPrefs.GetInt("Voice_Keycode");
        }
        ToggleBools(false);
        FillButtonsText();
    }
    
    void OnGUI()
    {
        //if (Move_Forward)
        //{
        //    if (Event.current.keyCode != KeyCode.None)
        //    {
        //        PlayerInputManager.instance.MoveForward = Event.current.keyCode;
        //        MoveForward.GetComponentInChildren<Text>().text = Event.current.keyCode.ToString();
        //        PlayerPrefs.SetInt("MoveForward_Keycode", (int)Event.current.keyCode);
        //        Move_Forward = false;
        //    }
        //}
        //if (Move_Left)
        //{
        //    if (Event.current.keyCode != KeyCode.None)
        //    {
        //        PlayerInputManager.instance.MoveLeft = Event.current.keyCode;
        //        MoveLeft.GetComponentInChildren<Text>().text = Event.current.keyCode.ToString();
        //        PlayerPrefs.SetInt("MoveLeft_Keycode", (int)Event.current.keyCode);
        //        Move_Left = false;
        //    }
        //}
        //if (Move_Right)
        //{
        //    if (Event.current.keyCode != KeyCode.None)
        //    {
        //        PlayerInputManager.instance.MoveRight = Event.current.keyCode;
        //        MoveRight.GetComponentInChildren<Text>().text = Event.current.keyCode.ToString();
        //        PlayerPrefs.SetInt("MoveRight_Keycode", (int)Event.current.keyCode);
        //        Move_Right = false;
        //    }
        //}
        //if (Move_Back)
        //{
        //    if (Event.current.keyCode != KeyCode.None)
        //    {
        //        PlayerInputManager.instance.MoveBack = Event.current.keyCode;
        //        MoveBack.GetComponentInChildren<Text>().text = Event.current.keyCode.ToString();
        //        PlayerPrefs.SetInt("Forward_Keycode", (int)Event.current.keyCode);
        //        Move_Back = false;
        //    }
        //}
        if (SomeThingButton())
        {
            if (Event.current.keyCode != KeyCode.None)
            {
                KC = Event.current.keyCode;
            }
            if (Event.current.isMouse)
            {
                KC = (KeyCode)Event.current.button + 323;
            }
            if (Event.current.shift)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    KC = KeyCode.LeftShift;
                }
                if (Input.GetKey(KeyCode.RightShift))
                {
                    KC = KeyCode.RightShift;
                }
            }
        }
        else
        {
            KC = KeyCode.None;
        }

        if (Aim_Button)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.AimButton = KC;
                AimButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("Aim_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(AimButton, true, 1));
                Aim_Button = false;
            }
        }
        if (Jump_Button)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.JumpButton = KC;
                JumpButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("Jump_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(JumpButton, true, 1));
                Jump_Button = false;
            }
        }
        if (Sprint_Button)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.SprintButton = KC;
                SprintButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("Sprint_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(SprintButton, true, 1));
                Sprint_Button = false;
            }
        }
        if (Reload_Button)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.ReloadButton = KC;
                ReloadButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("Reload_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(ReloadButton, true, 1));
                Reload_Button = false;
            }
        }
        if (Fire_Button)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.FireButton = KC;
                FireButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("Fire_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(FireButton, true, 1));
                Fire_Button = false;
            }
        }
        if (LeanLeft_Button)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.NaclonLeftButton = KC;
                LeanLeftButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("LeanLeft_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(LeanLeftButton, true, 1));
                LeanLeft_Button = false;
            }
        }
        if (LeanRight_Button)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.NaclonRightButton = KC;
                LeanRightButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("LeanRight_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(LeanRightButton, true, 1));
                LeanRight_Button = false;
            }
        }
        if (Suicide_Button)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.SuicideButton = KC;
                SuicideButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("Suicide_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(SuicideButton, true, 1));
                Suicide_Button = false;
            }
        }
        if (Pause_Button)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.PauseButton = KC;
                PauseButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("Pause_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(PauseButton, true, 1));
                Pause_Button = false;
            }
        }
        if (Score_Button)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.ScoreButton = KC;
                ScoreButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("Score_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(ScoreButton, true, 1));
                Score_Button = false;
            }
        }
        if (SwitchFire_Button)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.SwitchFireButton = KC;
                SwitchFireButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("Switch_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(SwitchFireButton, true, 1));
                SwitchFire_Button = false;
            }
        }
        if (Use_Button)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.Use_Button = KC;
                UseButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("Use_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(UseButton, true, 1));
                Use_Button = false;
            }
        }
        if (Crouch_Button)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.CrouchButton = KC;
                CrouchButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("Crouch_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(CrouchButton, true, 1));
                Crouch_Button = false;
            }
        }
        if (Throw_Button)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.ThrowButton = KC;
                ThrowButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("Throw_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(ThrowButton, true, 1));
                Throw_Button = false;
            }
        }
        if (Open_Console)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.OpenConsole = KC;
                OpenConsole.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("Console_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(OpenConsole, true, 1));
                Open_Console = false;
            }
        }
        if (Zoom_Button)
        {
            if (KC != KeyCode.None)
            {
                PlayerInputManager.instance.ZoomButton = KC;
                ZoomButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = KC.ToString();
                PlayerPrefs.SetInt("Zoom_Keycode", (int)KC);
                StartCoroutine(ToggleButtonScript(ZoomButton, true, 1));
                Zoom_Button = false;
            }
        }
    }

    bool SomeThingButton()
    {
        if (Move_Forward)
        {
            return true;
        }
        if (Move_Left)
        {
            return true;
        }
        if (Move_Right)
        {
            return true;
        }
        if (Move_Back)
        {
            return true;
        }
        if (Aim_Button)
        {
            return true;
        }
        if (Jump_Button)
        {
            return true;
        }
        if (Sprint_Button)
        {
            return true;
        }
        if (Reload_Button)
        {
            return true;
        }
        if (Fire_Button)
        {
            return true;
        }
        if (LeanLeft_Button)
        {
            return true;
        }
        if (LeanRight_Button)
        {
            return true;
        }
        if (Suicide_Button)
        {
            return true;
        }
        if (Pause_Button)
        {
            return true;
        }
        if (Score_Button)
        {
            return true;
        }
        if (SwitchFire_Button)
        {
            return true;
        }
        if (Use_Button)
        {
            return true;
        }
        if (Crouch_Button)
        {
            return true;
        }
        if (Throw_Button)
        {
            return true;
        }
        if (Open_Console)
        {
            return true;
        }
        if (Zoom_Button)
        {
            return true;
        }
        return false;
    }

    void ToggleBools(bool Toggle)
    {
        Move_Forward = Toggle;
        Move_Left = Toggle;
        Move_Right = Toggle;
        Move_Back = Toggle;
        Aim_Button = Toggle;
        Jump_Button = Toggle;
        Sprint_Button = Toggle;
        Reload_Button = Toggle;
        Fire_Button = Toggle;
        LeanLeft_Button = Toggle;
        LeanRight_Button = Toggle;
        Suicide_Button = Toggle;
        Pause_Button = Toggle;
        Score_Button = Toggle;
        SwitchFire_Button = Toggle;
        Use_Button = Toggle;
        Crouch_Button = Toggle;
        Throw_Button = Toggle;
        Open_Console = Toggle;
        Zoom_Button = Toggle;
        //MoveForward.GetComponentInChildren<Button>().enabled = true;
        //MoveLeft.GetComponentInChildren<Button>().enabled = true;
        //MoveRight.GetComponentInChildren<Button>().enabled = true;
        //MoveBack.GetComponentInChildren<Button>().enabled = true;
        AimButton.GetComponentInChildren<Button>().enabled = true;
        JumpButton.GetComponentInChildren<Button>().enabled = true;
        SprintButton.GetComponentInChildren<Button>().enabled = true;
        ReloadButton.GetComponentInChildren<Button>().enabled = true;
        FireButton.GetComponentInChildren<Button>().enabled = true;
        LeanLeftButton.GetComponentInChildren<Button>().enabled = true;
        LeanRightButton.GetComponentInChildren<Button>().enabled = true;
        SuicideButton.GetComponentInChildren<Button>().enabled = true;
        PauseButton.GetComponentInChildren<Button>().enabled = true;
        ScoreButton.GetComponentInChildren<Button>().enabled = true;
        SwitchFireButton.GetComponentInChildren<Button>().enabled = true;
        UseButton.GetComponentInChildren<Button>().enabled = true;
        CrouchButton.GetComponentInChildren<Button>().enabled = true;
        ThrowButton.GetComponentInChildren<Button>().enabled = true;
        OpenConsole.GetComponentInChildren<Button>().enabled = true;
        ZoomButton.GetComponentInChildren<Button>().enabled = true;
    }

    public void ResetButtons()
    {
        PlayerInputManager.instance.AimButton = KeyCode.Mouse1;
        PlayerPrefs.SetInt("Aim_Keycode", (int)KeyCode.Mouse1);
        PlayerInputManager.instance.JumpButton = KeyCode.Space;
        PlayerPrefs.SetInt("Jump_Keycode", (int)KeyCode.Space);
        PlayerInputManager.instance.SprintButton = KeyCode.LeftShift;
        PlayerPrefs.SetInt("Sprint_Keycode", (int)KeyCode.LeftShift);
        PlayerInputManager.instance.ReloadButton = KeyCode.R;
        PlayerPrefs.SetInt("Reload_Keycode", (int)KeyCode.R);
        PlayerInputManager.instance.FireButton = KeyCode.Mouse0;
        PlayerPrefs.SetInt("Fire_Keycode", (int)KeyCode.Mouse0);
        PlayerInputManager.instance.NaclonLeftButton = KeyCode.Q;
        PlayerPrefs.SetInt("LeanLeft_Keycode", (int)KeyCode.Q);
        PlayerInputManager.instance.NaclonRightButton = KeyCode.E;
        PlayerPrefs.SetInt("LeanRight_Keycode", (int)KeyCode.E);
        PlayerInputManager.instance.SuicideButton = KeyCode.P;
        PlayerPrefs.SetInt("Suicide_Keycode", (int)KeyCode.P);
        PlayerInputManager.instance.PauseButton = KeyCode.Escape;
        PlayerPrefs.SetInt("Pause_Keycode", (int)KeyCode.Escape);
        PlayerInputManager.instance.ScoreButton = KeyCode.Tab;
        PlayerPrefs.SetInt("Score_Keycode", (int)KeyCode.Tab);
        PlayerInputManager.instance.SwitchFireButton = KeyCode.Z;
        PlayerPrefs.SetInt("Switch_Keycode", (int)KeyCode.Z);
        PlayerInputManager.instance.Use_Button = KeyCode.F;
        PlayerPrefs.SetInt("Use_Keycode", (int)KeyCode.F);
        PlayerInputManager.instance.CrouchButton = KeyCode.LeftControl;
        PlayerPrefs.SetInt("Crouch_Keycode", (int)KeyCode.LeftControl);
        PlayerInputManager.instance.ThrowButton = KeyCode.G;
        PlayerPrefs.SetInt("Throw_Keycode", (int)KeyCode.G);
        PlayerInputManager.instance.OpenConsole = KeyCode.BackQuote;
        PlayerPrefs.SetInt("Console_Keycode", (int)KeyCode.BackQuote);
        PlayerInputManager.instance.ZoomButton = KeyCode.Mouse2;
        PlayerPrefs.SetInt("Zoom_Keycode", (int)KeyCode.Mouse2);
        PlayerInputManager.instance.SpeakButton = KeyCode.V;
        PlayerPrefs.SetInt("Voice_Keycode", (int)KeyCode.V);
    }

    public void ChangeMoveForward()
    {
        ToggleBools(false);
        Move_Forward = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(MoveForward, false, 0));
        MoveForward.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeMoveLeft()
    {
        ToggleBools(false);
        Move_Left = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(MoveLeft, false, 0));
        MoveLeft.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeMoveRight()
    {
        ToggleBools(false);
        Move_Right = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(MoveRight, false, 0));
        MoveRight.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeMoveBack()
    {
        ToggleBools(false);
        Move_Back = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(MoveBack, false, 0));
        MoveBack.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeAimButton()
    {
        ToggleBools(false);
        Aim_Button = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(AimButton, false, 0));
        AimButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeJumpButton()
    {
        ToggleBools(false);
        Jump_Button = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(JumpButton, false, 0));
        JumpButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeSprintButton()
    {
        ToggleBools(false);
        Sprint_Button = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(SprintButton, false, 0));
        SprintButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeReloadButton()
    {
        ToggleBools(false);
        Reload_Button = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(ReloadButton, false, 0));
        ReloadButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeFireButton()
    {
        ToggleBools(false);
        Fire_Button = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(FireButton, false, 0));
        FireButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeLeanLeftButton()
    {
        ToggleBools(false);
        LeanLeft_Button = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(LeanLeftButton, false, 0));
        LeanLeftButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeLeanRightButton()
    {
        ToggleBools(false);
        LeanRight_Button = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(LeanRightButton, false, 0));
        LeanRightButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeSuicideButton()
    {
        ToggleBools(false);
        Suicide_Button = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(SuicideButton, false, 0));
        SuicideButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeScoreButton()
    {
        ToggleBools(false);
        Score_Button = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(ScoreButton, false, 0));
        ScoreButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeSwitchFireButton()
    {
        ToggleBools(false);
        SwitchFire_Button = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(SwitchFireButton, false, 0));
        SwitchFireButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeUseButton()
    {
        ToggleBools(false);
        Use_Button = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(UseButton, false, 0));
        UseButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeCrouchButton()
    {
        ToggleBools(false);
        Crouch_Button = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(CrouchButton, false, 0));
        CrouchButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangePauseButton()
    {
        ToggleBools(false);
        Pause_Button = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(PauseButton, false, 0));
        PauseButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeThrowButton()
    {
        ToggleBools(false);
        Throw_Button = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(ThrowButton, false, 0));
        ThrowButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeConsoleButton()
    {
        ToggleBools(false);
        Open_Console = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(OpenConsole, false, 0));
        OpenConsole.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }
    public void ChangeZoomButton()
    {
        ToggleBools(false);
        Zoom_Button = true;
        FillButtonsText();
        StartCoroutine(ToggleButtonScript(ZoomButton, false, 0));
        ZoomButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = "___";
    }

    IEnumerator ToggleButtonScript(GameObject button, bool toggle, float time)
    {
        yield return new WaitForSeconds(time);
        button.GetComponentInChildren<Button>().enabled = toggle;
    }

    void FillButtonsText()
    {
        if (!Move_Forward)
        {
            //MoveForward.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.MoveForward.ToString();
        }
        if (!Move_Left)
        {
            //MoveLeft.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.MoveLeft.ToString();
        }
        if (!Move_Right)
        {
            //MoveRight.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.MoveRight.ToString();
        }
        if (!Move_Back)
        {
            //MoveBack.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.MoveBack.ToString();
        }
        if (!Aim_Button)
        {
            AimButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.AimButton.ToString();
        }
        if (!Jump_Button)
        {
            JumpButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.JumpButton.ToString();
        }
        if (!Sprint_Button)
        {
            SprintButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.SprintButton.ToString();
        }
        if (!Reload_Button)
        {
            ReloadButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.ReloadButton.ToString();
        }
        if (!Fire_Button)
        {
            FireButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.FireButton.ToString();
        }
        if (!LeanLeft_Button)
        {
            LeanLeftButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.NaclonLeftButton.ToString();
        }
        if (!LeanRight_Button)
        {
            LeanRightButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.NaclonRightButton.ToString();
        }
        if (!Suicide_Button)
        {
            SuicideButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.SuicideButton.ToString();
        }
        if (!Pause_Button)
        {
            PauseButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.PauseButton.ToString();
        }
        if (!Score_Button)
        {
            ScoreButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.ScoreButton.ToString();
        }
        if (!SwitchFire_Button)
        {
            SwitchFireButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.SwitchFireButton.ToString();
        }
        if (!Use_Button)
        {
            UseButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.Use_Button.ToString();
        }
        if (!Crouch_Button)
        {
            CrouchButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.CrouchButton.ToString();
        }
        if (!Throw_Button)
        {
            ThrowButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.ThrowButton.ToString();
        }
        if (!Open_Console)
        {
            OpenConsole.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.OpenConsole.ToString();
        }
        if (!Zoom_Button)
        {
            ZoomButton.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = PlayerInputManager.instance.ZoomButton.ToString();
        }
    }
}
