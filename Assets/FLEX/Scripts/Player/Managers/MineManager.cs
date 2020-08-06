using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class MineManager : MonoBehaviour
{
    public static MineManager instance;
    public GameObject[] MineSpawns;

    void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        MineSpawns = GameObject.FindGameObjectsWithTag("MineSpawn");
        for (int j = 0; j < MineSpawns.Length; j++)
        {
            int spawn = Random.Range(0, 5);
            if (spawn == 3)
            {
                int RandomMine = Random.Range(0, GameManager.instance.MinePrefabs.Length);
                SpawnMines(j, RandomMine);
            }
        }
    }

    public void SpawnMines(int index, int minetype)
    {
        PhotonNetwork.InstantiateSceneObject(GameManager.instance.MinePrefabs[minetype].name, MineSpawns[index].transform.position, MineSpawns[index].transform.rotation, 0, null);
    }
}
