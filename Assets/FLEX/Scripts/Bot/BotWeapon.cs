using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotWeapon : MonoBehaviour
{
    public int WeaponId;
    public int Damage;
    public int ClipSize;
    public GameObject Target;
    public GameObject RayPosition;

    void Start()
    {

    }

    void Update()
    {

    }

    public void FireBotWeapon(string attacker)
    {
        if(ClipSize > 0)
        {
            Vector3 Spread = new Vector3(Random.Range(-0.01f, 0.01f) * 3f, Random.Range(-0.01f, 0.01f) * 3f, 1f);
            Vector3 Direction = RayPosition.transform.TransformDirection(Spread);
            Ray WeaponRay = new Ray(RayPosition.transform.position, Direction);
            RaycastHit WeaponHit;
            if (Physics.Raycast(WeaponRay, out WeaponHit))
            {
                if (WeaponHit.collider.tag == "PlayerHitbox")
                {
                    Debug.Log("player");
                    if (WeaponHit.transform.tag == "PlayerHitbox" && WeaponHit.transform.root.GetComponent<PlayerStats>() != null && WeaponHit.transform.root.GetComponent<PlayerStats>().isAlive)
                    {
                        WeaponHit.transform.root.GetComponent<PhotonView>().RPC("ApplyPlayerDamage", PhotonTargets.All, Damage, "M4", PhotonNetwork.player, WeaponHit.transform.GetComponent<PlayerBodyPartMultiplier>().DamageModifier, false);
                    }
                }
            }
        }
        else if (ClipSize == 0)
        {
            BotReload();
        }
    }

    void BotReload()
    {

    }

    void BotWeaponEmpty()
    {

    }
}
