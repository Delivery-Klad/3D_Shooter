using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class VehicleManager : MonoBehaviour {

	public static VehicleManager instance;
	//public List<VehicleStats> CurrentVehicles = new List<VehicleStats>();
	public const string VehicleCurrentAmountProp = "currentveh";
	public const string VehicleMaxAmountProp = "maxveh";
	public string VehicleEnterHinstring;
	public GameObject[] VehicleSpawns;

	void Awake()
	{
		instance = this;
	}

	public void Init()
	{
		VehicleSpawns = GameObject.FindGameObjectsWithTag ("VehicleSpawn");
		for (int i = 0; i < VehicleSpawns.Length; i++) {
			int RandomVehicle = Random.Range (0, GameManager.instance.VehiclePrefabs.Length);
			SpawnVehicle (i, RandomVehicle);
		}
	}

	public void SpawnVehicle(int index, int vehicletype)
	{
		PhotonNetwork.InstantiateSceneObject (GameManager.instance.VehiclePrefabs[vehicletype].name, VehicleSpawns [index].transform.position, VehicleSpawns [index].transform.rotation, 0, null);
	}
}