using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootWeapon : MonoBehaviour
{
    public int id;
    public string WeaponName;
    public PhotonView PV;
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip Collision;

    [PunRPC]
    void RemoveGun(bool put)
    {
        if (put)
        {
            if (PV.isMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _audio.PlayOneShot(Collision);
    }
}
