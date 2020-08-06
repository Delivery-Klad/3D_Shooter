using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Trigger : MonoBehaviour
{
	public string TriggerAction;
	public int TriggerIndex;
	public bool Triggered = false;
	public string HintString;

	public void TriggerInteractable()
	{
		if (Triggered == false)
        {
			GameManager.instance.gameObject.GetComponent<PhotonView> ().RPC (TriggerAction, PhotonTargets.AllBuffered, TriggerIndex);
			GameManager.instance.gameObject.GetComponent<PhotonView> ().RPC ("SyncInteractableState", PhotonTargets.AllBuffered, gameObject.name, true);
		}
	}
}
