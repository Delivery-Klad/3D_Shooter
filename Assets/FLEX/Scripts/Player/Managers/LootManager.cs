using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class LootManager : MonoBehaviour
{
    public static LootManager instance;
    [Header("Точки спавна оружия")]
    public GameObject[] LootSpawns;
    [Header("Точки спавна патронов")]
    public GameObject[] AmmoSpawns;
    [Header("Точки спавна аптечек")]
    public GameObject[] HPSpawns;
    [Header("Точки спавна модулей")]
    public GameObject[] ModuleSpawns;
    //[Header("Родительские объекты")]
    //public Transform LootParent;
    //public GameObject ModuleParent;

    void Awake()
    {
        instance = this;
    }

    public void Spawn()
    {
        LootSpawns = GameObject.FindGameObjectsWithTag("LootSpawn");
        AmmoSpawns = GameObject.FindGameObjectsWithTag("AmmoSpawn");
        HPSpawns = GameObject.FindGameObjectsWithTag("HPSpawn");
        ModuleSpawns = GameObject.FindGameObjectsWithTag("ModuleSpawn");
        for (int i = 0; i < LootSpawns.Length; i++)
        {
            int RandomLoot = Random.Range(0, GameManager.instance.LootPrefabs.Length);
            SpawnLoot(i, RandomLoot);
        }
        for (int i = 0; i < AmmoSpawns.Length; i++)
        {
            int RandomAmmo = Random.Range(0, GameManager.instance.AmmoPrefabs.Length);
            SpawnAmmo(i, RandomAmmo);
        }
        for (int i = 0; i < HPSpawns.Length; i++)
        {
            int RandomHP = Random.Range(0, GameManager.instance.HPPrefabs.Length);
            SpawnHP(i, RandomHP);
        }
        for (int i = 0; i < ModuleSpawns.Length; i++)
        {
            int RandomModule = Random.Range(0, GameManager.instance.ModulesPrefabs.Length);
            SpawnModule(i, RandomModule);
        }
    }
    void SpawnLoot(int index, int loottype)
    {
        PhotonNetwork.InstantiateSceneObject(GameManager.instance.LootPrefabs[loottype].name, LootSpawns[index].transform.position, LootSpawns[index].transform.rotation, 0, null);
    }
    void SpawnAmmo(int index, int Ammotype)
    {
        PhotonNetwork.InstantiateSceneObject(GameManager.instance.AmmoPrefabs[Ammotype].name, AmmoSpawns[index].transform.position, AmmoSpawns[index].transform.rotation, 0, null);
    }
    void SpawnHP(int index, int HPtype)
    {
        PhotonNetwork.InstantiateSceneObject(GameManager.instance.HPPrefabs[HPtype].name, HPSpawns[index].transform.position, HPSpawns[index].transform.rotation, 0, null);
    }
    void SpawnModule(int index, int Moduletype)
    {
        PhotonNetwork.InstantiateSceneObject(GameManager.instance.ModulesPrefabs[Moduletype].name, ModuleSpawns[index].transform.position, ModuleSpawns[index].transform.rotation, 0, null);
    }
}
