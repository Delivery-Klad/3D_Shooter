using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadShadows : MonoBehaviour
{
    [SerializeField] Light MainLight;

    void Start()
    {
        if (PlayerPrefs.HasKey("Shadows"))
        {
            int index = PlayerPrefs.GetInt("Shadows");
            if (index == 0)
            {
                MainLight.shadows = LightShadows.None;
            }
            else if (index == 1)
            {
                MainLight.shadows = LightShadows.Soft;
            }
            else if (index == 2)
            {
                MainLight.shadows = LightShadows.Hard;
            }
        }
        if (PlayerPrefs.HasKey("ShadowsQuality"))
        {
            int index = PlayerPrefs.GetInt("ShadowsQuality");
            if (index == 0)
            {
                MainLight.shadowResolution = (UnityEngine.Rendering.LightShadowResolution)ShadowResolution.Low;
            }
            else if (index == 1)
            {
                MainLight.shadowResolution = (UnityEngine.Rendering.LightShadowResolution)ShadowResolution.Medium;
            }
            else if (index == 2)
            {
                MainLight.shadowResolution = (UnityEngine.Rendering.LightShadowResolution)ShadowResolution.High;
            }
            else if (index == 3)
            {
                MainLight.shadowResolution = (UnityEngine.Rendering.LightShadowResolution)ShadowResolution.VeryHigh;
            }
        }
    }
}
