using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is written by paultricklebank
/// Class which deals with applying damage to a destroyable object.
/// you MUST have a collider attached to the same child object as this script.
/// </summary>
/// 
[RequireComponent(typeof(PhotonView))]


public class DestroyableObject : MonoBehaviour {

    
    public float health = 100;
    public bool playRandomDeathEffect = false;
    public float TimeToDestroyEffect = 5f;                                                
    public GameObject[] DeathEffect = null;
    public GameObject AudioPlayer;
    public bool playdeathAudio = false;
    private AudioSource mySource = null;
    public AudioClip deathClip = null;
    public float maxDistanceToHearAudio = 100f;



    void Start()
    {
      
    }
    /// <summary>
    /// Apply damage to the GameObject this is attached to.
    /// </summary>
    /// <param name="damage"></param>
    [PunRPC]
    public void ApplyDamage(int damage)
    {
        health = health - damage;
        if (health < 0)
        {
            health = 0;
			DestroyObject();
        }
    }
    void DestroyObject()
    {
        if (playRandomDeathEffect == true)
        {
            int i = Random.Range(0, DeathEffect.Length);
            Instantiate(DeathEffect[i], this.transform.position, this.transform.rotation);
        }
		else 
        {
            foreach (GameObject go in DeathEffect)
            {
                Instantiate(go, this.transform.position, this.transform.rotation);
            }
        }
        if (playdeathAudio == true)
        {
            GameObject go = Instantiate(AudioPlayer, this.transform.position, this.transform.rotation);
            mySource = go.GetComponent<AudioSource>();
            mySource.spatialBlend = 1;
            mySource.rolloffMode = AudioRolloffMode.Custom;
            mySource.maxDistance = maxDistanceToHearAudio;
            mySource.PlayOneShot(deathClip, 0.5f);
            Destroy(go, deathClip.length);
        }
       	
		if (PhotonNetwork.isMasterClient)
        {
			PhotonNetwork.Destroy (this.gameObject);
		}
    }
}
