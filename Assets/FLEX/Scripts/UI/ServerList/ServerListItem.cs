using UnityEngine;
using UnityEngine.UI;

public class ServerListItem : MonoBehaviour
{
    [HideInInspector]public string AppID;
	private RoomInfo Server;
	public delegate void JoinServerDelegate(RoomInfo server);
	private JoinServerDelegate joinServerCallback;

	[SerializeField]private Text ServerNameText;
	[SerializeField]private Text ServerMapNameText;
	[SerializeField]private Text ServerPlayersText;
	[SerializeField]private Text ServerGameTypeText;

	public void Setup(RoomInfo server, JoinServerDelegate joinservercallback)
	{
		Server = server;
		joinServerCallback = joinservercallback;

		ServerNameText.text = Server.Name;
		ServerPlayersText.text = Server.PlayerCount + "/" + Server.MaxPlayers;
		ServerGameTypeText.text = Server.CustomProperties ["gm"].ToString();
		ServerMapNameText.text = Server.CustomProperties ["map"].ToString();
	}

	public void JoinServer()
	{
        PhotonNetwork.PhotonServerSettings.AppID = AppID;
        PhotonNetwork.ConnectUsingSettings(PhotonNetworkManager.instance.GameVersion);
        joinServerCallback.Invoke (Server);
	}

    public void TurnOffBackAudio()
    {
        GameManager.instance.OffAudio();
    }
}
