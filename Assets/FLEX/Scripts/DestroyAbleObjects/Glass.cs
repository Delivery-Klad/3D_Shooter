using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    public GameObject MainGlass;
    public GameObject BreakedGlass;
    public int HP = 100;
    public bool Has2Textures = false;
    private PhotonPlayer Attacker;
    private PhotonView PV;

    private void Start()
    {
        PV = gameObject.GetComponent<PhotonView>();
    }

    void CheckHP()
    {
        if (HP <= 0)
        {
            if (Has2Textures == true)
            {
                BreakedGlass.SetActive(true);
            }
            if (PV.isMine)
            {
                PhotonNetwork.Destroy(MainGlass);
            }
            Attacker.AddGlass();
        }
    }

    [PunRPC]
    public void ApplyDamage(int dmg, PhotonPlayer attacker)
    {
        if (HP > 0)
        {  
            HP -= Mathf.RoundToInt(dmg * 1.0f);
        }
        Attacker = attacker;
        CheckHP();
    }
}
