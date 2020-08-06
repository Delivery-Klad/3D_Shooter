using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationSystem : MonoBehaviour
{
    public Text NotificationMessages;

    private void Start()
    {
        NotificationMessages.text = "";
    }

    public void AddMessage(string text)
    {
        StartCoroutine(ShowMessage(text));
    }

    public IEnumerator ShowMessage(string text)
    {
        NotificationMessages.text = text;
        yield return new WaitForSeconds(2);
        NotificationMessages.text = "";
    }
}
