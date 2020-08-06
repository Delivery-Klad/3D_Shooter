using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootWeapon : MonoBehaviour
{
    public int id;
    public string WeaponName;
    public PhotonView PV;

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
}
