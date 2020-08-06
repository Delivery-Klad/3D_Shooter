using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

	[HideInInspector]public static PlayerInputManager instance;
	[Header("Player Input Manager")]
	public bool FreezePlayerControls;
	public float AimSpeedModifier = 1f;
	public float InputX;			
	public float InputY;
	public float MouseX;
	public float MouseY;
    [HideInInspector] public bool Jump;
    [HideInInspector] public bool Sprint;
    [HideInInspector] public bool IsAuto;
    [HideInInspector] public bool Attack;
    [HideInInspector] public bool Aim;
    [HideInInspector] public bool Reload;
    [HideInInspector] public bool NextWeapon;
    [HideInInspector] public bool FirstSeat;
    [HideInInspector] public bool SecondSeat;
    [HideInInspector] public bool LeanLeft;
    [HideInInspector] public bool LeanRight;
    [HideInInspector] public bool ChangeCamera;
    [HideInInspector] public bool Suicide;
    [HideInInspector] public bool Pause;
    [HideInInspector] public bool UseButton;
    [HideInInspector] public bool Scoreboard;
    [HideInInspector] public bool SwitchFireMode;
    [HideInInspector] public bool Crouch;
    [HideInInspector] public bool Console;
    public bool Zoom;
    public KeyCode AimButton;
    public KeyCode JumpButton;
    public KeyCode SprintButton;
    public KeyCode ReloadButton;
    public KeyCode FireButton;
    public KeyCode NaclonLeftButton;
    public KeyCode NaclonRightButton;
    public KeyCode SuicideButton;
    public KeyCode PauseButton;
    public KeyCode ScoreButton;
    public KeyCode SwitchFireButton;
    public KeyCode Use_Button;
    public KeyCode CrouchButton;
    public KeyCode ThrowButton;
    public KeyCode OpenConsole;
    public KeyCode ZoomButton;

    void Awake()
	{
		if (instance == null)
        {
			instance = this;
		}
        else if (instance != null)
        {
			DestroyImmediate (this.gameObject);
		}
	}
	
	void Update ()
    {
		if (GameManager.instance.MatchActive)
        {
			HandlePlayerInput ();
		}
	}

	void HandlePlayerInput()
	{
		if (!FreezePlayerControls && !GameManager.instance.InVehicle)
        {
			InputX = Input.GetAxis ("Horizontal");
			InputY = Input.GetAxis ("Vertical");
			MouseX = Input.GetAxis ("Mouse X") * AimSpeedModifier;
			MouseY = Input.GetAxis ("Mouse Y") * AimSpeedModifier;
			Jump = Input.GetKeyDown (JumpButton);
			Sprint = Input.GetKey (SprintButton);
			Reload = Input.GetKeyDown (ReloadButton);
			if (IsAuto)
            {
				Attack = Input.GetKey (FireButton);
			}
            else
            {
				Attack = Input.GetKeyDown (FireButton);
			}
			Aim = Input.GetKey (AimButton);
			NextWeapon = Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetAxis("Mouse ScrollWheel") < 0;
			LeanLeft = Input.GetKey (NaclonLeftButton);
			LeanRight = Input.GetKey (NaclonRightButton);
			Suicide = Input.GetKeyDown (SuicideButton);
			Pause = Input.GetKeyDown (PauseButton);
			UseButton = Input.GetKeyDown (Use_Button);
			Scoreboard = Input.GetKey (ScoreButton);
			SwitchFireMode = Input.GetKeyDown (SwitchFireButton);
			Crouch = Input.GetKey (CrouchButton);
			FirstSeat = false;
			SecondSeat = false;
		}  
		if (FreezePlayerControls && GameManager.instance.InVehicle && !InGameUI.instance.Paused)
        {
			UseButton = Input.GetKeyDown (Use_Button);
			Pause = Input.GetKeyDown (PauseButton);
			Scoreboard = Input.GetKey (ScoreButton);
			InputX = Input.GetAxis ("Horizontal");
			InputY = Input.GetAxis ("Vertical");
			MouseX = Input.GetAxis ("Mouse X") * AimSpeedModifier;
			MouseY = Input.GetAxis ("Mouse Y") * AimSpeedModifier;
			Jump = Input.GetKeyDown (JumpButton);
			FirstSeat = Input.GetKeyDown (KeyCode.F1);
			SecondSeat = Input.GetKeyDown (KeyCode.F2);
			if (IsAuto)
            {
				Attack = Input.GetKey (FireButton);
			}
            else
            {
				Attack = Input.GetKeyDown (FireButton);
			}
		}
		if(FreezePlayerControls && !GameManager.instance.InVehicle || InGameUI.instance.Paused || !GameManager.instance.MatchActive)
        {
			InputX = 0f;
			InputY = 0f;
			MouseX = 0;
			MouseY = 0;
			Attack = false;
			ChangeCamera = false;
			LeanLeft = false;
			LeanRight = false;
			Sprint = false;
			Pause = Input.GetKeyDown (PauseButton);
			UseButton = false;
			SwitchFireMode = false;
			Scoreboard = Input.GetKey (ScoreButton);
			Crouch = false;
			FirstSeat = false;
			SecondSeat = false;
		}
        if (Input.GetKeyDown(ZoomButton))
        {
            Zoom = true;
        }
        if (Input.GetKeyUp(ZoomButton))
        {
            Zoom = false;
        }
	}
}
