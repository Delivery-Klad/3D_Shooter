using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PlayerStats : MonoBehaviour
{

    [Header("Player Attributes")]
    public int PlayerHealth = 100;
    public float PlayerStamina = 100.0f;
    public bool isAlive = true;
    public PunTeams.Team PlayerTeam;
    public int CurrentGunId;
    public int CurrentPistolId;
    public int NatoAmmo;
    public int RifleAmmo;
    public int PistolAmmo;
    [HideInInspector] public bool GodMode = false;

    [Header("Player Sound Effects")]
    [Range(0f, 1f)]
    public float FootStepVolume = 0.5f;
    public float WaterStepVolume = 1.0f;
    public float SandStepVolume = 1.0f;
    public float WoodStepVolume = 1.0f;
    public float MetalStepVolume = 1.0f;
    public float StoneStepVolume = 1.0f;
    public AudioSource FootstepAudiosource;
    public AudioClip[] FootstepSounds;
    public AudioClip FootStepLandSound;
    public float StepInterval;
    public float WoodStepInterval;
    public float StoneStepInterval;
    public float SandStepInterval;
    public float MetalStepInterval;
    public float WaterStepInterval;
    public float RunMultiplier = 1;
    public float StepTimer;
    public AudioClip[] WaterstepSounds;
    public AudioClip PlayerWaterEnterSound;
    public AudioClip[] SandstepSounds;
    public AudioClip[] WoodstepSounds;
    public AudioClip[] MetalstepSounds;
    public AudioClip[] StonestepSounds;
    [Space(10)]
    [Range(0f, 1f)]
    public float GearSoundVolume = 0.4f;
    public AudioClip GearPlayerLandSound;
    public AudioSource PlayerGearAudioSource;
    [Space(10)]
    [Range(0f, 1f)]
    public float BreathSoundVolume = 0.4f;
    public AudioSource PlayerBreathAudioSource;
    public AudioClip[] PlayerSprintBreathSounds;
    [Range(0f, 3f)]
    public float SprintBreathInterval;
    public float SprintBreathTimer;

    [Header("Player References")]
    public Text PlayerNameText;
    public CharacterController PlayerCharacterController;
    public PlayerMovementController PlayerMovementController;
    public PlayerWeaponManager PlayerWeaponManager;
    public PlayerThirdPersonController PlayerThirdPersonController;
    public PhotonView PlayerPhotonView;
    public PhotonVoiceRecorder PVoice;
    private float distance = 0;

    void Start()
    {
        GameManager.instance.LocalPlayer = gameObject;
        if (PlayerPhotonView.isMine)
        {
            PhotonNetwork.player.TagObject = this.gameObject;
            InGameUI.instance.HP.value = PlayerHealth;
            PlayerNameText.gameObject.SetActive(false);
            PlayerPhotonView.RPC("SyncPlayerTeam", PhotonTargets.AllBuffered, PlayerTeam);
        }
        PlayerPrefs.SetFloat("TotalDistance", 0);

        if (PlayerPrefs.HasKey("PostProcessingSetting"))
        {
            PostProcessLayer PPL = gameObject.GetComponent<CameraHolder>().Camera.GetComponent<PostProcessLayer>();
            int index = PlayerPrefs.GetInt("PostProcessingSetting");
            if (index == 0)
            {
                PPL.antialiasingMode = PostProcessLayer.Antialiasing.None;
            }
            else if (index == 1)
            {
                PPL.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                if (PlayerPrefs.HasKey("FastMode"))
                {
                    int Fast = PlayerPrefs.GetInt("FastMode");
                    if (Fast == 1)
                    {
                        PPL.fastApproximateAntialiasing.fastMode = true;
                    }
                    else
                    {
                        PPL.fastApproximateAntialiasing.fastMode = false;
                    }

                }
            }
            else if (index == 2)
            {
                PPL.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                if (PlayerPrefs.HasKey("SMAAQuality"))
                {
                    int Quality = PlayerPrefs.GetInt("FasSMAAQualitytMode");
                    if (Quality == 0)
                    {
                        PPL.subpixelMorphologicalAntialiasing.quality = SubpixelMorphologicalAntialiasing.Quality.Low;
                    }
                    else if (Quality == 1)
                    {
                        PPL.subpixelMorphologicalAntialiasing.quality = SubpixelMorphologicalAntialiasing.Quality.Medium;
                    }
                    else if (Quality == 2)
                    {
                        PPL.subpixelMorphologicalAntialiasing.quality = SubpixelMorphologicalAntialiasing.Quality.High;
                    }
                }
            }
            else if (index == 3)
            {
                PPL.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
                if (PlayerPrefs.HasKey("JitterSpreadTAA"))
                {
                    PPL.temporalAntialiasing.jitterSpread = PlayerPrefs.GetFloat("JitterSpreadTAA");
                }
                if (PlayerPrefs.HasKey("StationaryBlendingTAA"))
                {
                    PPL.temporalAntialiasing.stationaryBlending = PlayerPrefs.GetFloat("StationaryBlendingTAA");
                }
                if (PlayerPrefs.HasKey("MotionBlendingTAA"))
                {
                    PPL.temporalAntialiasing.motionBlending = PlayerPrefs.GetFloat("MotionBlendingTAA");
                }
                if (PlayerPrefs.HasKey("SharpnessTAA"))
                {
                    PPL.temporalAntialiasing.sharpness = PlayerPrefs.GetFloat("SharpnessTAA");
                }
            }
        }
    }

    void FixedUpdate()
    {
        CurrentWeapon();
        CurrentPistol();
        if (PlayerPrefs.HasKey("TotalDistance"))
        {
            distance = PlayerPrefs.GetFloat("TotalDistance");
        }
        distance += 1; // не доделано
        PlayerPrefs.SetFloat("TotalDistance", distance);
        if (PlayerInputManager.instance.Speak)
        {
            PVoice.Transmit = true;
            InGameUI.instance.MikImg.SetActive(true);
        }
        else
        {
            PVoice.Transmit = false;
            InGameUI.instance.MikImg.SetActive(false);
        }
    }

    [PunRPC]
    public void SyncPlayerTeam(PunTeams.Team SyncPlayerTeam)
    {
        PlayerTeam = SyncPlayerTeam;
        SetPlayerNameText();
    }

    public void SetPlayerNameText()
    {
        if (PlayerTeam == PunTeams.Team.red || PlayerTeam == PunTeams.Team.none)
        {
            PlayerNameText.text = " ";
            this.gameObject.name = PlayerPhotonView.owner.NickName;
        }
        else if (PlayerTeam == PunTeams.Team.blue)
        {
            PlayerNameText.text = " ";
            this.gameObject.name = PlayerPhotonView.owner.NickName;
        }
    }

    [PunRPC]
    public void ApplyPlayerDamage(int dmg, string source, PhotonPlayer attacker, float dmgmod, bool SelfInflicted)
    {
        if (!GodMode)
        {
            if (attacker.GetTeam() == PunTeams.Team.none || GameManager.instance.CurrentTeam != attacker.GetTeam() || attacker == PhotonNetwork.player || source == "RoadKill")
            {
                if (PlayerHealth > 0)
                {
                    PlayerHealth -= Mathf.RoundToInt(dmg * dmgmod);
                    if (PlayerPhotonView.isMine)
                    {
                        InGameUI.instance.DoHitscreen(1f);
                        if (PlayerHealth <= 0)
                        {
                            PlayerHealth = 0;
                            if (source != "RoadKill")
                            {
                                PlayerPhotonView.RPC("OnPlayerKilled", PhotonTargets.All, source, attacker, SelfInflicted);
                            }
                            else
                            {
                                if (attacker == null)
                                {
                                    PlayerPhotonView.RPC("OnPlayerKilled", PhotonTargets.All, "Crashed", PhotonNetwork.player);
                                }
                                else
                                {
                                    PlayerPhotonView.RPC("OnPlayerKilled", PhotonTargets.All, source, attacker);
                                }
                            }
                            int Deaths = 0;
                            if (PlayerPrefs.HasKey("TotalDeaths"))
                            {
                                Deaths = PlayerPrefs.GetInt("TotalDeaths");
                            }
                            Deaths += 1;
                            PlayerPrefs.SetInt("TotalDeaths", Deaths);
                        }
                        InGameUI.instance.HP.value = PlayerHealth;
                    }
                }
            }
        }
    }

    public void ApplyPlayerHealth()
    {
        InGameUI.instance.HP.value = PlayerHealth;
    }

    [PunRPC]
    public void OnPlayerKilled(string source, PhotonPlayer attacker, bool SelfInflicted)
    {
        this.isAlive = false;
        if (attacker == PhotonNetwork.player && !SelfInflicted && !this.PlayerPhotonView.isMine)
        {
            EventManager.TriggerEvent("OnPlayerKilled");
        }
        if (PlayerPhotonView.isMine && GameManager.instance.IsAlive)
        {
            InGameUI.instance.PlayerUseText.text = "";
            if (attacker.NickName != PhotonNetwork.player.NickName)
            {
                attacker.AddKill(1);
                if (GameManager.instance.CurrentGameType.GameTypeLoadName != "CTF")
                {
                    if (attacker.GetTeam() == PunTeams.Team.red)
                    {
                        EventManager.TriggerEvent("AddRedTeamScore");
                    }
                    else if (attacker.GetTeam() == PunTeams.Team.blue)
                    {
                        EventManager.TriggerEvent("AddBlueTeamScore");
                    }
                }
                int Kills = 0;
                if (PlayerPrefs.HasKey("TotalKills"))
                {
                    Kills = PlayerPrefs.GetInt("TotalKills");
                }
                Kills += 1;
                PlayerPrefs.SetInt("TotalKills", Kills);
            }
            PhotonNetwork.player.AddDeath(1);
            PlayerThirdPersonController.EnableWeaponIK(false);
            GameManager.instance.gameObject.GetComponent<PhotonView>().RPC("AddKillFeedEntry", PhotonTargets.All, attacker.NickName, source, PhotonNetwork.playerName);
            PlayerMovementController.PlayerLegs.SetActive(false);
            PlayerMovementController.enabled = false;
            if (!GameManager.instance.InVehicle)
            {
                PlayerWeaponManager.CurrentWeapon.SetActive(false);
                PlayerWeaponManager.enabled = false;
                PlayerThirdPersonController.ThirdPersonPlayerKilled();
            }
            InGameUI.instance.PlayerHUDPanel.SetActive(false);
            GameManager.instance.IsAlive = false;
            GameManager.instance.InVehicle = false;
            Invoke("PlayerRespawn", GameManager.instance.RespawnDelay);
        }
    }

    void PlayerRespawn()
    {
        if (GameManager.instance.MatchActive)
        {
            EventManager.TriggerEvent("OnPlayerRespawn");
            PhotonNetwork.Destroy(this.gameObject);
            //дописать сброс оружия
        }
    }

    [PunRPC]
    public void PlayFootstepSoundNetwork(string Type)
    {
        if (Type == "Normal")
        {
            FootstepAudiosource.PlayOneShot(FootstepSounds[Random.Range(0, FootstepSounds.Length)], FootStepVolume);
        }
        if (Type == "Water")
        {
            FootstepAudiosource.PlayOneShot(WaterstepSounds[Random.Range(0, WaterstepSounds.Length)], WaterStepVolume);
        }
        if (Type == "Wood")
        {
            FootstepAudiosource.PlayOneShot(WoodstepSounds[Random.Range(0, WoodstepSounds.Length)], WoodStepVolume);
        }
        if (Type == "Wood2")
        {
            FootstepAudiosource.PlayOneShot(WoodstepSounds[Random.Range(0, WoodstepSounds.Length)], WoodStepVolume);
        }
        if (Type == "Sand")
        {
            FootstepAudiosource.PlayOneShot(SandstepSounds[Random.Range(0, SandstepSounds.Length)], SandStepVolume);
        }
        if (Type == "Metal")
        {
            FootstepAudiosource.PlayOneShot(MetalstepSounds[Random.Range(0, MetalstepSounds.Length)], MetalStepVolume);
        }
        if (Type == "Stone")
        {
            FootstepAudiosource.PlayOneShot(StonestepSounds[Random.Range(0, StonestepSounds.Length)], StoneStepVolume);
        }
        if (Type == "Stone2")
        {
            FootstepAudiosource.PlayOneShot(StonestepSounds[Random.Range(0, StonestepSounds.Length)], StoneStepVolume);
        }
    }

    #region Misc
    [PunRPC]
    public void PlayFXAtPosition(int EffectID, Vector3 Position, Vector3 EffectDirection)
    {
        Instantiate(GameManager.instance.IngameEffectsReferences[EffectID], Position, Quaternion.FromToRotation(Vector3.forward, EffectDirection));
    }
    #endregion
    void CurrentWeapon()
    {
        CurrentGunId = GameManager.instance.PlayerPrimaryWeapon;
    }

    void CurrentPistol()
    {
        CurrentPistolId = GameManager.instance.PlayerSecondaryWeapon;
    }

    [PunRPC]
    public void SpawnGun(int Id, Vector3 posi, Quaternion rota)
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.InstantiateSceneObject(GameManager.instance.LootPrefabs[Id].name, posi + new Vector3(0.0f, 1.0f, 0.0f), rota, 0, null);
        }
    }
}
