using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCrosshair : MonoBehaviour
{
    [HideInInspector] public static DynamicCrosshair instance;
    public float CurrentSpread = 40;
    public float SpreadSpeed;

    public Parts[] parts;

    float temp;
    float curSpread;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        CrosshairUpdate();
        if (PlayerInputManager.instance.Sprint)
        {
            CurrentSpread = 80.0f;
        }
    }

    void CrosshairUpdate()
    {
        temp = 0.005f * SpreadSpeed;
        curSpread = Mathf.Lerp(curSpread, CurrentSpread, temp);

        for (int i = 0; i < parts.Length; i++)
        {
            Parts part = parts[i];
            part.transform.anchoredPosition = part.position * curSpread;
        }
    }

    [System.Serializable]
    public class Parts
    {
        public RectTransform transform;
        public Vector2 position;
    }
}
