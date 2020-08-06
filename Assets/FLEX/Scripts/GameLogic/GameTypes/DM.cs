
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DM : MonoBehaviour
{
	void  OnEnable()
	{
		EventManager.StartListening ("OnPlayerRespawn", OnPlayerRespawn);
		EventManager.StartListening ("OnPlayerSpawn", OnPlayerSpawn);
		EventManager.StartListening ("DisableGameType", DisableGameType);
		EventManager.StartListening ("ShowSpawnMenu", ShowSpawnMenu);
		EventManager.StartListening ("GameTypeEndGame", GameTypeEndGame);
		StartGameLogic ();
	}

	void DisableGameType()
	{
		EventManager.StopListening ("OnPlayerRespawn", OnPlayerRespawn);
		EventManager.StopListening ("OnPlayerSpawn", OnPlayerSpawn);
		EventManager.StopListening ("DisableGameType", DisableGameType);
		EventManager.StopListening ("ShowSpawnMenu", ShowSpawnMenu);
		EventManager.StopListening ("GameTypeEndGame", GameTypeEndGame);
		this.enabled = false;
	}

	void Update()
	{
		if (GameManager.instance.MatchActive)
        {
			HandleIngameTimer ();
			DisplayIngameTimer ();
		}
		ToggleScoreboard ();
	}	

	void StartGameLogic()
	{
		PlayerInputManager.instance.FreezePlayerControls = false;
		GameManager.instance.HasTeam = false;
		GameManager.instance.SetCursorLock (false);
		GameManager.instance.CanSpawn = true;
		GameManager.instance.InVehicle = false;
		InGameUI.instance.ServerNameText.text = PhotonNetwork.room.Name;
		GameManager.instance.GameScoreLimit = int.Parse(PhotonNetwork.room.CustomProperties ["sl"].ToString ());
		for (int i = 0; i < GameManager.instance.MapList.Length; i++)
        {
			if (GameManager.instance.MapList [i].MapName == PhotonNetwork.room.CustomProperties ["map"].ToString ())
            {
				GameManager.instance.CurrentMap = GameManager.instance.MapList [i];
			}
		}

		InGameUI.instance.CurrentMapText.text = GameManager.instance.CurrentMap.MapName;
		InGameUI.instance.ServerGameTypeText.text = GameManager.instance.CurrentGameType.GameTypeFullName;
		if (PhotonNetwork.isMasterClient)
        {
			ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
			ht.Add("StartTime", PhotonNetwork.ServerTimestamp);
			PhotonNetwork.room.SetCustomProperties(ht);
            for (int i = 0; i < GameManager.instance.MapList.Length; i++)
            {
                if (GameManager.instance.MapList[i] == GameManager.instance.CurrentMap)
                {
                    if (GameManager.instance.MapList[i].AllowVehicles == true)
                    {
                        if (GameManager.instance.CanSpawnVehicles == true)
                        {
                            VehicleManager.instance.Init();
                            GameManager.instance.CanSpawnVehicles = false;
                        }
                    }
                    if (GameManager.instance.MapList[i].AllowMines == true)
                    {
                        if (GameManager.instance.CanSpawnMines == true)
                        {
                            MineManager.instance.Init();
                            GameManager.instance.CanSpawnMines = false;
                        }
                    }
                    if (GameManager.instance.MapList[i].AllowLoot == true)
                    {
                        if (GameManager.instance.CanSpawnLoot == true)
                        {
                            LootManager.instance.Spawn();
                            BotManager.instance.Spawn();
                            GameManager.instance.CanSpawnLoot = false;
                        }
                    }
                }
            }
        }
		ShowSpawnMenu ();
	}

	#region Game Type Events
	public void ShowSpawnMenu()
	{
		GameManager.instance.SceneCamera.SetActive (true);
		if (InGameUI.instance.Paused)
        {
			InGameUI.instance.ResumeGame ();
		}
		InGameUI.instance.EnableChatInputfield(false);
		InGameUI.instance.DMSpawnMenu.SetActive (true);
		InGameUI.instance.ServerInfoPanel.SetActive (true);
		GameManager.instance.SetCursorLock(false);
	}
	public void OnPlayerRespawn()
	{
		GameManager.instance.SpawnPlayer (GameManager.instance.CurrentTeam.ToString());
	}

	public void OnPlayerSpawn()
	{
		InGameUI.instance.DMSpawnMenu.SetActive(false);
		InGameUI.instance.ServerInfoPanel.SetActive (false);
		InGameUI.instance.TimerScoreHolder.SetActive (true);
		InGameUI.instance.PlayerHUDPanel.SetActive (true);
		InGameUI.instance.CanPause = true;
		GameManager.instance.IsAlive = true;
		GameManager.instance.CurrentTeam = PhotonNetwork.player.GetTeam ();
		if (!InGameUI.instance.Paused)
        {
			GameManager.instance.SetCursorLock (true);
			PlayerInputManager.instance.FreezePlayerControls = false;
		}
        Invoke("SetPlayerWeapon", 0.1f);
    }

    void SetPlayerWeapon()
    {
        GameObject LocalPlayer = PhotonNetwork.player.TagObject as GameObject;
        LocalPlayer.GetComponent<PlayerWeaponManager>().GiveWeapon(GameManager.instance.PlayerPrimaryWeapon);
        LocalPlayer.GetComponent<PlayerWeaponManager>().GiveWeapon(GameManager.instance.PlayerSecondaryWeapon);
        LocalPlayer.GetComponent<PlayerWeaponManager>().EquipWeapon(0);
    }

    //IEnumerator SetPlayerWeapon(int Primary, int Secondary, float Delay, bool JustSpawned)
    //{
    //    yield return new WaitForSeconds(Delay);
    //    GameObject LocalPlayer = PhotonNetwork.player.TagObject as GameObject;
    //    if (!JustSpawned)
    //    {
    //        LocalPlayer.GetComponent<PlayerWeaponManager>().TakeWeapon(0);
    //    }
    //    LocalPlayer.GetComponent<PlayerWeaponManager>().GiveWeapon(Primary);
    //    if (Secondary != -1)
    //    {
    //        LocalPlayer.GetComponent<PlayerWeaponManager>().GiveWeapon(Secondary);
    //    }
    //    LocalPlayer.GetComponent<PlayerWeaponManager>().EquipWeapon(0);
    //}

    public void GameTypeEndGame()
	{
		GameManager.instance.CanSpawn = false;
		GameManager.instance.IsAlive = false;
		InGameUI.instance.PlayerHUDPanel.SetActive (false);
		GameManager.instance.MatchActive = false;
		GameManager.instance.CurrentTeam =PunTeams.Team.none;
		GameManager.instance.HasTeam = false;
		GameManager.instance.InVehicle = false;
		PlayerInputManager.instance.FreezePlayerControls = true;
		if (PhotonNetwork.isMasterClient)
        {
			StartCoroutine (WaitToLoadNextMap ());
		}
		InGameUI.instance.EndGamePanel.SetActive (true);
	}

	IEnumerator WaitToLoadNextMap()
	{
		yield return new WaitForSeconds (GameManager.instance.EndGameTime);
		PhotonNetwork.RemoveRPCs (GameManager.instance.GameManagerPhotonView);
		GameManager.instance.GameManagerPhotonView.RPC ("LoadNextMap", PhotonTargets.All, "TestMap");
	}
	#endregion

	#region Game Type UI
	void HandleIngameTimer()
	{
		if (GameManager.instance.MatchActive && GameManager.instance.GameTimeLimit != 0)
        {
			GameManager.instance.GameTimeLeft = (GameManager.instance.GameTimeLimit * 60f) - ((PhotonNetwork.ServerTimestamp - GameManager.instance.StartTime) / 1000.0f);
		} 
		if(GameManager.instance.GameTimeLeft < 0)
        {
			GameManager.instance.MatchActive = false;
			GameManager.instance.GameTimeLeft = 0;
			GameManager.instance.GameManagerPhotonView.RPC ("EndGame", PhotonTargets.AllBuffered, "Time limit Reached!");
		}
	}

	void DisplayIngameTimer()
	{
		if (GameManager.instance.GameTimeLimit != 0)
        {
			if (!GameManager.instance.MatchActive)
            {
				return;
			}
            else
            {
				int minutes = Mathf.FloorToInt (GameManager.instance.GameTimeLeft / 60F);
				int seconds = Mathf.FloorToInt (GameManager.instance.GameTimeLeft - minutes * 60);
				InGameUI.instance.IngameTimerText.text = string.Format ("{0:0}:{1:00}", minutes, seconds);
			}
		}
        else
        {
			InGameUI.instance.IngameTimerText.fontSize = 50;
			InGameUI.instance.IngameTimerText.text = "∞";
		}
	}

	void ToggleScoreboard()
	{
		if (PlayerInputManager.instance.Scoreboard)
        {
			if (!InGameUI.instance.ShowScoreBoard)
            {
				ShowScoreBoard ();
				InGameUI.instance.ShowScoreBoard = true;
			}
		}
        else
        {
			if (InGameUI.instance.ShowScoreBoard)
            {
				HideScoreBoard ();
				InGameUI.instance.ShowScoreBoard = false;
			}
		}
	}

	void ShowScoreBoard()
	{
		InGameUI.instance.DeathMatchScoreBoardPanel.SetActive (true);
		InGameUI.instance.GameTypeNameText.text = GameManager.instance.CurrentGameType.GameTypeFullName;
		if (PunTeams.PlayersPerTeam [PunTeams.Team.none].Count != 0)
        {
			foreach (PhotonPlayer DeathMatchPlayer in PunTeams.PlayersPerTeam[PunTeams.Team.none].ToArray())
            {
				GameObject DeathMatchScoreEntry = Instantiate (InGameUI.instance.DeathMatchScoreBoardEntry, InGameUI.instance.DeatMatchScoreBoardEntryAnchor);
				ScoreBoardEntry DeathMatchScoreEntryProps = DeathMatchScoreEntry.GetComponent<ScoreBoardEntry> ();
				DeathMatchScoreEntryProps.PlayerNameText.text = DeathMatchPlayer.NickName;
				DeathMatchScoreEntryProps.PlayerScoreText.text = DeathMatchPlayer.GetScore ().ToString();
				DeathMatchScoreEntryProps.PlayerKillsText.text = DeathMatchPlayer.GetKills ().ToString();
				DeathMatchScoreEntryProps.PlayerDeathsText.text = DeathMatchPlayer.GetDeaths ().ToString();
			}
		}
	}

	void HideScoreBoard()
	{
		GameObject[] ScoreBoardEntries = GameObject.FindGameObjectsWithTag("ScoreBoardEntry");
		foreach (GameObject ScoreEntry in ScoreBoardEntries)
        {
			Destroy (ScoreEntry);
		}
		InGameUI.instance.DeathMatchScoreBoardPanel.SetActive (false);
	}
	#endregion
}
