using UnityEngine;

public class Take_HP : MonoBehaviour
{
    public int CountHP = 30;
    public int maxPlayerHP = 100;
    public PhotonView PV;

    [PunRPC]
    void RemoveHP(bool put)
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
