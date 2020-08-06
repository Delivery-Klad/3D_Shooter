using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodTr : MonoBehaviour
{


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("PlayerEnterWood2", SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("PlayerExitWood2", SendMessageOptions.DontRequireReceiver);
        }
    }
}