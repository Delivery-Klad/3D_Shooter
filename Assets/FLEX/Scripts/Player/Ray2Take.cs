using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Ray2Take : MonoBehaviour
{
    public PlayerStats PlayerStatistics;
    public NotificationSystem NS;
    public GameObject RayPoint;
    private RaycastHit RayHit;
    public int CurrentGunId;
    public int CurrentPistolId;
    private PhotonView PV;
    public int id;
    public GameObject TakePanel;
    public GameObject weapon;
    public GameObject Module;
    public int AmmoLimit = 500;

    RaycastHit hit;
    float hit_distance = 0;
    DepthOfField depth;
    PostProcessVolume vol;
    [Range(0f, 10f)][SerializeField]float speed = 5f;

    private void Start()
    {
        vol = GameObject.Find("SceneManager").GetComponent<PostProcessVolume>();
        vol.profile.TryGetSettings(out depth);
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(RayPoint.transform.position, RayPoint.transform.forward, out RayHit, 2f))
        {
            Debug.DrawRay(RayPoint.transform.position, RayPoint.transform.forward * RayHit.distance, Color.blue);
            if (RayHit.transform.tag == "DropWeapon" || RayHit.transform.tag == "Module" && !PlayerInputManager.instance.FreezePlayerControls)
            {
                TakePanel.GetComponent<Text>().text = PlayerInputManager.instance.Use_Button.ToString() + " - Поднять";
            }
            else if (RayHit.transform.tag == "DropAmmo" || RayHit.transform.tag == "DropHP" && !PlayerInputManager.instance.FreezePlayerControls)
            {
                TakePanel.GetComponent<Text>().text = PlayerInputManager.instance.Use_Button.ToString() + " - Использовать";
            }
            else if (RayHit.transform.tag == "WoodenDoor")
            {
                TakePanel.GetComponent<Text>().text = PlayerInputManager.instance.Use_Button.ToString() + " - Взаимодействовать";
            }
            else
            {
                TakePanel.GetComponent<Text>().text = "";
            }
            if (Input.GetKeyDown(PlayerInputManager.instance.Use_Button))
            {
                if (RayHit.transform.tag == "DropWeapon")
                {
                    Vector3 a = new Vector3(0, 0.5f, 0);
                    PV = RayHit.transform.root.GetComponent<PhotonView>();
                    weapon = gameObject.GetComponent<PlayerWeaponManager>().PlayerInventory[0];

                    if (weapon.activeInHierarchy)
                    {
                        CurrentGunId = PlayerStatistics.CurrentGunId;
                        id = RayHit.transform.root.GetComponent<LootWeapon>().id;
                        if (weapon.GetComponent<PlayerWeapon>().bulletType == PlayerWeapon.BulletType.Nato)
                        {
                            PlayerStatistics.NatoAmmo += weapon.GetComponent<PlayerWeapon>().Ammoleft;
                        }
                        else if (weapon.GetComponent<PlayerWeapon>().bulletType == PlayerWeapon.BulletType.Rifle)
                        {
                            PlayerStatistics.RifleAmmo += weapon.GetComponent<PlayerWeapon>().Ammoleft;
                        }
                        else
                        {
                            PlayerStatistics.PistolAmmo += weapon.GetComponent<PlayerWeapon>().Ammoleft;
                        }
                        gameObject.GetComponent<PhotonView>().RPC("SpawnGun", PhotonTargets.All, CurrentGunId, RayHit.transform.position + a, RayHit.transform.rotation);
                        InGameUI.instance.SelectPrimaryWeapon(id);
                        NS.AddMessage("Подобрано  ( " + RayHit.transform.root.GetComponent<LootWeapon>().WeaponName + ")");
                    }
                    else
                    {
                        CurrentGunId = PlayerStatistics.CurrentPistolId;
                        id = RayHit.transform.root.GetComponent<LootWeapon>().id;
                        if (weapon.GetComponent<PlayerWeapon>().bulletType == PlayerWeapon.BulletType.Nato)
                        {
                            PlayerStatistics.NatoAmmo += weapon.GetComponent<PlayerWeapon>().Ammoleft;
                        }
                        else if (weapon.GetComponent<PlayerWeapon>().bulletType == PlayerWeapon.BulletType.Rifle)
                        {
                            PlayerStatistics.RifleAmmo += weapon.GetComponent<PlayerWeapon>().Ammoleft;
                        }
                        else
                        {
                            PlayerStatistics.PistolAmmo += weapon.GetComponent<PlayerWeapon>().Ammoleft;
                        }
                        gameObject.GetComponent<PhotonView>().RPC("SpawnGun", PhotonTargets.All, CurrentGunId, RayHit.transform.position + a, RayHit.transform.rotation);
                        InGameUI.instance.SelectSecondaryWeapon(id);
                        NS.AddMessage("Подобрано  ( " + RayHit.transform.root.GetComponent<LootWeapon>().WeaponName + " )");
                    }
                    PV.RPC("RemoveGun", PhotonTargets.All, true);
                    int Items = 0;
                    if (PlayerPrefs.HasKey("TotalItems"))
                    {
                        Items = PlayerPrefs.GetInt("TotalItems");
                    }
                    Items += 1;
                    PlayerPrefs.SetInt("TotalItems", Items);
                }
                else if (RayHit.transform.tag == "DropHP")
                {
                    PV = RayHit.transform.root.GetComponent<PhotonView>();
                    int maxPlayerHP = RayHit.transform.root.GetComponent<Take_HP>().maxPlayerHP;

                    if (PlayerStatistics.PlayerHealth != maxPlayerHP)
                    {
                        PlayerStatistics.PlayerHealth += RayHit.transform.root.GetComponent<Take_HP>().CountHP;
                        PV.RPC("RemoveHP", PhotonTargets.All, true);
                        NS.AddMessage("Здоровье восполнено");
                    }
                    else
                    {
                        NS.AddMessage("У вас максимальный уровень здоровья");
                    }
                    if (PlayerStatistics.PlayerHealth > maxPlayerHP)
                    {
                        PlayerStatistics.PlayerHealth = maxPlayerHP;
                    }
                    PlayerStatistics.ApplyPlayerHealth();
                }
                else if (RayHit.transform.tag == "Module")
                {
                    PV = RayHit.transform.root.GetComponent<PhotonView>();
                    Module = RayHit.transform.gameObject;
                    if (gameObject.GetComponent<PlayerWeaponManager>().PlayerInventory[0].activeInHierarchy)
                    {
                        weapon = gameObject.GetComponent<PlayerWeaponManager>().PlayerInventory[0];
                    }
                    else if (weapon = gameObject.GetComponent<PlayerWeaponManager>().PlayerInventory[1])
                    {
                        weapon = gameObject.GetComponent<PlayerWeaponManager>().PlayerInventory[1];
                    }
                    if (Module.GetComponent<Take_Modules>().Muzzle_break)
                    {
                        if (weapon.GetComponent<PlayerWeapon>().Muzzle_br != null)
                        {
                            if (!weapon.GetComponent<PlayerWeapon>().Muzzle_break && !weapon.GetComponent<PlayerWeapon>().Silencer)
                            {
                                weapon.GetComponent<PlayerWeapon>().SetModules(true, false, false, false, false);
                                PV.RPC("RemoveModule", PhotonTargets.All, true);
                                NS.AddMessage("Дульный тормоз установлен на ( " + weapon.GetComponent<PlayerWeapon>().WeaponName + " )");
                            }
                            else if (weapon.GetComponent<PlayerWeapon>().Muzzle_break)
                            {
                                NS.AddMessage("Оружие уже содержит данную модификацию");
                            }
                            else if (weapon.GetComponent<PlayerWeapon>().Silencer)
                            {
                                weapon.GetComponent<PlayerWeapon>().SetModules(true, false, false, false, false);
                                PV.RPC("RemoveModule", PhotonTargets.All, true);
                                NS.AddMessage("Дульный тормоз установлен на ( " + weapon.GetComponent<PlayerWeapon>().WeaponName + " )");
                            }
                        }
                        else
                        {
                            NS.AddMessage("Оружие не поддерживает данную модификацию");
                        }
                    }
                    else if (Module.GetComponent<Take_Modules>().ACOG)
                    {
                        if (weapon.GetComponent<PlayerWeapon>().Acog != null)
                        {
                            if (!weapon.GetComponent<PlayerWeapon>().ACOG && !weapon.GetComponent<PlayerWeapon>().Red_dot && !weapon.GetComponent<PlayerWeapon>().Sniper)
                            {
                                weapon.GetComponent<PlayerWeapon>().SetModules(false, true, false, false, false);
                                PV.RPC("RemoveModule", PhotonTargets.All, true);
                                NS.AddMessage("ACOG установлен на ( " + weapon.GetComponent<PlayerWeapon>().WeaponName + " )");
                            }
                            else if (weapon.GetComponent<PlayerWeapon>().ACOG)
                            {
                                NS.AddMessage("Оружие уже содержит данную модификацию");
                            }
                            else if (weapon.GetComponent<PlayerWeapon>().Red_dot)
                            {
                                weapon.GetComponent<PlayerWeapon>().SetModules(false, true, false, false, false);
                                PV.RPC("RemoveModule", PhotonTargets.All, true);
                                NS.AddMessage("ACOG установлен на ( " + weapon.GetComponent<PlayerWeapon>().WeaponName + " )");
                            }
                            else if (weapon.GetComponent<PlayerWeapon>().Sniper)
                            {
                                weapon.GetComponent<PlayerWeapon>().SetModules(false, true, false, false, false);
                                PV.RPC("RemoveModule", PhotonTargets.All, true);
                                NS.AddMessage("ACOG установлен на ( " + weapon.GetComponent<PlayerWeapon>().WeaponName + " )");
                            }
                        }
                        else
                        {
                            NS.AddMessage("Оружие не поддерживает данную модификацию");
                        }
                    }
                    else if (Module.GetComponent<Take_Modules>().Red_dot)
                    {
                        if (weapon.GetComponent<PlayerWeapon>().Red_Dot != null)
                        {
                            if (!weapon.GetComponent<PlayerWeapon>().ACOG && !weapon.GetComponent<PlayerWeapon>().Red_dot && !weapon.GetComponent<PlayerWeapon>().Sniper)
                            {
                                weapon.GetComponent<PlayerWeapon>().SetModules(false, false, true, false, false);
                                PV.RPC("RemoveModule", PhotonTargets.All, true);
                                NS.AddMessage("RedDot установлен на ( " + weapon.GetComponent<PlayerWeapon>().WeaponName + " )");
                            }
                            else if (weapon.GetComponent<PlayerWeapon>().Red_dot)
                            {
                                NS.AddMessage("Оружие уже содержит данную модификацию");
                            }
                            else if (weapon.GetComponent<PlayerWeapon>().ACOG)
                            {
                                weapon.GetComponent<PlayerWeapon>().SetModules(false, false, true, false, false);
                                PV.RPC("RemoveModule", PhotonTargets.All, true);
                                NS.AddMessage("RedDot установлен на ( " + weapon.GetComponent<PlayerWeapon>().WeaponName + " )");
                            }
                            else if (weapon.GetComponent<PlayerWeapon>().Sniper)
                            {
                                weapon.GetComponent<PlayerWeapon>().SetModules(false, true, false, false, false);
                                PV.RPC("RemoveModule", PhotonTargets.All, true);
                                NS.AddMessage("RedDot установлен на ( " + weapon.GetComponent<PlayerWeapon>().WeaponName + " )");
                            }
                        }
                        else
                        {
                            NS.AddMessage("Оружие не поддерживает данную модификацию");
                        }
                    }
                    else if (Module.GetComponent<Take_Modules>().Silencer)
                    {
                        if (weapon.GetComponent<PlayerWeapon>().Sil != null)
                        {
                            if (!weapon.GetComponent<PlayerWeapon>().Muzzle_break && !weapon.GetComponent<PlayerWeapon>().Silencer)
                            {
                                weapon.GetComponent<PlayerWeapon>().SetModules(false, false, false, true, false);
                                PV.RPC("RemoveModule", PhotonTargets.All, true);
                                NS.AddMessage("Глушитель установлен на ( " + weapon.GetComponent<PlayerWeapon>().WeaponName + " )");
                            }
                            else if (weapon.GetComponent<PlayerWeapon>().Silencer)
                            {
                                NS.AddMessage("Оружие уже содержит данную модификацию");
                            }
                            else if (weapon.GetComponent<PlayerWeapon>().Muzzle_break)
                            {
                                weapon.GetComponent<PlayerWeapon>().SetModules(false, false, false, true, false);
                                PV.RPC("RemoveModule", PhotonTargets.All, true);
                                NS.AddMessage("Глушитель установлен на ( " + weapon.GetComponent<PlayerWeapon>().WeaponName + " )");
                            }
                        }
                        else
                        {
                            NS.AddMessage("Оружие не поддерживает данную модификацию");
                        }
                    }
                    else if (Module.GetComponent<Take_Modules>().Sniper)
                    {
                        if (weapon.GetComponent<PlayerWeapon>().Sniper_ != null)
                        {
                            if (!weapon.GetComponent<PlayerWeapon>().ACOG && !weapon.GetComponent<PlayerWeapon>().Red_dot && !weapon.GetComponent<PlayerWeapon>().Sniper)
                            {
                                weapon.GetComponent<PlayerWeapon>().SetModules(false, false, false, false, true);
                                PV.RPC("RemoveModule", PhotonTargets.All, true);
                                NS.AddMessage("Scope установлен на ( " + weapon.GetComponent<PlayerWeapon>().WeaponName + " )");
                            }
                            else if (weapon.GetComponent<PlayerWeapon>().Sniper)
                            {
                                NS.AddMessage("Оружие уже содержит данную модификацию");
                            }
                            else if (weapon.GetComponent<PlayerWeapon>().ACOG)
                            {
                                weapon.GetComponent<PlayerWeapon>().SetModules(false, false, false, false, true);
                                PV.RPC("RemoveModule", PhotonTargets.All, true);
                                NS.AddMessage("Scope установлен на ( " + weapon.GetComponent<PlayerWeapon>().WeaponName + " )");
                            }
                            else if (weapon.GetComponent<PlayerWeapon>().Sniper)
                            {
                                weapon.GetComponent<PlayerWeapon>().SetModules(false, true, false, false, true);
                                PV.RPC("RemoveModule", PhotonTargets.All, true);
                                NS.AddMessage("Scope установлен на ( " + weapon.GetComponent<PlayerWeapon>().WeaponName + " )");
                            }
                        }
                        else
                        {
                            NS.AddMessage("Оружие не поддерживает данную модификацию");
                        }
                    }
                }
                else if (RayHit.transform.tag == "DropAmmo")
                {
                    PV = RayHit.transform.root.GetComponent<PhotonView>();
                    weapon = gameObject.GetComponent<PlayerWeaponManager>().PlayerInventory[0];
                    if (weapon.activeInHierarchy)
                    {
                        if (weapon.GetComponent<PlayerWeapon>().bulletType == PlayerWeapon.BulletType.Nato)
                        {
                            if (PlayerStatistics.NatoAmmo < AmmoLimit)
                            {
                                PlayerStatistics.NatoAmmo += RayHit.transform.root.GetComponent<Take_Ammo>().CountAmmo;
                                PV.RPC("RemoveAmmo", PhotonTargets.All, true);
                                NS.AddMessage("Патроны восполнены");
                            }
                        }
                        else if (weapon.GetComponent<PlayerWeapon>().bulletType == PlayerWeapon.BulletType.Rifle)
                        {
                            if (PlayerStatistics.RifleAmmo < AmmoLimit)
                            {
                                PlayerStatistics.RifleAmmo += RayHit.transform.root.GetComponent<Take_Ammo>().CountAmmo;
                                PV.RPC("RemoveAmmo", PhotonTargets.All, true);
                                NS.AddMessage("Патроны восполнены");
                            }
                        }
                        else
                        {
                            if (PlayerStatistics.PistolAmmo < AmmoLimit)
                            {
                                PlayerStatistics.PistolAmmo += RayHit.transform.root.GetComponent<Take_Ammo>().CountAmmo;
                                PV.RPC("RemoveAmmo", PhotonTargets.All, true);
                                NS.AddMessage("Патроны восполнены");
                            }
                        }
                    }
                    else
                    {
                        weapon = gameObject.GetComponent<PlayerWeaponManager>().PlayerInventory[1];
                        if (weapon.GetComponent<PlayerWeapon>().bulletType == PlayerWeapon.BulletType.Nato)
                        {
                            if (PlayerStatistics.NatoAmmo < AmmoLimit)
                            {
                                PlayerStatistics.NatoAmmo += RayHit.transform.root.GetComponent<Take_Ammo>().CountAmmo;
                                PV.RPC("RemoveAmmo", PhotonTargets.All, true);
                                NS.AddMessage("Патроны восполнены");
                            }
                        }
                        else if (weapon.GetComponent<PlayerWeapon>().bulletType == PlayerWeapon.BulletType.Rifle)
                        {
                            if (PlayerStatistics.RifleAmmo < AmmoLimit)
                            {
                                PlayerStatistics.RifleAmmo += RayHit.transform.root.GetComponent<Take_Ammo>().CountAmmo;
                                PV.RPC("RemoveAmmo", PhotonTargets.All, true);
                                NS.AddMessage("Патроны восполнены");
                            }
                        }
                        else
                        {
                            if (PlayerStatistics.PistolAmmo < AmmoLimit)
                            {
                                PlayerStatistics.PistolAmmo += RayHit.transform.root.GetComponent<Take_Ammo>().CountAmmo;
                                PV.RPC("RemoveAmmo", PhotonTargets.All, true);
                                NS.AddMessage("Патроны восполнены");
                            }
                        }
                    }
                }
            }
        }
        else
        {
            TakePanel.GetComponent<Text>().text = "";
        }

        if (Physics.Raycast(transform.position, transform.forward * 100, out hit, 100f))
        {
            hit_distance = Vector3.Distance(transform.position, hit.point);
        }
        else
        {
            if (hit_distance < 100f)
            {
                hit_distance++;
            }
        }
        Focus();
    }

    void Focus()
    {
        depth.focusDistance.value = hit_distance;
        depth.focusDistance.value = Mathf.Lerp(depth.focusDistance.value, hit_distance, Time.deltaTime * speed);
    }
}
