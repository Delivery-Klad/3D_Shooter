using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalTrigger : MonoBehaviour
{


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("PlayerEnterMetal", SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("PlayerExitMetal", SendMessageOptions.DontRequireReceiver);
        }
    }
}