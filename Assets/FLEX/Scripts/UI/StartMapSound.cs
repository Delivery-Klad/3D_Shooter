using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class StartMapSound : MonoBehaviour
{
    [SerializeField] AudioMixerGroup Mixer;
    float i = 0.0f;

    void Start()
    {
        StartCoroutine("Sound");
    }

    IEnumerator Sound()
    {
        yield return new WaitForSeconds(5);
        Mixer.audioMixer.GetFloat("Effects", out i);
        Mixer.audioMixer.SetFloat("Mute", i);
    }
}
