using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    public static BotManager instance;
    [Header("Точки спавна NPC")]
    public GameObject[] BotSpawns;

    void Awake()
    {
        instance = this;
    }

    public void Spawn()
    {
        BotSpawns = GameObject.FindGameObjectsWithTag("BotSpawn");
        for (int i = 0; i < BotSpawns.Length; i++)
        {
            int RandomBot = Random.Range(0, GameManager.instance.BotPrefabs.Length);
            SpawnBot(i, RandomBot);
        }
    }

    void SpawnBot(int index, int bottype)
    {
        //PhotonNetwork.InstantiateSceneObject(GameManager.instance.BotPrefabs[bottype].name, BotSpawns[index].transform.position, BotSpawns[index].transform.rotation, 0, null);
    }
}
