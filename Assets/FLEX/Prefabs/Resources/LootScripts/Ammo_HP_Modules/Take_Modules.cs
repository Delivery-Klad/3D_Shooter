using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Take_Modules : MonoBehaviour
{
    public bool Muzzle_break = false;
    public bool Silencer = false;
    public bool ACOG = false;
    public bool Red_dot = false;

    [PunRPC]
    void RemoveModule(bool put)
    {
        if (put == true)
        {
            Destroy(gameObject);
        }
    }
}
