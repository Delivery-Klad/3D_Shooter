using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knock : MonoBehaviour
{
    public AudioSource aud;
    public AudioClip hover;
    public AudioClip click;
    public void hovers()
    {
        aud.PlayOneShot(hover);
    }
    public void clicks()
    {
        aud.PlayOneShot(click);
    }
}
