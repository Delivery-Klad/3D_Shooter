using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractables : MonoBehaviour {

	public PhotonView PlayerPhotonView;
	public bool PlayerInTrigger = false;
	public Collider CurrentTrigger;

	void Update () {
		OnPlayerSuicide ();
	}
    
	void OnPlayerSuicide()
	{
		if (PlayerInputManager.instance.Suicide && GameManager.instance.IsAlive)
        {		
			PlayerPhotonView.RPC ("ApplyPlayerDamage", PhotonTargets.All, 999, "World", PhotonNetwork.player, 1f, true);    //Deal 999 damage to ourself with the world as source
            int Deaths = 0;
            if (PlayerPrefs.HasKey("TotalDeaths"))
            {
                Deaths = PlayerPrefs.GetInt("TotalDeaths");
            }
            Deaths += 1;
            PlayerPrefs.SetInt("TotalDeaths", Deaths);
        }
	}

	void PlayerHurtTrigger(int dmg)
	{
		if (dmg != 0)
        {
			PlayerPhotonView.RPC ("ApplyPlayerDamage", PhotonTargets.All, dmg, "HurtTrigger", PhotonNetwork.player, 1f, true);  //Deal 999 damage to ourself with the hurttrigger as source
            int Deaths = 0;
            if (PlayerPrefs.HasKey("TotalDeaths"))
            {
                Deaths = PlayerPrefs.GetInt("TotalDeaths");
            }
            Deaths += 1;
            PlayerPrefs.SetInt("TotalDeaths", Deaths);
        }
	}

	void PlayerLandMine()
	{
		PlayerPhotonView.RPC ("ApplyPlayerDamage", PhotonTargets.All, 999, "Mine", PhotonNetwork.player, 1f, true);	//Deal 999 damage to ourself 
		PlayerPhotonView.RPC ("PlayFXAtPosition", PhotonTargets.All, 0, this.transform.position, Vector3.zero);
        int Deaths = 0;
        if (PlayerPrefs.HasKey("TotalDeaths"))
        {
            Deaths = PlayerPrefs.GetInt("TotalDeaths");
        }
        Deaths += 1;
        PlayerPrefs.SetInt("TotalDeaths", Deaths);
    }

    void Console()
    {
        PlayerPhotonView.RPC("ApplyPlayerDamage", PhotonTargets.All, 999, "Console", PhotonNetwork.player, 1f, true);
        int Deaths = 0;
        if (PlayerPrefs.HasKey("TotalDeaths"))
        {
            Deaths = PlayerPrefs.GetInt("TotalDeaths");
        }
        Deaths += 1;
        PlayerPrefs.SetInt("TotalDeaths", Deaths);
    }
}
