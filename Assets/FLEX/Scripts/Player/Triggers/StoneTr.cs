using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneTr : MonoBehaviour
{


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("PlayerEnterStone2", SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("PlayerExitStone2", SendMessageOptions.DontRequireReceiver);
        }
    }
}
