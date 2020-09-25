using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerThirdPersonController : MonoBehaviour
{
    [Header("References")]
    public PlayerMovementController PlayerMovementManager;
    public PhotonView ThirdPersonPhotonView;
    public Animator PlayerThirdPersonAnimator;
    public Rigidbody[] PlayerThirdPersonRigidbodies;
    public Collider[] PlayerThirdPersonColliders;
    public Renderer[] ThirdPersonRenderers;
    public Transform ThirdPersonWeaponHolder;
    public Transform PlayerCamera;
    public GameObject ThirdPersonWorldWeapon;
    public float SyncedAimangle;
    public bool WeaponIK = false;
    public Transform LeftHand;
    public Vector3 Offset;
    public bool UseTarget = true;
    public Transform Target;
    public Transform Chest;
    public Vector3 TargetDefaultValues;
    public float TargetStartHeight = 0.5f;
    public float PlayerPoseModifier = 0.3f;
    public Rigidbody ThirdPersonModelRigidbody;
    public Collider ThirdPersonModelCollider;
    public bool PlayDrawAnimation = false;
    public int BulletHP = 5;
    [Header("Sounds")]
    public float FootStepVolume = 0.5f;
    public float WaterStepVolume = 1.0f;
    public float SandStepVolume = 1.0f;
    public float WoodStepVolume = 1.0f;
    public float MetalStepVolume = 1.0f;
    public float StoneStepVolume = 1.0f;
    public AudioSource FootstepAudiosource;
    public AudioClip[] FootstepSounds;
    public AudioClip[] SandstepSounds;
    public AudioClip[] WoodstepSounds;
    public AudioClip[] MetalstepSounds;
    public AudioClip[] StonestepSounds;
    public AudioClip[] WaterstepSounds;

    void Start()
    {
        if (ThirdPersonPhotonView.isMine)
        {
            ShowPlayerModel(false);
            SetPlayerModelColliders(false);
        }
    }

    void Update()
    {
        if (ThirdPersonPhotonView.isMine)
        {
            Vector3 TargetHeight = new Vector3(0f, this.PlayerThirdPersonAnimator.GetFloat("Aim") + TargetStartHeight + PlayerPoseModifier, 0f);
            Target.position = TargetHeight;
        }
    }

    void LateUpdate()
    {
        HandleThirdPersonAiming();
    }

    public void SetPlayerModelColliders(bool Toggle)
    {
        foreach (Collider PlayerCollider in PlayerThirdPersonColliders)
        {
            PlayerCollider.isTrigger = false;
            PlayerCollider.enabled = Toggle;
        }
        foreach (Rigidbody PlayerRigidbody in PlayerThirdPersonRigidbodies)
        {
            PlayerRigidbody.isKinematic = !Toggle;
        }
    }

    public void ShowPlayerModel(bool Toggle)
    {
        foreach (Renderer ThirdPersonRenderer in ThirdPersonRenderers)
        {
            ThirdPersonRenderer.enabled = Toggle;
        }
        if (ThirdPersonWorldWeapon != null)
        {
            ThirdPersonWorldWeapon.SetActive(Toggle);
        }
    }

    public void ThirdPersonPlayerKilled()
    {
        if (!GameManager.instance.InVehicle)
        {
            PlayerCamera.SetParent(PlayerThirdPersonColliders[7].transform);
            ThirdPersonPhotonView.RPC("OnThirdPersonDeath", PhotonTargets.All, null);
        }
    }

    [PunRPC]
    public void SetThirdPersonWeapon(int WeaponID)
    {
        EnableWeaponIK(false);
        if (PlayDrawAnimation)
        {
            PlayerThirdPersonAnimator.SetTrigger("DrawWeapon");
        }
        else if (!PlayDrawAnimation)
        {
            EnableWeaponIK(true);
            PlayDrawAnimation = true;
        }
        if (ThirdPersonWorldWeapon != null)
        {
            Destroy(ThirdPersonWorldWeapon);
            ThirdPersonWorldWeapon = Instantiate(GameManager.instance.AllGameWeapons[WeaponID].ThirdPersonPrefab, ThirdPersonWeaponHolder);
            PlayerThirdPersonAnimator.SetInteger("WeaponType", ThirdPersonWorldWeapon.GetComponent<ThirdPersonWeapon>().WeaponHoldType);
            if (ThirdPersonPhotonView.isMine)
            {
                ThirdPersonWorldWeapon.SetActive(false);
            }
        }
        else
        {
            ThirdPersonWorldWeapon = Instantiate(GameManager.instance.AllGameWeapons[WeaponID].ThirdPersonPrefab, ThirdPersonWeaponHolder);
            PlayerThirdPersonAnimator.SetInteger("WeaponType", ThirdPersonWorldWeapon.GetComponent<ThirdPersonWeapon>().WeaponHoldType);
            if (ThirdPersonPhotonView.isMine)
            {
                ThirdPersonWorldWeapon.SetActive(false);
            }
        }
    }

    [PunRPC]
    public void PlayFootstepSoundNetwork(string Type)
    {
        if (Type == "Normal")
        {
            FootstepAudiosource.PlayOneShot(FootstepSounds[Random.Range(0, FootstepSounds.Length)], FootStepVolume);
        }
        if (Type == "Water")
        {
            FootstepAudiosource.PlayOneShot(WaterstepSounds[Random.Range(0, WaterstepSounds.Length)], WaterStepVolume);
        }
        if (Type == "Wood")
        {
            FootstepAudiosource.PlayOneShot(WoodstepSounds[Random.Range(0, WoodstepSounds.Length)], WoodStepVolume);
        }
        if (Type == "Wood2")
        {
            FootstepAudiosource.PlayOneShot(WoodstepSounds[Random.Range(0, WoodstepSounds.Length)], WoodStepVolume);
        }
        if (Type == "Sand")
        {
            FootstepAudiosource.PlayOneShot(SandstepSounds[Random.Range(0, SandstepSounds.Length)], SandStepVolume);
        }
        if (Type == "Metal")
        {
            FootstepAudiosource.PlayOneShot(MetalstepSounds[Random.Range(0, MetalstepSounds.Length)], MetalStepVolume);
        }
        if (Type == "Stone")
        {
            FootstepAudiosource.PlayOneShot(StonestepSounds[Random.Range(0, StonestepSounds.Length)], StoneStepVolume);
        }
        if (Type == "Stone2")
        {
            FootstepAudiosource.PlayOneShot(StonestepSounds[Random.Range(0, StonestepSounds.Length)], StoneStepVolume);
        }
    }

    [PunRPC]
    public void ThirdPersonEmpty()
    {
        ThirdPersonWeapon TpWeapon = ThirdPersonWorldWeapon.GetComponent<ThirdPersonWeapon>();
        if (TpWeapon.WeaponEmptySound.Length != 0)
        {
            TpWeapon.ThirdPersonAudioSource.PlayOneShot(TpWeapon.WeaponEmptySound[0], 0.1f);
        }
    }

    [PunRPC]
    public void ThirdPersonSwitch()
    {
        ThirdPersonWeapon TpWeapon = ThirdPersonWorldWeapon.GetComponent<ThirdPersonWeapon>();
        if (TpWeapon.WeaponSwitchSound.Length != 0)
        {
            TpWeapon.ThirdPersonAudioSource.PlayOneShot(TpWeapon.WeaponSwitchSound[0]);
        }
    }

    [PunRPC]
    public void FlashOn()
    {
        ThirdPersonWeapon TpWeapon = ThirdPersonWorldWeapon.GetComponent<ThirdPersonWeapon>();
        TpWeapon.flashlight.SetActive(true);
    }

    [PunRPC]
    public void FlashOff()
    {
        ThirdPersonWeapon TpWeapon = ThirdPersonWorldWeapon.GetComponent<ThirdPersonWeapon>();
        TpWeapon.flashlight.SetActive(false);
    }

    [PunRPC]
    public void SetModules(bool muzzle, bool acog, bool reddot, bool sil, bool _sniper)
    {
        ThirdPersonWeapon TpWeapon = ThirdPersonWorldWeapon.GetComponent<ThirdPersonWeapon>();
        if (muzzle)
        {
            Debug.Log("zaletelo");
            TpWeapon.Muzzle_break = true;
            TpWeapon.Muzzle_br.SetActive(true);
            if (TpWeapon.Silencer)
            {
                TpWeapon.Silencer = false;
                TpWeapon.Sil.SetActive(false);
            }
        }
        if (acog)
        {
            TpWeapon.ACOG = true;
            TpWeapon.Acog.SetActive(true);
            if (TpWeapon.Red_dot)
            {
                TpWeapon.Red_dot = false;
                TpWeapon.Red_Dot.SetActive(false);
            }
            else if (TpWeapon.Sniper)
            {
                TpWeapon.Sniper = false;
                TpWeapon.Sniper_.SetActive(false);
            }
        }
        if (reddot)
        {
            TpWeapon.Red_dot = true;
            TpWeapon.Red_Dot.SetActive(true);
            if (TpWeapon.ACOG)
            {
                TpWeapon.ACOG = false;
                TpWeapon.Acog.SetActive(false);
            }
            else if (TpWeapon.Sniper)
            {
                TpWeapon.Sniper = false;
                TpWeapon.Sniper_.SetActive(false);
            }
        }
        if (sil)
        {
            TpWeapon.Silencer = true;
            TpWeapon.Sil.SetActive(true);
            if (TpWeapon.Muzzle_break)
            {
                TpWeapon.Muzzle_break = false;
                TpWeapon.Muzzle_br.SetActive(false);
            }
        }
        if (_sniper)
        {
            TpWeapon.Sniper = true;
            TpWeapon.Sniper_.SetActive(true);
            if (TpWeapon.ACOG)
            {
                TpWeapon.ACOG = false;
                TpWeapon.Acog.SetActive(false);
            }
            else if (TpWeapon.Red_dot)
            {
                TpWeapon.Red_dot = false;
                TpWeapon.Red_Dot.SetActive(false);
            }
        }
    }

    [PunRPC]
    public void ThirdPersonReload()
    {
        ThirdPersonWeapon TpWeapon = ThirdPersonWorldWeapon.GetComponent<ThirdPersonWeapon>();
        PlayerThirdPersonAnimator.SetBool("Reloading", true);
        TpWeapon.Clip.SetActive(false);
        StartCoroutine(HideMag(TpWeapon));
        if (TpWeapon.WeaponReloadSound.Length != 0)
        {
            TpWeapon.ThirdPersonAudioSource.PlayOneShot(TpWeapon.WeaponReloadSound[0]);
        }
        EnableWeaponIK(false);
    }

    IEnumerator HideMag(ThirdPersonWeapon TpWeapon)
    {
        yield return new WaitForSeconds(1);
        TpWeapon.Clip.SetActive(true);
    }

    [PunRPC]
    public void FinishReload()
    {
        PlayerThirdPersonAnimator.SetBool("Reloading", false);
        EnableWeaponIK(true);
    }

    public void EnableWeaponIK(bool Toggle)
    {
        ThirdPersonPhotonView.RPC("ThirdPersonEnableWeaponIK", PhotonTargets.AllBuffered, Toggle);
    }

    [PunRPC]
    public void ThirdPersonEnableWeaponIK(bool Toggle)
    {
        this.WeaponIK = Toggle;
    }

    [PunRPC]
    public void ThirdPersonFireWeapon()
    {
        ThirdPersonWeapon TpWeapon = ThirdPersonWorldWeapon.GetComponent<ThirdPersonWeapon>();
        PlayerThirdPersonAnimator.SetTrigger("Shoot");
        if (TpWeapon.WeaponFireSound.Length != 0)
        {
            TpWeapon.MuzzleFlash.Play();
            TpWeapon.ThirdPersonAudioSource.PlayOneShot(TpWeapon.WeaponFireSound[0]);
        }
        if (TpWeapon.WeaponFireLoopSound != null)
        {
            TpWeapon.MuzzleFlash.Play();
            TpWeapon.ThirdPersonAudioSource.loop = true;
            TpWeapon.ThirdPersonAudioSource.clip = TpWeapon.WeaponFireLoopSound;
            TpWeapon.ThirdPersonAudioSource.Play();
        }
    }

    [PunRPC]
    public void ThirdPersonImpacts(Vector3 Spread, Vector3 Direction, Vector3 Pos)
    {
        ThirdPersonWeapon TpWeapon = ThirdPersonWorldWeapon.GetComponent<ThirdPersonWeapon>();
        Ray WeaponRay = new Ray(Pos, Direction);
        RaycastHit WeaponHit;
        //WeaponHit = Physics.RaycastAll(WeaponRay, TpWeapon.WeaponRange, TpWeapon.WeaponLayerMask).OrderBy(h => h.distance).ToArray();

        if (Physics.Raycast(WeaponRay, out WeaponHit, TpWeapon.WeaponRange, TpWeapon.WeaponLayerMask))
        {
            if (WeaponHit.transform.GetComponent<Rigidbody>() != null)
            {
                WeaponHit.transform.GetComponent<Rigidbody>().AddForce(TpWeapon.WeaponImpactForce * Direction, ForceMode.Impulse);
            }
            if (WeaponHit.collider.tag == "Untagged" || WeaponHit.collider.tag == "Concrete")
            {
                GameObject ConcreteHole = Instantiate(TpWeapon.ConcreteImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                ConcreteHole.transform.parent = WeaponHit.transform;
            }
            if (WeaponHit.collider.tag == "Metal" || WeaponHit.collider.tag == "MetalDoor")
            {
                GameObject MetalHole = Instantiate(TpWeapon.MetalImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                MetalHole.transform.parent = WeaponHit.transform;
            }
            if (WeaponHit.collider.tag == "Sand")
            {
                GameObject SandHole = Instantiate(TpWeapon.SandImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                SandHole.transform.parent = WeaponHit.transform;
            }
            if (WeaponHit.collider.tag == "Ground")
            {
                GameObject SandHole = Instantiate(TpWeapon.SandImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                SandHole.transform.parent = WeaponHit.transform;
            }
            if (WeaponHit.collider.tag == "Wood" || WeaponHit.collider.tag == "WoodenDoor")
            {
                GameObject WoodHole = Instantiate(TpWeapon.WoodImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                WoodHole.transform.parent = WeaponHit.transform;
            }
            if (WeaponHit.collider.tag == "Water")
            {
                GameObject WaterHole = Instantiate(TpWeapon.WaterImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                WaterHole.transform.parent = WeaponHit.transform;
            }
            if (WeaponHit.collider.tag == "Glass" || WeaponHit.collider.tag == "BreakedGlass")
            {
                GameObject GlassHole = Instantiate(TpWeapon.GlassImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                GlassHole.transform.parent = WeaponHit.transform;
                //WeaponHit.transform.GetComponent<PhotonView>().RPC("ApplyDamage", PhotonTargets.All, Damage);
            }
            if (WeaponHit.collider.tag == "Vehicle")
            { //&& WeaponHit.transform.root.GetComponent<VehicleStats> ().VehicleAlive == true
                GameObject MetalHole = Instantiate(TpWeapon.MetalImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                MetalHole.transform.parent = WeaponHit.transform;
                //WeaponHit.transform.root.GetComponent<VehicleStats> ().VehiclePhotonView.RPC ("FinishVehicleDamage", PhotonTargets.All, Damage ,WeaponName, PhotonNetwork.player);
            }
        }
    }

    [PunRPC]
    public void OnThirdPersonDeath()
    {
        SetPlayerModelColliders(true);
        PlayerThirdPersonAnimator.enabled = false;
        EnableWeaponIK(false);
        UseTarget = false;
        foreach (Collider part in PlayerThirdPersonColliders)
        {
            part.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    void OnAnimatorIK(int layer)
    {
        if (WeaponIK)
        {
            PlayerThirdPersonAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            PlayerThirdPersonAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            PlayerThirdPersonAnimator.SetIKPosition(AvatarIKGoal.LeftHand, ThirdPersonWorldWeapon.GetComponent<ThirdPersonWeapon>().LeftHandTransform.position);
            PlayerThirdPersonAnimator.SetIKRotation(AvatarIKGoal.LeftHand, ThirdPersonWorldWeapon.GetComponent<ThirdPersonWeapon>().LeftHandTransform.rotation);
        }
    }

    public void HandleThirdPersonAiming()
    {
        if (PlayerThirdPersonAnimator.GetBool("Sprinting") == false && UseTarget == true)
        {
            Chest.LookAt(Target);
            Chest.rotation = Chest.rotation * Quaternion.Euler(Offset);
        }
        if (ThirdPersonPhotonView.isMine)
        {
            float AimAngle = PlayerCamera.transform.localRotation.x;
            PlayerThirdPersonAnimator.SetFloat("Aim", AimAngle * -1.5f);
        }
        if (!ThirdPersonPhotonView.isMine)
        {
            PlayerThirdPersonAnimator.SetFloat("Aim", Mathf.Lerp(PlayerThirdPersonAnimator.GetFloat("Aim"), SyncedAimangle, 0.05f));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(PlayerThirdPersonAnimator.GetFloat("Aim"));
        }
        else if (stream.isReading)
        {
            SyncedAimangle = (float)stream.ReceiveNext();
        }
    }
}
