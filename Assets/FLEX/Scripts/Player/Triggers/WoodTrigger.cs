using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodTrigger : MonoBehaviour
{


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("PlayerEnterWood", SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("PlayerExitWood", SendMessageOptions.DontRequireReceiver);
        }
    }
}