using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;
    private int _frameRange = 60;
    private int[] _fpsBuffer;
    private int _fpsBufferIndex;

    public int AverageFPS { get; private set; }


    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        if (_fpsBuffer == null || _frameRange != _fpsBuffer.Length)
        {
            InitializeBuffer();
        }

        UpdateBuffer();
        CalculateFps();
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(1, 1, 1, 1.0f);
        float msec = deltaTime * 1000.0f;
        string text = string.Format("{0:0.0} ms ({1:0.} fps) {2} ping", msec, AverageFPS, PhotonNetwork.GetPing());
        GUI.Label(rect, text, style);
    }

    private void InitializeBuffer()
    {
        if (_frameRange <= 0)
        {
            _frameRange = 1;
        }

        _fpsBuffer = new int[_frameRange];
        _fpsBufferIndex = 0;
    }

    private void UpdateBuffer()
    {
        _fpsBuffer[_fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
        if (_fpsBufferIndex >= _frameRange)
        {
            _fpsBufferIndex = 0;
        }
    }

    private void CalculateFps()
    {
        int sum = 0;
        for (int i = 0; i < _frameRange; i++)
        {
            int fps = _fpsBuffer[i];
            sum += fps;
        }
        AverageFPS = sum / _frameRange;
    }
}