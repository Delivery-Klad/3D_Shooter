using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonNetworkManager : MonoBehaviour {

	[HideInInspector]public static PhotonNetworkManager instance;
	[Header("Photon Networking Settings")]
	public string GameVersion = "";					//The current gameversion we are playing at, is used for determining the server list

	void Awake()
	{
		if (instance != null)
        {
			DestroyImmediate (gameObject);
		}
        else
        {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}	
	}

	public void OnJoinedLobby() //When we have connected to the photonservices
	{
		MainMenuUI.instance.MainMenuPanel.SetActive (true); //Show the main menu
		PhotonNetwork.automaticallySyncScene = true;		//Automatically syncs the scene only when entering a new room
	}

	public void OnJoinedRoom()	//Called on ourself when we join a room
	{
		GameManager.instance.GameTimeLimit = float.Parse(PhotonNetwork.room.CustomProperties ["tl"].ToString()); //Sync the time limit to the other clients
		foreach (PhotonPlayer player in PhotonNetwork.otherPlayers)
        {
			if (player.NickName == PhotonNetwork.player.NickName)
            {		//If there is a player already with the same name as our player
				PhotonNetwork.player.NickName = PhotonNetwork.player.NickName + Random.Range(0,99).ToString(); //Change the name of our player to something unique
			}
		}
		StartCoroutine (WaitTillGameTypeDeclared());
	}

	IEnumerator WaitTillGameTypeDeclared () {
		while ( PhotonNetwork.room.CustomProperties ["gm"].ToString () == null)
        {
			yield return new WaitForSeconds(0.1f);
		}
		GameManager.instance.GetCurrentGameType();
	}

	public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged) //This function is called when the custom properties of a room is being changed
	{
		if (propertiesThatChanged.ContainsKey("StartTime"))
		{
			GameManager.instance.MatchActive = true;
			GameManager.instance.StartTime = (int) propertiesThatChanged["StartTime"];
		}
	}

	public void OnGameError(string reason)
	{
		if (GameManager.instance.InVehicle && GameManager.instance.CurrentVehicle != null)
        {
			//GameManager.instance.CurrentVehicle.GetComponent<VehicleStats> ().VehiclePhotonView.RPC ("OnPlayerExit", PhotonTargets.AllBuffered, PhotonNetwork.player, GameManager.instance.CurrentVehicle.GetComponent<VehicleStats> ().GetPlayerCurrentSeatIndex ());
		}
		Debug.LogError (reason);
		GameManager.instance.Ingame = false;
		GameManager.instance.ClearMatchSettings ();
		PhotonNetwork.Disconnect ();
	}

	public void OnLeftRoom()
	{
		GameManager.instance.ResetPlayerStats ();
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		GameManager.instance.AddGameMessage ("<color=green>" + player.NickName + "</color> joined the game!");
	}

	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		GameManager.instance.AddGameMessage ("<color=red>" + player.NickName + "</color> left the game!");
	}
}
