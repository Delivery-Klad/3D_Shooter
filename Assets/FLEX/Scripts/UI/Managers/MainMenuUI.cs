using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {

    public Dropdown dropDown;
    UnityEngine.Resolution[] res;

    [HideInInspector]public static MainMenuUI instance;
	[Header("Меню")]
	public GameObject MainMenuPanel;
	public Text ConnectionStatusText;

	[Header("Сервера")]
	public Transform ServerListPanel;
	public GameObject ServerListEntryPrefab;
	public bool JustLoadedServerList = false;
	private List<GameObject> ServerListEntries = new List<GameObject> ();

	[Header("Создание сервера")]
	public InputField GameNameInputField;
	public Text SelectedGameTypeText;
	public Text SelectedMapText;
	public Text SelectedMaxPlayersText;
	private int SelectedGameTypeInt = 0;
	private int SelectedMapInt = 0;
	private byte SelectedMaxPlayersByte = 0;

	[Header("Настройки сервера")]
	public GameObject TimeLimitPanel;
	public Text SelectedTimeLimitText;
	private int SelectedTimeLimitInt = 0;
	private int SelectedScoreLimitInt = 0;

	[Header("Настройки")]
	public InputField PlayerNameInputfield;
	public Dropdown ResolutionDropDown;
	private Resolution[] Resolutions;
    public Slider GlobalVolume;
    private float volume;
    public Slider BloomSetting;
    public Slider ShutterAngleSetting;
    public Slider SaturationSetting;
    public Slider ContrastSetting;
    public Dropdown PostProcessingSetting;
    public Toggle FastModeFXAA;
    public Dropdown SMAAQuality;
    public Slider JitterSpreadTAA;
    public Slider StationaryBlendingTAA;
    public Slider MotionBlendingTAA;
    public Slider SharpnessTAA;

    public GameObject FXAAFastMode;
    public GameObject QualitySMAA;
    public GameObject TAAJitterSpread;
    public GameObject TAAStationaryBlending;
    public GameObject TAAMotionBlending;
    public GameObject TAASharpness;


    void Awake()
	{
		instance = this;
	}

	void Start()
	{
        LoadGraphicSettings();
        OnMenuStart ();

        UnityEngine.Resolution[] resolution = Screen.resolutions;
        res = resolution.Distinct().ToArray();
        string[] strRes = new string[res.Length];
        for (int i = 0; i < res.Length; i++)
        {
            strRes[i] = res[i].width.ToString() + "x" + res[i].height.ToString();
        }
        dropDown.AddOptions(strRes.ToList());
        if (PlayerPrefs.HasKey("Resol"))
        {
            dropDown.value = PlayerPrefs.GetInt("Resol");
            Screen.SetResolution(res[PlayerPrefs.GetInt("Resol")].width, res[PlayerPrefs.GetInt("Resol")].height, Screen.fullScreen);
        }
        else
        {
            dropDown.value = res.Length - 1;
            Screen.SetResolution(res[res.Length - 1].width, res[res.Length - 1].height, Screen.fullScreen);
        }
    }

	void OnMenuStart()
	{
		GameNameInputField.text = "GAME:" + Random.Range (1000, 5000);
		if (PlayerPrefs.GetString ("PlayerName") != "")
        {
			PlayerNameInputfield.text = PlayerPrefs.GetString ("PlayerName");
		}
        else
        {
			PlayerNameInputfield.text = "Guinea_Pig:" + Random.Range (1000, 5000);
		}
		PhotonNetwork.playerName = PlayerNameInputfield.text;
		SelectedGameTypeText.text = GameManager.instance.CurrentGameType.GameTypeLoadName;
		SelectedMapText.text = GameManager.instance.CurrentMap.MapName;
		//SelectedMapPreview.sprite = GameManager.instance.MapList[0].MapLoadImage;
	}

	void FixedUpdate()
	{
		if (GameManager.instance.Ingame == false)
        {
			MainMenuUI.instance.ConnectionStatusText.text = PhotonNetwork.connectionState.ToString ();
		}
    }

    void LoadGraphicSettings()
    {
        if (PlayerPrefs.HasKey("GlobalVolume"))
        {
            volume = PlayerPrefs.GetFloat("GlobalVolume");
            GlobalVolume.value = volume;
            AudioListener.volume = volume;
        }
        if (PlayerPrefs.HasKey("BloomSetting"))
        {
            BloomSetting.value = PlayerPrefs.GetFloat("BloomSetting");
        }
        if (PlayerPrefs.HasKey("ShutterAngleSetting"))
        {
            ShutterAngleSetting.value = PlayerPrefs.GetFloat("ShutterAngleSetting");
        }
        if (PlayerPrefs.HasKey("SaturationSetting"))
        {
            SaturationSetting.value = PlayerPrefs.GetFloat("SaturationSetting");
        }
        if (PlayerPrefs.HasKey("ContrastSetting"))
        {
            ContrastSetting.value = PlayerPrefs.GetFloat("ContrastSetting");
        }
        if (PlayerPrefs.HasKey("PostProcessingSetting"))
        {
            PostProcessingSetting.value = PlayerPrefs.GetInt("PostProcessingSetting");
        }
        if (PlayerPrefs.HasKey("FastMode"))
        {
            int Fast = PlayerPrefs.GetInt("FastMode");
            if(Fast == 1)
            {
                FastModeFXAA.isOn = true;
            }
            else
            {
                FastModeFXAA.isOn = false;
            }
        }
        if (PlayerPrefs.HasKey("SMAAQuality"))
        {
            SMAAQuality.value = PlayerPrefs.GetInt("SMAAQuality");
        }
        if (PlayerPrefs.HasKey("JitterSpreadTAA"))
        {
            JitterSpreadTAA.value = PlayerPrefs.GetFloat("JitterSpreadTAA");
        }
        if (PlayerPrefs.HasKey("StationaryBlendingTAA"))
        {
            StationaryBlendingTAA.value = PlayerPrefs.GetFloat("StationaryBlendingTAA");
        }
        if (PlayerPrefs.HasKey("MotionBlendingTAA"))
        {
            MotionBlendingTAA.value = PlayerPrefs.GetFloat("MotionBlendingTAA");
        }
        if (PlayerPrefs.HasKey("SharpnessTAA"))
        {
            SharpnessTAA.value = PlayerPrefs.GetFloat("SharpnessTAA");
        }
    }

    public void SaveGlobalVolume(Slider slider)
    {
        AudioListener.volume = slider.value;
        PlayerPrefs.SetFloat("GlobalVolume", slider.value);
    }

    public void SaveBloom(Slider slider)
    {
        PlayerPrefs.SetFloat("BloomSetting", slider.value);
        LoadGraphicSettings();
    }

    public void SaveMotionBloor(Slider slider)
    {
        PlayerPrefs.SetFloat("ShutterAngleSetting", slider.value);
        LoadGraphicSettings();
    }

    public void SaveSaturation(Slider slider)
    {
        PlayerPrefs.SetFloat("SaturationSetting", slider.value);
        LoadGraphicSettings();
    }

    public void SaveContrast(Slider slider)
    {
        PlayerPrefs.SetFloat("ContrastSetting", slider.value);
        LoadGraphicSettings();
    }

    public void SavePostProcessing(Dropdown dropdown)
    {
        PlayerPrefs.SetInt("PostProcessingSetting", dropdown.value);
        LoadGraphicSettings();
    }

    public void ResetAudioSettings(Slider slider)
    {
        AudioListener.volume = 1f;
        PlayerPrefs.SetFloat("GlobalVolume", 1f);
        slider.value = 1f;
    }

    public void ResetBloomSettings(Slider _bloom)
    {
        PlayerPrefs.SetFloat("BloomSetting", 1f);
        _bloom.value = 1f;
    }

    public void ResetMotionBloorSettings(Slider _motion)
    {
        PlayerPrefs.SetFloat("ShutterAngleSetting", 250f);
        _motion.value = 250f;
    }

    public void ResetSaturationSettings(Slider _saturation)
    {
        PlayerPrefs.SetFloat("SaturationSetting", 20f);
        _saturation.value = 20f;
    }

    public void ResetContrastSettings(Slider _contrast)
    {
        PlayerPrefs.SetFloat("ContrastSetting", -5f);
        _contrast.value = -5f;
    }

    public void ResetPostProcessingSettings(Dropdown _dropdown)
    {
        PlayerPrefs.SetInt("PostProcessingSetting", 2);
        _dropdown.value = 2;
        LoadGraphicSettings();
    }

    public void SaveFastMode(Toggle _toggle)
    {
        if (_toggle.isOn)
        {
            PlayerPrefs.SetInt("FastMode", 1);
        }
        else
        {
            PlayerPrefs.SetInt("FastMode", 0);
        }
    }

    public void SaveSMAAQuality(Dropdown _dropdown)
    {
        PlayerPrefs.SetInt("SMAAQuality", _dropdown.value);
        LoadGraphicSettings();
    }

    public void SaveJitterSpreadTAA(Slider _slider)
    {
        PlayerPrefs.SetFloat("JitterSpreadTAA", _slider.value);
    }

    public void SaveStationaryBlendingTAA(Slider _slider)
    {
        PlayerPrefs.SetFloat("StationaryBlendingTAA", _slider.value);
    }

    public void SaveMotionBlendingTAA(Slider _slider)
    {
        PlayerPrefs.SetFloat("MotionBlendingTAA", _slider.value);
    }

    public void SaveSharpnessTAA(Slider _slider)
    {
        PlayerPrefs.SetFloat("SharpnessTAA", _slider.value);
    }

    public void CheckSettings(Dropdown _dropdown)
    {
        if (_dropdown.value == 0)
        {
            ToggleAntialisingSettings(false);
        }
        else if (_dropdown.value == 1)
        {
            ToggleAntialisingSettings(false);
            FXAAFastMode.SetActive(true);
        }
        else if (_dropdown.value == 2)
        {
            ToggleAntialisingSettings(false);
            QualitySMAA.SetActive(true);
        }
        else if (_dropdown.value == 3)
        {
            ToggleAntialisingSettings(false);
            TAAJitterSpread.SetActive(true);
            TAAStationaryBlending.SetActive(true);
            TAAMotionBlending.SetActive(true);
            TAASharpness.SetActive(true);
        }
    }

    void ToggleAntialisingSettings(bool toggle)
    {
        FXAAFastMode.SetActive(toggle);
        QualitySMAA.SetActive(toggle);
        TAAJitterSpread.SetActive(toggle);
        TAAStationaryBlending.SetActive(toggle);
        TAAMotionBlending.SetActive(toggle);
        TAASharpness.SetActive(toggle);
    }

    public void ToogleOflineMode(Toggle _toggle)
    {
        if (_toggle.isOn)
        {
            PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.OfflineMode;
            PhotonNetwork.ConnectUsingSettings(PhotonNetworkManager.instance.GameVersion);
        }
        else
        {
            PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.PhotonCloud;
            PhotonNetwork.ConnectUsingSettings(PhotonNetworkManager.instance.GameVersion);
        }
    }

    #region Main Menu
    public void QuitGame()
	{
		Application.Quit ();
	}

	#endregion

	#region Create Server Menu
	public void CreateServer()
	{
        GameManager.instance.GameName = GameNameInputField.text; 												//Set the Gamemanger gamename to the contents of the gamenameinputfield
		GameManager.instance.Ingame = true;			 															//Set our ingame state to true
		RoomOptions roomOptions = new RoomOptions();															//Create a new RoomOptions entry
		roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();								//Create a new photon hashtable
		roomOptions.CustomRoomProperties.Add ("map", GameManager.instance.CurrentMap.MapName);					//Create a entry for the current game map
		roomOptions.CustomRoomProperties.Add ("gm", GameManager.instance.CurrentGameType.GameTypeLoadName);		//Create a entry for the current game type
		roomOptions.CustomRoomProperties.Add ("tl", GameManager.instance.GameTimeLimit);						//Create a entry for the current game time limit
		roomOptions.CustomRoomProperties.Add ("sl", GameManager.instance.GameScoreLimit);						//Create a entry for the current game time limit
		roomOptions.CustomRoomProperties.Add ("maxveh", GameManager.instance.MaxVehicles);						//Create a entry for the maximum amount of vehicles
		roomOptions.CustomRoomPropertiesForLobby = new string[] { "map", "gm" };							    //Make the mapname and gametype name accesable from the room list so it can be displayed in the server browser
		roomOptions.MaxPlayers = GameManager.instance.MaxPlayers;												//Set the player cap for this room to 20
		PhotonNetwork.CreateRoom (GameManager.instance.GameName, roomOptions, null);							//Creates the room with the just created RoomOptions
		GameManager.instance.ToggleLoadScreen(true, GameManager.instance.CurrentMap);
		PhotonNetwork.LoadLevel(GameManager.instance.CurrentMap.MapName);
        int Games = 0;
        if (PlayerPrefs.HasKey("TotalGames"))
        {
            Games = PlayerPrefs.GetInt("TotalGames");
        }
        Games += 1;
        PlayerPrefs.SetInt("TotalGames", Games);
    }

	public void UpdateGameTypeSettings()
	{
		if (GameManager.instance.CurrentGameType.ScoreLimitEnabled == true)
        {
			//ScoreLimitPanel.SetActive (true);
		}
        else
        {
			//ScoreLimitPanel.SetActive (false);
		}
		if (GameManager.instance.CurrentGameType.TimeLimitEnabled == true)
        {
			TimeLimitPanel.SetActive (true);
		}
        else
        {
			TimeLimitPanel.SetActive (false);
		}
	}
	#region Select GameType
	public void NextGameType()
	{
		if (GameManager.instance.GameTypeList.Length > 1 && SelectedGameTypeInt != GameManager.instance.GameTypeList.Length - 1) { //If the length of the gametypelist is greater than 1 and the current index isnt at the length minus one
			SelectedGameTypeInt++;
		}
        else
        {
			SelectedGameTypeInt = 0;
		}
		GameManager.instance.CurrentGameType = GameManager.instance.GameTypeList [SelectedGameTypeInt]; 		//Set the gamemanger currentgametype to the chosen index of the gametypelist
		SelectedGameTypeText.text = GameManager.instance.GameTypeList [SelectedGameTypeInt].GameTypeLoadName; 	//Set the selected gametype text to the chosen gametype
		UpdateGameTypeSettings ();
	}

	public void PrevioustGameType()
	{
		if (GameManager.instance.GameTypeList.Length > 1 && SelectedGameTypeInt != 0) { 						//If the length of the gametypelist is greater than 1 and the current index isnt zero
			SelectedGameTypeInt--; 																				//Subtract 1 to the index
		} else { 																								//If one of the previous conditions is set the false
			SelectedGameTypeInt = GameManager.instance.GameTypeList.Length - 1; 								//Set the index to the current index isnt at the length minus one
		}
		GameManager.instance.CurrentGameType = GameManager.instance.GameTypeList [SelectedGameTypeInt]; 		//Set the gamemanger currentgametype to the chosen index of the gametypelist
		SelectedGameTypeText.text = GameManager.instance.GameTypeList [SelectedGameTypeInt].GameTypeLoadName; 	//Set the selected gametype text to the chosen gametype
		UpdateGameTypeSettings ();
	}
	#endregion

	#region Select Map
	public void NextMap()
	{
		if (GameManager.instance.MapList.Length > 1 && SelectedMapInt != GameManager.instance.MapList.Length - 1)
        {
			SelectedMapInt++;
		}
        else
        {
			SelectedMapInt = 0;
		}
		GameManager.instance.CurrentMap = GameManager.instance.MapList [SelectedMapInt];
		SelectedMapText.text = GameManager.instance.MapList [SelectedMapInt].MapName;
		//SelectedMapPreview.sprite = GameManager.instance.MapList [SelectedMapInt].MapLoadImage;
	}

	public void PrevioustMap()
	{
		if (GameManager.instance.MapList.Length > 1 && SelectedMapInt != 0)
        {
			SelectedMapInt--;
		}
        else
        {
			SelectedMapInt = GameManager.instance.MapList.Length - 1;
		}
		GameManager.instance.CurrentMap = GameManager.instance.MapList [SelectedMapInt];
		SelectedMapText.text = GameManager.instance.MapList [SelectedMapInt].MapName;
		//SelectedMapPreview.sprite = GameManager.instance.MapList [SelectedMapInt].MapLoadImage;

	}
	#endregion

	#region Select Time Limit 
	public void NextTimeLimit()
	{
		if (GameManager.instance.TimeLimits.Length > 1 && SelectedTimeLimitInt != GameManager.instance.TimeLimits.Length - 1) { //If the length of the gametypelist is greater than 1 and the current index isnt at the length minus one
			SelectedTimeLimitInt++;	//Add 1 to the index
		} else { //If one of the previous conditions is set the false
			SelectedTimeLimitInt = 0; //Set the index back to zero
		}
		GameManager.instance.GameTimeLimit = GameManager.instance.TimeLimits[SelectedTimeLimitInt]; //Set the gamemanger currentgametype to the chosen index of the gametypelist
		if (SelectedTimeLimitInt != 0) {
			SelectedTimeLimitText.text = GameManager.instance.TimeLimits[SelectedTimeLimitInt].ToString(); //Set the selected gametype text to the chosen gametype
		}
		else {
			SelectedTimeLimitText.text = "Unlim";
		}
	}

	public void PrevioustTimeLimit()
	{
		if (GameManager.instance.TimeLimits.Length > 1 && SelectedTimeLimitInt != 0) { //If the length of the gametypelist is greater than 1 and the current index isnt zero
			SelectedTimeLimitInt--; //Subtract 1 to the index
		} else { //If one of the previous conditions is set the false
			SelectedTimeLimitInt = GameManager.instance.TimeLimits.Length - 1; //Set the index to the current index isnt at the length minus one
		}
		GameManager.instance.GameTimeLimit = GameManager.instance.TimeLimits [SelectedTimeLimitInt]; //Set the gamemanger currentgametype to the chosen index of the gametypelist
		if (SelectedTimeLimitInt != 0) {
			SelectedTimeLimitText.text = GameManager.instance.TimeLimits[SelectedTimeLimitInt].ToString(); //Set the selected gametype text to the chosen gametype
		}
		else {
			SelectedTimeLimitText.text = "Unlim";
		}
	}
	#endregion

	#region Select Score Limit 
	public void NextScoreLimit()
	{
		if (GameManager.instance.ScoreLimits.Length > 1 && SelectedScoreLimitInt != GameManager.instance.ScoreLimits.Length - 1) { //If the length of the gametypelist is greater than 1 and the current index isnt at the length minus one
			SelectedScoreLimitInt++;
		}
        else
        {
			SelectedScoreLimitInt = 0;
		}
		GameManager.instance.GameScoreLimit = GameManager.instance.ScoreLimits[SelectedScoreLimitInt];
		if (SelectedScoreLimitInt != 0) {
			//SelectedScoreLimitText.text = GameManager.instance.ScoreLimits[SelectedScoreLimitInt].ToString(); //Set the selected gametype text to the chosen gametype
		}
		else
        {
			//SelectedScoreLimitText.text = "Unlim";
		}
	}

	public void PrevioustScoreLimit()
	{
		if (GameManager.instance.ScoreLimits.Length > 1 && SelectedScoreLimitInt != 0) {
			SelectedScoreLimitInt--;
		}
        else
        {
			SelectedScoreLimitInt = GameManager.instance.ScoreLimits.Length - 1; //Set the index to the current index isnt at the length minus one
		}
		GameManager.instance.GameScoreLimit = GameManager.instance.ScoreLimits[SelectedScoreLimitInt]; //Set the gamemanger currentgametype to the chosen index of the gametypelist
		if (SelectedScoreLimitInt != 0) {
			//SelectedScoreLimitText.text = GameManager.instance.ScoreLimits[SelectedScoreLimitInt].ToString(); //Set the selected gametype text to the chosen gametype
		}
		else {
			//SelectedScoreLimitText.text = "Unlim";
		}
	}
	#endregion

	#region Select Max Players
	public void IncreaseMaxPlayers()
	{
		if (SelectedMaxPlayersByte < 20) { //If the selected amout of maxplayers is below 20
			SelectedMaxPlayersByte += 2;	//Add 2 to the index
		} else { //If one of the previous conditions is set the false
			SelectedMaxPlayersByte = 4; //Set the index back to zero
		}
		GameManager.instance.MaxPlayers = SelectedMaxPlayersByte; //Set the gamemanager maxplayers to the chosen amount
		SelectedMaxPlayersText.text = SelectedMaxPlayersByte.ToString(); //Set the selected max players amout text to the chosen amount
	}

	public void DecreaseMaxPlayers()
	{
		if (SelectedMaxPlayersByte > 4) { //If the selected amout of maxplayers is below 20
			SelectedMaxPlayersByte -= 2;	//Subtract 2 of the index
		} else { //If one of the previous conditions is set the false
			SelectedMaxPlayersByte = 20; //Set the index to 20
		}
		GameManager.instance.MaxPlayers = SelectedMaxPlayersByte; //Set the gamemanager maxplayers to the chosen amount
		SelectedMaxPlayersText.text = SelectedMaxPlayersByte.ToString(); //Set the selected max players amout text to the chosen amount
	}
	#endregion
	#endregion

	#region Servers Menu
	public void DisplayServerList()
	{
		if (JustLoadedServerList == false)
        {
			ClearServerList ();
            //for (int i = 0; i < GameManager.instance.PhotonAppKeys.Length; i++)
            //{
                //GameManager.instance.Reconnect(i);
                RoomInfo[] ServerList = PhotonNetwork.GetRoomList();
                foreach (RoomInfo room in ServerList)
                {
                    GameObject ServerListEntry = Instantiate(ServerListEntryPrefab, ServerListPanel);
                    ServerListEntry.GetComponent<ServerListItem>().AppID = PhotonNetwork.PhotonServerSettings.AppID;
                    ServerListEntries.Add(ServerListEntry);
                    ServerListEntry.GetComponent<ServerListItem>().Setup(room, JoinServer);
                }
            //}
		}
	}

    public void ConnectToServerById(int ID)
    {
        GameManager.instance.Reconnect(ID);
    }

    public void JoinServer(RoomInfo roomtojoin)
	{
		PhotonNetwork.JoinRoom (roomtojoin.Name);
		GameManager.instance.ToggleLoadScreen(true, null);
        int Games = 0;
        if (PlayerPrefs.HasKey("TotalGames"))
        {
            Games = PlayerPrefs.GetInt("TotalGames");
        }
        Games += 1;
        PlayerPrefs.SetInt("TotalGames", Games);
	}

	void ClearServerList()
	{
		foreach (GameObject ServerListEntry in ServerListEntries) {
			Destroy (ServerListEntry);
		}
		ServerListEntries.Clear ();
	}

	public void JoinRandomServer()
	{
		PhotonNetwork.JoinRandomRoom (); //Joins a random room
		GameManager.instance.ToggleLoadScreen(true, null);
	}
	#endregion

	#region Settings Menu
	public void SetResolution(int resolutionindex)
	{
        Screen.SetResolution(res[dropDown.value].width, res[dropDown.value].height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resol", dropDown.value);
    }

	public void SetQuality(int QualityIndex)
	{
		QualitySettings.SetQualityLevel (QualityIndex);
	}

	public void SetFullScreen(bool isFullScreen)
	{
		Screen.fullScreen = isFullScreen;
	}

	public void SetPlayername(string name)
	{
		PhotonNetwork.playerName = name;
		PlayerPrefs.SetString ("PlayerName", name);
	}
	#endregion

	#region Icon Workshop
	public void LoadIconWorkShop()
	{
		PhotonNetwork.Disconnect ();
		SceneManager.LoadScene ("Icon_Workshop");
	}
	#endregion
}
