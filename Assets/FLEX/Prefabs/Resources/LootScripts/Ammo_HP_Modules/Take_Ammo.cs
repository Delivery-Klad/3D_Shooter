using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Take_Ammo : MonoBehaviour
{
    public int CountAmmo = 30;
    public PhotonView PV;

    [PunRPC]
    void RemoveAmmo(bool put)
    {
        if (put == true)
        {
            if (PV.isMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
