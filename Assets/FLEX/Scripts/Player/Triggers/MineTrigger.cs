using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTrigger : MonoBehaviour {
    public AudioSource Audio;
    public AudioClip Exp;
    public GameObject Mine;
    public bool Delite = false;
    public int DestroyTimer = 0;
    void Update()
    {
        if(Delite == true)
        {
            DestroyTimer += 1;
        }
        if(DestroyTimer == 60)
        {
            PhotonNetwork.Destroy(Mine);
            Destroy(Mine);
        }
    }
    void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			other.SendMessage("PlayerLandMine",SendMessageOptions.DontRequireReceiver);
            Audio.PlayOneShot(Exp);
            Delite = true;
        }
	}
}
