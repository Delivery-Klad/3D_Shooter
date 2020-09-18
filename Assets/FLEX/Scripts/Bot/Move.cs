using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Move : MonoBehaviour
{
    public Transform[] colliders;
    public string NameOfBot = "NPC";
    public int HP = 100;
    public int DeathTimer = 0;
    public GameObject[] player;
    public float[] distance;
    NavMeshAgent nav;
    public int target = -1;
    public bool Fire = false;
    public float Radius = 25;
    public bool Agr = false;
    public int temp = -20;
    public int Damage = 10;
    public int BanTimer = 0;
    public bool Dead = false;
    private int StepTimer = 0;
    public float RunSpeed = 4f;
    public float WalkSpeed = 0.5f;
    public Transform[] Ways;
    System.Random rnd = new System.Random();
    private int Way;
    public int Hits = 0;
    private PhotonView PV;
    private Animator AN;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        PV = GetComponent<PhotonView>();
        AN = GetComponent<Animator>();
    }

    void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            PV.RPC("RpcSendTransform", PhotonTargets.AllBuffered, gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            PV.RPC("RpcSendRotation", PhotonTargets.AllBuffered, gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z, gameObject.transform.rotation.w);
        }
        player = GameObject.FindGameObjectsWithTag("Player");
        distance = new float[20];
        for (int i = 0; i < player.Length; i++)
        {
            distance[i] = Vector3.Distance(player[i].transform.position, transform.position);
            if (temp >= 0)
            {
                if (distance[temp] > Radius)
                {
                    target = -1;
                    Agr = false;
                    Fire = false;
                }
            }
            if (distance[i] < Radius && distance[i] > 2.8)
            {
                if (target == -1 && Agr == false)
                {
                    Agr = true;
                    target = i;
                    temp = i;
                    Fire = false;
                }
                else if (Agr == true && i == temp)
                {
                    Fire = false;
                    target = temp;
                }
            }
            if (distance[i] < Radius && distance[i] < 2)
            {
                if (target != -1 && target == i) ////////
                {
                    target = -1;
                    Fire = true;
                }
            }
            if (target != -1 && Dead != true)
            {
                nav.speed = RunSpeed;
                nav.enabled = true;
                nav.SetDestination(player[target].transform.position);
                AN.SetTrigger("Run");
            }
            if (Fire == true && Dead != true)
            {
                nav.enabled = false;
                AN.SetTrigger("Attack");
                BanTimer += 1;
                if (BanTimer % 70 == 0)
                {
                    player[temp].transform.root.GetComponent<PhotonView>().RPC("ApplyPlayerDamage", PhotonTargets.All, Damage, NameOfBot, PhotonNetwork.player, 1.0f, false);
                }
            }
            //if (target == -1 && Fire == false)
            //{
            //    nav.enabled = false;
            //    gameObject.GetComponent<Animator>().SetTrigger("Idle");
            //}
        }
        if (Agr == false && Dead != true)
        {
            nav.speed = WalkSpeed;
            nav.enabled = true;
            if (PhotonNetwork.isMasterClient)
            {
                if (StepTimer % 600 == 0)
                {
                    Way = rnd.Next(0, Ways.Length - 1);
                }
                StepTimer += 1;
            }
            AN.SetTrigger("Walk");
            nav.SetDestination(Ways[Way].transform.position);
        }
        if (HP <= 0)
        {
            Dead = true;
            nav.enabled = false;
            AN.enabled = false;
            foreach(Transform part in colliders)
            {
                part.GetComponent<Rigidbody>().isKinematic = false;
            }
            DeathTimer += 1;

            if (DeathTimer >= 1800)
            {
                PhotonNetwork.Destroy(this.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    [PunRPC]
    public void ApplyBotDamage(int dmg, string source, PhotonPlayer attacker, float dmgmod, bool SelfInflicted)
    {
        if (HP > 0)
        {
            HP -= Mathf.RoundToInt(dmg * dmgmod);
        }
    }

    [PunRPC]
    private void RpcSendTransform(float x, float y, float z)
    {
        Vector3 takenPos = new Vector3(x, y, z);
        gameObject.transform.position = takenPos;
    }

    [PunRPC]
    private void RpcSendRotation(float x, float y, float z, float w)
    {
        Quaternion takenRot = new Quaternion(x, y, z, w);
        gameObject.transform.rotation = takenRot;
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.isWriting)
    //    {
    //        stream.SendNext(Way);
    //    }
    //    else if (stream.isReading)
    //    {
    //        Way = (int)stream.ReceiveNext();
    //    }
    //}
}