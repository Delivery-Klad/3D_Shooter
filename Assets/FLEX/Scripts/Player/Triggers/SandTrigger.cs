using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandTrigger : MonoBehaviour
{


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("PlayerEnterSand", SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("PlayerExitSand", SendMessageOptions.DontRequireReceiver);
        }
    }
}