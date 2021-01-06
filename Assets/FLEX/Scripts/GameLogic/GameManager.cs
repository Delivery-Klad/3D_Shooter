using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{

    [HideInInspector] public static GameManager instance;
    public PhotonView GameManagerPhotonView;
    [Space(10)]

    [Header("Режим игры")]
    public string GameName = "";
    public byte MaxPlayers = 20;
    public MapSettings CurrentMap;
    public GameType CurrentGameType;
    public float GameTimeLimit = 0.1f;
    public float GameScoreLimit = 0.1f;
    public int MaxVehicles = 2;
    public float RespawnDelay = 5f;
    public GameObject BackAudio;

    [Header("Игровые элементы")]
    public GameType[] GameTypeList;
    public float[] TimeLimits;
    public int[] ScoreLimits;
    public MapSettings[] MapList;
    public string[] ServerRotation;
    private int ServerRotationIndex = -1;
    public Weapon[] AllGameWeapons;
    public GameObject[] WeaponModels;
    public GameObject RedTeamPlayer;
    public GameObject BlueTeamPlayer;
    public GameObject NoneTeamPlayer;
    public GameObject[] VehiclePrefabs;
    public GameObject[] MinePrefabs;
    public GameObject[] LootPrefabs;
    public GameObject[] AmmoPrefabs;
    public GameObject[] HPPrefabs;
    public GameObject[] ModulesPrefabs;
    public GameObject[] BotPrefabs;
    public GameObject LoadingPanel;
    public GameObject ErrorMessagePanel;
    public Text ErrorMessageText;
    public GameObject[] IngameEffectsReferences;

    [Header("Стартовое оружие")]
    public int PlayerPrimaryWeapon = 0;
    public int PlayerSecondaryWeapon = 4;

    [Header("Окружающие звуки")]
    public AudioSource AmbientAudioSource;
    public AudioClip[] AmbientAudioclips;

    [Header("Причины смэрти")]
    public string[] FallCause;
    private int FallId;
    public string[] SuicideCause;
    private int SuicideId;
    public string[] MineCause;
    private int MineId;

    [Header("Элементы в игре")]
    public bool MapChanged = false;
    public bool Ingame = false;
    public bool Teambased = false;
    public PunTeams.Team CurrentTeam;
    public bool IsAlive = false;
    public bool InVehicle = false;
    public bool MatchActive = false;
    public bool CanSpawn = false;
    public bool HasTeam = false;
    public bool AllowLoadout = true;
    public bool CanSpawnVehicles = true;
    public bool CanSpawnMines = true;
    public bool CanSpawnLoot = true;
    System.Random RndCause = new System.Random();
    [Header("Настройки Photon")]
    public string[] PhotonAppKeys;

    [Header("Настройки PostProcess")]
    private PostProcessVolume PPV;
    private Bloom _bloom;
    private AutoExposure _autoExposure;
    private MotionBlur _motionBloor;
    private ColorGrading _colorGrading;
    private AmbientOcclusion _ambientOcclusion;

    [Header("Настройки PostProcess")]
    public AudioClip SilSound;

    [HideInInspector] public GameObject[] DeathMatchSpawns;
    [HideInInspector] public GameObject[] RedTeamSpawns;
    [HideInInspector] public GameObject[] BlueTeamSpawns;
    [HideInInspector] public GameObject CurrentVehicle;
    [HideInInspector] public float GameTimeLeft = 1f;
    [HideInInspector] public int StartTime;
    [HideInInspector] public float EndGameTime = 15f;
    [HideInInspector] public GameObject SceneCamera;
    [HideInInspector] public string EndGameReason;
    [HideInInspector] public GameObject LocalPlayer;

    [Header("Настройки отображения статистики")]
    public FPSDisplay disp_1;
    public GameObject disp_2;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnLevelWasFinishedLoading;
        }
        else if (instance != null)
        {
            DestroyImmediate(this.gameObject);
        }
    }

    public void Reconnect(int AppID)
    {
        PhotonNetwork.PhotonServerSettings.AppID = PhotonAppKeys[AppID];
        Debug.Log(PhotonAppKeys[AppID]);
        PhotonNetwork.ConnectUsingSettings(PhotonNetworkManager.instance.GameVersion);
    }

    public void OnAudio()
    {
        BackAudio.SetActive(true);
    }

    public void OffAudio()
    {
        BackAudio.SetActive(false);
    }

    void OnLevelWasFinishedLoading(Scene LoadedScene, LoadSceneMode SceneLoadMode)
    {
        if (LoadedScene.name == "MainMenu")
        {
            PhotonNetwork.ConnectUsingSettings(PhotonNetworkManager.instance.GameVersion);
            SetGameType(0);
            SetMap(0);
            SetTimeLimit(0);
            SetScoreLimit(0);
        }

        if (LoadedScene.name != "MainMenu" && LoadedScene.name != "Icon_Workshop")
        {
            SceneCamera = GameObject.Find("SceneCamera");
            DeathMatchSpawns = GameObject.FindGameObjectsWithTag("DeathMatchSpawn");
            RedTeamSpawns = GameObject.FindGameObjectsWithTag("RedTeamSpawn");
            BlueTeamSpawns = GameObject.FindGameObjectsWithTag("BlueTeamSpawn");
            PPV = GameObject.Find("SceneManager").GetComponent<PostProcessVolume>();
            if (MapChanged == true && PhotonNetwork.inRoom)
            {
                InitGameType();
                MapChanged = false;
            }
            PostProcessingChange();
        }
    }

    void PostProcessingChange()
    {
        PPV.profile.TryGetSettings(out _bloom);
        PPV.profile.TryGetSettings(out _motionBloor);
        PPV.profile.TryGetSettings(out _colorGrading);
        PPV.profile.TryGetSettings(out _ambientOcclusion);
        if (PlayerPrefs.HasKey("BloomSetting"))
        {
            _bloom.intensity.value = PlayerPrefs.GetFloat("BloomSetting");
        }
        else
        {
            _bloom.intensity.value = 1.0f;
        }
        if (PlayerPrefs.HasKey("ShutterAngleSetting"))
        {
            _motionBloor.shutterAngle.value = PlayerPrefs.GetFloat("ShutterAngleSetting");
        }
        else
        {
            _motionBloor.shutterAngle.value = 250.0f;
        }
        if (PlayerPrefs.HasKey("SaturationSetting"))
        {
            _colorGrading.saturation.value = PlayerPrefs.GetFloat("SaturationSetting");
        }
        else
        {
            _colorGrading.saturation.value = 20.0f;
        }
        if (PlayerPrefs.HasKey("ContrastSetting"))
        {
            _colorGrading.contrast.value = PlayerPrefs.GetFloat("ContrastSetting");
        }
        else
        {
            _colorGrading.contrast.value = -5.0f;
        }
        if (PlayerPrefs.HasKey("AmbientSetting"))
        {
            if (PlayerPrefs.GetInt("AmbientSetting") == 1)
            {
                _ambientOcclusion.active = true;
            }
            else
            {
                _ambientOcclusion.active = false;
            }
        }
        else
        {
            _ambientOcclusion.active = true;
        }
    }

    public void SetCursorLock(bool Toggle)
    {
        if (Toggle == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void SetPlayerCameraActiveState(bool Toggle)
    {
        GameObject playerobject = PhotonNetwork.player.TagObject as GameObject;
        playerobject.GetComponent<PlayerMovementController>().PlayerCamera.gameObject.SetActive(Toggle);
    }

    public void ClearMatchSettings()
    {
        EventManager.TriggerEvent("DisableGameType");
        CanSpawnVehicles = true;
        CanSpawnMines = true;
        CanSpawnLoot = true;
        InVehicle = false;
        AllowLoadout = true;
        Teambased = false;
        ResetPlayerStats();
        ClearAmbience();
        GameName = "";
        GameTimeLimit = 0;
        CanSpawn = false;
        CurrentGameType = GameTypeList[0];
        CurrentTeam = PunTeams.Team.none;
        PlayerInputManager.instance.FreezePlayerControls = false;
        SceneCamera.SetActive(true);
        SetCursorLock(false);
    }

    public void SetGameType(int index)
    {
        CurrentGameType = GameTypeList[index];
    }

    public void SetScoreLimit(int index)
    {
        GameScoreLimit = ScoreLimits[index];
    }

    public void SetMap(int index)
    {
        CurrentMap = MapList[index];
    }

    public void SetTimeLimit(int index)
    {
        GameTimeLimit = TimeLimits[index];
    }

    public void ShowError(string ErrorText)
    {
        ErrorMessagePanel.SetActive(true);
        ErrorMessageText.text = ErrorText;
    }

    public void GetCurrentGameType()
    {
        for (int i = 0; i < GameTypeList.Length; i++)
        {
            if (GameTypeList[i].GameTypeLoadName == PhotonNetwork.room.CustomProperties["gm"].ToString())
            {
                CurrentGameType = GameTypeList[i];
            }
        }

        if (CurrentGameType != null)
        {
            InitGameType();
            LoadingPanel.SetActive(false);
        }
    }

    void InitGameType()
    {
        if (CurrentGameType.GameTypeLoadName == "TDM")
        {
            GameTypeList[0].GameTypeBehaviour.enabled = true;
        }
        if (CurrentGameType.GameTypeLoadName == "DM")
        {
            GameTypeList[1].GameTypeBehaviour.enabled = true;
        }
        if (CurrentGameType.GameTypeLoadName == "Test")
        {
            GameTypeList[2].GameTypeBehaviour.enabled = true;
        }
        if (CurrentGameType.GameTypeLoadName == "GG")
        {
            GameTypeList[3].GameTypeBehaviour.enabled = true;
        }
    }

    public void SpawnPlayer(string Team)
    {
        if (Team == "red")
        {
            GameObject RedTeamPlayerObject = PhotonNetwork.Instantiate(GameManager.instance.RedTeamPlayer.name, GameManager.instance.RedTeamSpawns[UnityEngine.Random.Range(0, GameManager.instance.RedTeamSpawns.Length)].transform.position, GameManager.instance.RedTeamSpawns[UnityEngine.Random.Range(0, GameManager.instance.RedTeamSpawns.Length)].transform.rotation, 0);
            RedTeamPlayerObject.GetComponent<PlayerMovementController>().enabled = true;
            RedTeamPlayerObject.GetComponent<PlayerMovementController>().controller.enabled = true;
            GameManager.instance.SceneCamera.SetActive(false);
            RedTeamPlayerObject.GetComponent<PlayerMovementController>().PlayerWeaponHolder.gameObject.SetActive(true);
            RedTeamPlayerObject.GetComponent<PlayerWeaponManager>().enabled = true;
            RedTeamPlayerObject.GetComponent<PlayerInteractables>().enabled = true;
            PhotonNetwork.player.SetTeam(PunTeams.Team.red);
            RedTeamPlayerObject.GetComponent<PlayerStats>().PlayerTeam = PhotonNetwork.player.GetTeam();
        }
        else if (Team == "blue")
        {
            GameObject BlueTeamPlayerObject = PhotonNetwork.Instantiate(GameManager.instance.BlueTeamPlayer.name, GameManager.instance.BlueTeamSpawns[UnityEngine.Random.Range(0, GameManager.instance.BlueTeamSpawns.Length)].transform.position, GameManager.instance.BlueTeamSpawns[UnityEngine.Random.Range(0, GameManager.instance.BlueTeamSpawns.Length)].transform.rotation, 0);
            BlueTeamPlayerObject.GetComponent<PlayerMovementController>().enabled = true;
            BlueTeamPlayerObject.GetComponent<PlayerMovementController>().controller.enabled = true;
            GameManager.instance.SceneCamera.SetActive(false);
            BlueTeamPlayerObject.GetComponent<PlayerMovementController>().PlayerWeaponHolder.gameObject.SetActive(true);
            BlueTeamPlayerObject.GetComponent<PlayerWeaponManager>().enabled = true;
            BlueTeamPlayerObject.GetComponent<PlayerInteractables>().enabled = true;
            PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
            BlueTeamPlayerObject.GetComponent<PlayerStats>().PlayerTeam = PhotonNetwork.player.GetTeam();
        }
        else if (Team == "none")
        {
            GameObject DMPlayerObject = PhotonNetwork.Instantiate(GameManager.instance.BlueTeamPlayer.name, GameManager.instance.DeathMatchSpawns[UnityEngine.Random.Range(0, GameManager.instance.DeathMatchSpawns.Length)].transform.position, GameManager.instance.DeathMatchSpawns[UnityEngine.Random.Range(0, GameManager.instance.DeathMatchSpawns.Length)].transform.rotation, 0);
            DMPlayerObject.GetComponent<PlayerMovementController>().enabled = true;
            DMPlayerObject.GetComponent<PlayerMovementController>().controller.enabled = true;
            GameManager.instance.SceneCamera.SetActive(false);
            DMPlayerObject.GetComponent<PlayerMovementController>().PlayerWeaponHolder.gameObject.SetActive(true);
            DMPlayerObject.GetComponent<PlayerWeaponManager>().enabled = true;
            DMPlayerObject.GetComponent<PlayerInteractables>().enabled = true;
            PhotonNetwork.player.SetTeam(PunTeams.Team.none);
            DMPlayerObject.GetComponent<PlayerStats>().PlayerTeam = PhotonNetwork.player.GetTeam();
        }
        EventManager.TriggerEvent("OnPlayerSpawn");
    }

    [PunRPC]
    void SyncAmbientSoundNetwork(int index)
    {
        ClearAmbience();
        AmbientPlay(index, false, 0.75f);
        AddGameMessage("<color=green>Now playing: </color>" + AmbientAudioclips[index].name);
    }

    public void ToggleLoadScreen(bool Toggle, MapSettings MapToLoad)
    {
        if (MapToLoad != null)
        {
            Debug.Log(MapToLoad.MapName);
        }
        LoadingPanel.SetActive(Toggle);
    }

    public void AmbientPlay(int index, bool Loop, float Volume)
    {
        AmbientAudioSource.clip = AmbientAudioclips[index];
        AmbientAudioSource.loop = Loop;
        AmbientAudioSource.volume = Volume;
        AmbientAudioSource.Play();
    }

    void ClearAmbience()
    {
        AmbientAudioSource.Stop();
        if (AmbientAudioSource.isPlaying)
        {
            AddGameMessage("<color=red>Ambient sound stopped!</color>");
        }
    }

    public void ResetPlayerStats()
    {
        PhotonNetwork.player.SetKills(0);
        PhotonNetwork.player.SetDeaths(0);
        PhotonNetwork.player.SetScore(0);
    }

    public void ResetTeamScores()
    {
        PunTeams.Team.red.SetTeamScore(0);
        PunTeams.Team.blue.SetTeamScore(0);
    }

    [PunRPC]
    void SyncInteractableState(string interactable, bool state)
    {
        GameObject Interactable = GameObject.Find(interactable);
        Interactable.GetComponent<Trigger>().Triggered = true;
    }

    [PunRPC]
    public void EndGame(string Reason)
    {
        EndGameReason = Reason;
        EventManager.TriggerEvent("GameTypeEndGame");
    }

    [PunRPC]
    public void LoadNextMap(string MapToLoad)
    {
        MapChanged = true;
        ClearAmbience();
        GameManager.instance.ResetPlayerStats();
        GameManager.instance.ResetTeamScores();
        EventManager.TriggerEvent("DisableGameType");
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.LoadLevel(GetNextMapFromRotation());
        }
    }

    string GetNextMapFromRotation()
    {
        if (ServerRotation.Length > 1 && ServerRotationIndex != ServerRotation.Length - 1)
        {
            ServerRotationIndex++;
        }
        else
        {
            ServerRotationIndex = 0;
        }
        return ServerRotation[ServerRotationIndex];
    }

    #region Game Messages
    [PunRPC]
    public void AddKillFeedEntry(string attacker, string source, string victim)
    {
        GameObject Killfeedentry = GameObject.Instantiate(InGameUI.instance.KillFeedEntryPrefab, InGameUI.instance.KillfeedPanel.transform);
        if (source == "Falling")
        {
            FallId = RndCause.Next(0, FallCause.Length - 1);
            Killfeedentry.GetComponent<Text>().text = victim + " " + FallCause[FallId];
        }
        else if (source == "World")
        {
            SuicideId = RndCause.Next(0, SuicideCause.Length - 1);
            Killfeedentry.GetComponent<Text>().text = victim + " " + SuicideCause[SuicideId];
        }
        else if (source == "Mine")
        {
            MineId = RndCause.Next(0, MineCause.Length - 1);
            Killfeedentry.GetComponent<Text>().text = victim + " " + MineCause[MineId];
        }
        else
        {
            Killfeedentry.GetComponent<Text>().text = attacker + " [" + source + "] " + victim;
        }
    }

    [PunRPC]
    public void AddChatMessage(string message, PhotonPlayer sender)
    {
        GameObject Chatentry = GameObject.Instantiate(InGameUI.instance.ChatEntryPrefab, InGameUI.instance.ChatPanel.transform);
        Chatentry.GetComponent<Text>().text = sender.NickName + ": " + message;
        Chatentry.GetComponent<Text>().color = sender.GetTeam().GetTeamColor();
    }

    public void AddGameMessage(string message)
    {
        GameObject GameMessageentry = GameObject.Instantiate(InGameUI.instance.KillFeedEntryPrefab, InGameUI.instance.KillfeedPanel.transform);
        GameMessageentry.GetComponent<Text>().text = message;
    }
    #endregion
}

[System.Serializable]
public class MapSettings
{
    public string MapName;
    public Sprite MapLoadImage;
    public bool AllowVehicles = false;
    public bool AllowMines = false;
    public bool AllowLoot = false;
}

[System.Serializable]
public class GameType
{
    public string GameTypeFullName;
    public string GameTypeLoadName;
    public Behaviour GameTypeBehaviour;
    public bool TimeLimitEnabled = true;
    public bool ScoreLimitEnabled = false;
}

[System.Serializable]
public class Weapon
{
    public string WeaponName;
    public WeaponClass WeaponType;
    public GameObject FirstPersonPrefab;
    public GameObject ThirdPersonPrefab;
    public Sprite WeaponIcon;
    public string WeaponDesc;
}

[System.Serializable]
public enum WeaponClass
{
    Rifle,
    Sniper,
    Shotgun,
    Pistol
}

[System.Serializable]
public class SurfaceHit
{
    public string SurfaceHitname;
    public GameObject SurfaceParticle;
}
