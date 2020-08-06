using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotMovementController : MonoBehaviour
{
    public string NameOfBot = "Soldier";
    public int HP = 100;
    public int DeathTimer = 0;
    public GameObject[] player;
    public float[] distance;
    NavMeshAgent nav;
    public int target = -1;
    public bool Fire = false;
    public float Radius = 70;
    public bool Agr = false;
    public int temp = -20;
    public int Damage = 10;
    public int BanTimer = 0;
    public bool Dead = false;
    private int StepTimer = 0;
    public float RunSpeed = 4f;
    public float WalkSpeed = 4f;
    System.Random rnd = new System.Random();
    public int Hits = 0;
    private PhotonView PV;
    public GameObject[] Points;
    public int point = 0;
    public GameObject Weapon;
    //private BotWeapon BW;
    public GameObject RayPosition;
    private RaycastHit RayHit;
    private int ShootTimer;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        PV = GetComponent<PhotonView>();
        //BW = Weapon.GetComponent<BotWeapon>();
        Points = GameObject.FindGameObjectsWithTag("BotMovementPoint");
    }
    
    void FixedUpdate()
    {
        if (PhotonNetwork.isMasterClient)
        {
            PV.RPC("RpcSendTransform", PhotonTargets.All, gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            PV.RPC("RpcSendRotation", PhotonTargets.All, gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z, gameObject.transform.rotation.w);
        }
        player = GameObject.FindGameObjectsWithTag("Player");
        distance = new float[GameManager.instance.MaxPlayers];
        if (Physics.Raycast(RayPosition.transform.position, RayPosition.transform.forward, out RayHit, 150f))
        {
            Debug.DrawRay(RayPosition.transform.position, RayPosition.transform.forward * RayHit.distance, Color.blue);
            if(RayHit.transform.tag == "Player" || RayHit.transform.tag == "PlayerHitbox")
            {
                gameObject.GetComponent<Animator>().SetTrigger("Aim");
                if (ShootTimer % 70 == 0)
                {
                    FireBotWeapon(RayHit);
                }
                ShootTimer += 1;
            }
        }
        
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
            if (distance[i] < Radius && distance[i] > 20)
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
            if (distance[i] < Radius && distance[i] < 20)
            {
                if (target != -1)
                {
                    //target = -1;
                    Fire = true;
                }
            }
            if (target != -1 && !Dead  && !Fire)
            {
                gameObject.GetComponent<NavMeshAgent>().speed = RunSpeed;
                nav.enabled = true;
                nav.SetDestination(player[target].transform.position);
                gameObject.GetComponent<Animator>().SetTrigger("Run");
            }
            if (Fire)
            {
                nav.enabled = false;
                gameObject.GetComponent<Animator>().SetTrigger("Aim");
                LookAt(gameObject.transform, player[target].transform.position, 1.3f);
            }
        }

        if (Agr == false && Dead != true)
        {
            gameObject.GetComponent<NavMeshAgent>().speed = WalkSpeed;
            nav.enabled = true;
            if (PhotonNetwork.isMasterClient)
            {
                if (StepTimer % 600 == 0)
                {
                    point = rnd.Next(0, Points.Length - 1);
                }
                StepTimer += 1;
            }
            gameObject.GetComponent<Animator>().SetTrigger("Run");
            nav.SetDestination(Points[point].transform.position);
        }
        if (HP <= 0)
        {
            gameObject.GetComponent<Animator>().enabled = false;
            Dead = true;
            nav.enabled = false;
            DeathTimer += 1;
            if (DeathTimer >= 1800)
            {
                PhotonNetwork.Destroy(this.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    void LookAt(Transform transform, Vector3 point, float speed)
    {
        var direction = (point - transform.position).normalized;
        //direction.y = 0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), speed);
    }

    void FireBotWeapon(RaycastHit WeaponHit)
    {
        if (WeaponHit.transform.tag == "PlayerHitbox" || WeaponHit.transform.tag == "Player" && WeaponHit.transform.root.GetComponent<PlayerStats>() != null && WeaponHit.transform.root.GetComponent<PlayerStats>().isAlive)
        {
            Debug.Log("player");
            WeaponHit.transform.root.GetComponent<PhotonView>().RPC("ApplyPlayerDamage", PhotonTargets.All, Damage, NameOfBot, PhotonNetwork.player, 1f, false);
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
}
