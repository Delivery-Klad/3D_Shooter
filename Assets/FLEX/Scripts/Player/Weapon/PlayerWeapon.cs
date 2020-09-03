using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerWeapon : MonoBehaviour {

    [Header("Weapon Stats")]
    public bool flash;
    public GameObject flashlight;
    public bool flashon;
    public bool NeedToHide = false;
    public GameObject Scope;
    public GameObject HidenScope;
    public bool Muzzle_break = false;
    public bool Silencer = false;
    public bool ACOG = false;
    public bool Red_dot = false;
    public bool ForeGrip = false;
    public GameObject Muzzle_br;
    public GameObject Sil;
    public GameObject Acog;
    public GameObject Red_Dot;
    public GameObject Fore_Grip;
    public Vector3 ACOGAimPosition;
    public Vector3 RedDotAimPosition;
    public string WeaponName;
	public int WeaponID;
    public BulletType bulletType;
    public int BulletHP = 5;
    public int ShootDistance = 100;
	public int Damage = 35;
	public int ClipSize = 30;
	public int Ammoleft = 30;
	public int CurrentAmmo = 0;
    public int AmmoLimit = 500;
    private float FireRate = 0.1f;
	public float FirstModeFireRate;
	public float SecondModeFireRate;
	public float WeaponRange;
	public float WeaponImpactForce;

	[Header("Режимы стрельбы")]
	private bool CanSwitchWeaponMode = true;
	public enum WeaponType { none, bullet, burst, shotgun, launcher};
	public enum WeaponFiringType {semi, auto};
	[Header("Основной режим стрельбы")]
	public WeaponType FirstMode = WeaponType.bullet;
	public WeaponFiringType FirstModeFiringType = WeaponFiringType.auto;
	[Header("Второстепенный режим стрельбы")]
	public WeaponType SecondMode = WeaponType.none;
	public WeaponFiringType SecondModeFiringType = WeaponFiringType.semi;
	[HideInInspector]public WeaponType CurrentMode = WeaponType.bullet;
	[HideInInspector]public WeaponFiringType CurrentFiringType = WeaponFiringType.auto;
	[Space(10)]
	public LayerMask WeaponLayerMask;
	private float ShootTimer = 0;	
	public Transform Weaponbarrel;
	public Transform FirstModeBarrel;
	public Transform SecondModeBarrel;

	[Header("Разброс оружия")]
	public float baseInaccuracyhip = 1.2f;
	public float baseInaccuracyaim = 0.1f;
	private float triggerTime = 3f;

	[Header("Позиция оружия")]
	public Vector3 AimPosition;
	public Vector3 WeaponOffset;
	[Range(25,70)]
	public int AimFov = 40;
	[Range(0.1f,2f)]
	public float AimSpeedModifier = 1f;
	public bool IsAiming;
	private Camera PlayerCamera;

    [Header("Найстройка отдачи")]
    public Transform WeaponRecoilHolder;
    public Vector3 RecoilRotation;
    public Vector3 RecoilKickback;
    public float RecoilModifier = 1f;
    private Vector3 CurrentRecoil1;
    private Vector3 CurrentRecoil2;
    private Vector3 CurrentRecoil3;
    private Vector3 CurrentRecoil4;

    [Header("Настройка очереди")]
	[Range(1,10)]
	public int shotsPerBurst = 3;
	public float BurstTime = 0.07f;
	public bool Bursting = false;

	[Header("Настройка дроби")]
	[Range(1,20)]
	public int pelletsPerShot = 10;

	[Header("Прицелы")]
	public Texture CrosshairFirstMode;
	public Texture CrosshairSecondMode;

	[Header("Следы пуль")]
	public GameObject ConcreteImpact;
	public GameObject WoodImpact;
	public GameObject MetalImpact;
	public GameObject FleshImpact;
	public GameObject SandImpact;
	public GameObject WaterImpact;
    public GameObject GlassImpact;

    [Header("Эффекты стрельбы")]
	public ParticleSystem WeaponPrimaryMuzzleFlash;
	public ParticleSystem WeaponShellEject;

	[Header("Звуки оружия")]
	public AudioSource WeaponAudiosource;
	public AudioClip WeaponDrawSound;
	public AudioClip WeaponCurrentFireSound;
	public AudioClip WeaponFirePrimarySound;
	public AudioClip WeaponFireSecondarySound;
	public AudioClip WeaponReloadSound;
	public AudioClip WeaponEmptySound;
	public AudioClip WeaponSwitchModeSound;

	[Header("Анимации оружия")]
	public Animator WeaponMovementAnimator;
	public enum MovementType { Rifle, Pistol };
	public MovementType WeaponMovementType;
	public Animation WeaponAnimationComponent;
	public AnimationClip WeaponPrimaryIdleAnimation;
	public AnimationClip WeaponDrawAnimation;
	public AnimationClip WeaponFireAnimation;
	public AnimationClip WeaponReloadAnimation;
	public AnimationClip WeaponFireEmptyAnimation;
	public AnimationClip WeaponSwitchAnimation;
	public AnimationClip WeaponSecondaryIdleAnimation;
	public AnimationClip WeaponFireSecondaryAnimation;
	public AnimationClip WeaponReloadSecondaryAnimation;

	[Header("Компоненты игрока")]
	public PlayerMovementController PlayerMovementController;
	public CharacterController PlayerCharacterController;
	public PlayerWeaponManager PlayerWeaponManager;
	public Transform WeaponHolder;
	public PhotonView WeaponPhotonView;
    public PlayerStats PlayerStats;

	void Start()
	{
        Ammoleft = 0;
        PlayerStats = transform.root.GetComponent<PlayerStats>();
        PlayerMovementController = transform.root.GetComponent<PlayerMovementController> ();
		PlayerCharacterController = transform.root.GetComponent<CharacterController> ();
		WeaponHolder = transform.parent;
		PlayerCamera = PlayerMovementController.PlayerCamera.GetComponent<Camera>();
		WeaponRecoilHolder = PlayerWeaponManager.WeaponRecoilHolder;
		Bursting = false;
		IsAiming = false;
		CanSwitchWeaponMode = true;
		CurrentMode = FirstMode;
		FireRate = FirstModeFireRate;
		Weaponbarrel = FirstModeBarrel;
		CurrentFiringType = FirstModeFiringType;
		WeaponAudiosource = transform.parent.GetComponent<AudioSource> ();
		triggerTime *= baseInaccuracyhip;
		WeaponCurrentFireSound = WeaponFirePrimarySound;
	} 

	void OnEnable()
	{
		PlayerWeaponManager = transform.root.GetComponent<PlayerWeaponManager> ();
		WeaponMovementAnimator = PlayerWeaponManager.WeaponMovementAnimator;
		if (WeaponMovementType == MovementType.Rifle)
        {
			WeaponMovementAnimator.SetLayerWeight (1, 0f);
		}
        else if (WeaponMovementType == MovementType.Pistol)
        {
			WeaponMovementAnimator.SetLayerWeight (1, 1f);
		}
		if (FirstModeFiringType == WeaponFiringType.semi)
        {
			PlayerInputManager.instance.IsAuto = false;
		}
        else
        {
			PlayerInputManager.instance.IsAuto = true;
		}
	}

	void Update()
	{
		if (!GameManager.instance.InVehicle)
        {
			GetPlayerInput ();
			RecoilController ();
			UpdateWeaponHud ();
			WeaponMovementAnimationController();
		}
        if (flash)
        {
            if (Input.GetKeyDown(KeyCode.O) && !flashon)
            {
                flashlight.SetActive(true);
                flashon = true;
                PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("FlashOn", PhotonTargets.All, null);
            }
            else if (Input.GetKeyDown(KeyCode.O) && flashon)
            {
                flashlight.SetActive(false);
                flashon = false;
                PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("FlashOff", PhotonTargets.All, null);
            }
        }
        if (bulletType == BulletType.Nato)
        {
            CurrentAmmo = PlayerStats.NatoAmmo;
        }
        else if (bulletType == BulletType.Rifle)
        {
            CurrentAmmo = PlayerStats.RifleAmmo;
        }
        else
        {
            CurrentAmmo = PlayerStats.PistolAmmo;
        }
    }

	void GetPlayerInput()
	{
		if (PlayerInputManager.instance.Attack && Ammoleft >= 1 && CanSwitchWeaponMode && !WeaponAnimationComponent.IsPlaying (WeaponReloadAnimation.name) && PlayerMovementController.PlayerWalkingState != WalkingState.Running && !WeaponAnimationComponent.IsPlaying (WeaponDrawAnimation.name))
        {
			if (CurrentMode == WeaponType.bullet)
            {
				WeaponFire ();
			}
            else if (CurrentMode == WeaponType.burst && !Bursting)
            {
				WeaponBurstFire ();
			}
            else if (CurrentMode == WeaponType.shotgun)
            {
				WeaponFireShotgun ();
			}
		}
        else if (Input.GetKeyDown (PlayerInputManager.instance.FireButton) && Ammoleft == 0 && CanSwitchWeaponMode && !WeaponAnimationComponent.IsPlaying (WeaponReloadAnimation.name) && PlayerMovementController.PlayerWalkingState != WalkingState.Running && !WeaponAnimationComponent.IsPlaying (WeaponDrawAnimation.name) && !PlayerInputManager.instance.FreezePlayerControls && !PlayerWeaponManager.WeaponMovementAnimator.IsInTransition(0))
        {
			if (WeaponEmptySound != null)
            {
				WeaponAudiosource.PlayOneShot (WeaponEmptySound, 0.1f);
                PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("ThirdPersonEmpty", PhotonTargets.All, null);
            }
        }

		if (PlayerInputManager.instance.Reload && CurrentAmmo >= 1 && Ammoleft != ClipSize && !WeaponAnimationComponent.IsPlaying (WeaponReloadAnimation.name) && PlayerMovementController.PlayerWalkingState != WalkingState.Running && !WeaponAnimationComponent.IsPlaying (WeaponDrawAnimation.name) && !IsAiming)
        {
			ReloadWeapon();
		}
        if (!PlayerInputManager.instance.Crouch)
        {
            if (PlayerInputManager.instance.Aim && !WeaponAnimationComponent.IsPlaying(WeaponReloadAnimation.name) && PlayerMovementController.PlayerWalkingState != WalkingState.Running && !WeaponAnimationComponent.IsPlaying(WeaponDrawAnimation.name))
            {
                IsAiming = true;
                AimWeapon(true);
                RecoilModifier = 0.3f;
                triggerTime = 3f;
                triggerTime *= baseInaccuracyaim;
                PlayerInputManager.instance.AimSpeedModifier = AimSpeedModifier;
            }
            else
            {
                IsAiming = false;
                AimWeapon(false);
                RecoilModifier = 1f;
                triggerTime = 3f;
                triggerTime *= baseInaccuracyhip;
                PlayerInputManager.instance.AimSpeedModifier = 1f;
                if (!PlayerInputManager.instance.Sprint)
                {
                    DynamicCrosshair.instance.CurrentSpread = 40.0f;
                    DynamicCrosshair.instance.CurrentSpread *= baseInaccuracyhip;
                }
            }
        }
        else
        {
            if (PlayerInputManager.instance.Aim && !WeaponAnimationComponent.IsPlaying(WeaponReloadAnimation.name) && PlayerMovementController.PlayerWalkingState != WalkingState.Running && !WeaponAnimationComponent.IsPlaying(WeaponDrawAnimation.name))
            {
                IsAiming = true;
                AimWeapon(true);
                RecoilModifier = 0.3f;
                triggerTime = 3f;
                triggerTime *= baseInaccuracyaim / 2;
                PlayerInputManager.instance.AimSpeedModifier = AimSpeedModifier;
            }
            else
            {
                IsAiming = false;
                AimWeapon(false);
                RecoilModifier = 1f;
                triggerTime = 3f;
                triggerTime *= baseInaccuracyhip / 2;
                PlayerInputManager.instance.AimSpeedModifier = 1f;
                if (!PlayerInputManager.instance.Sprint)
                {
                    DynamicCrosshair.instance.CurrentSpread = 40.0f;
                    DynamicCrosshair.instance.CurrentSpread *= baseInaccuracyhip / 1.5f;
                }
            }
        }

		if(PlayerInputManager.instance.SwitchFireMode && SecondMode != WeaponType.none && CanSwitchWeaponMode && !WeaponAnimationComponent.IsPlaying (WeaponReloadAnimation.name) && !WeaponAnimationComponent.IsPlaying (WeaponDrawAnimation.name) && PlayerMovementController.PlayerWalkingState != WalkingState.Running)
        {
			if(CurrentMode != FirstMode)
            {
				StartCoroutine(SetPrimaryMode());
			}
            else
            {
				StartCoroutine(SetSecondaryMode());
			}
		}
	}

	void WeaponMovementAnimationController()
    {
		if (PlayerInputManager.instance.FreezePlayerControls == false)
        {
			if (PlayerMovementController.WasStanding && !PlayerCharacterController.isGrounded)
            {
				PlayerMovementController.WasStanding = false;
			}
            else if (!PlayerMovementController.WasStanding && PlayerCharacterController.isGrounded)
            {
				PlayerMovementController.WasStanding = true;
			}
			if (PlayerCharacterController.isGrounded && PlayerMovementController.PlayerMovementVelocity > 0f)
            {
				if (PlayerMovementController.PlayerWalkingState == WalkingState.Running)
                {
					WeaponMovementAnimator.SetFloat ("Movement", 1f, 0.2f, Time.deltaTime);
				}
                else if (PlayerMovementController.PlayerWalkingState == WalkingState.Walking)
                {
					WeaponMovementAnimator.SetFloat ("Movement", 0.5f, 0.2f, Time.deltaTime);
				}
			}
            else
            {
				WeaponMovementAnimator.SetFloat ("Movement", 0f, 0.2f, Time.deltaTime);
			}
		}
        else
        {
			WeaponMovementAnimator.SetFloat ("Movement", 0f, 0.2f, Time.deltaTime);
		}
	}

	IEnumerator SetPrimaryMode()
	{
		CanSwitchWeaponMode = false;
		if (WeaponSwitchAnimation != null)
        {
			WeaponAnimationComponent.Rewind (WeaponSwitchAnimation.name);
			WeaponAnimationComponent.Play (WeaponSwitchAnimation.name);
		}
		if (WeaponSwitchModeSound != null)
        {
            PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("ThirdPersonSwitch", PhotonTargets.All, null);
            WeaponAudiosource.clip = WeaponSwitchModeSound;
			WeaponAudiosource.Play ();
		}
		yield return new WaitForSeconds (WeaponSwitchAnimation.length);
		CurrentMode = FirstMode;
		WeaponCurrentFireSound = WeaponFirePrimarySound;
		WeaponAnimationComponent.CrossFade (WeaponPrimaryIdleAnimation.name, 0.2f);
		Weaponbarrel = FirstModeBarrel;
		FireRate = FirstModeFireRate;
		CanSwitchWeaponMode = true;
		if (FirstModeFiringType == WeaponFiringType.auto)
        {
			PlayerInputManager.instance.IsAuto = true;
		}
        else if (FirstModeFiringType == WeaponFiringType.semi)
        {
			PlayerInputManager.instance.IsAuto = false;
		}
		CurrentFiringType = FirstModeFiringType;
	}

	IEnumerator SetSecondaryMode()
	{
		CanSwitchWeaponMode = false;
		if (WeaponSwitchAnimation != null)
        {
			WeaponAnimationComponent.Rewind (WeaponSwitchAnimation.name);
			WeaponAnimationComponent.Play (WeaponSwitchAnimation.name);
		}
		if (WeaponSwitchModeSound != null)
        {
			WeaponAudiosource.clip = WeaponSwitchModeSound;
			WeaponAudiosource.Play ();
		}
		yield return new WaitForSeconds (WeaponSwitchAnimation.length);
		CurrentMode = SecondMode;
		WeaponCurrentFireSound = WeaponFireSecondarySound;
		WeaponAnimationComponent.CrossFade (WeaponSecondaryIdleAnimation.name, 1f);
		Weaponbarrel = SecondModeBarrel;
		FireRate = SecondModeFireRate;
		CanSwitchWeaponMode = true;
		if (SecondModeFiringType == WeaponFiringType.auto)
        {
			PlayerInputManager.instance.IsAuto = true;
		}
        else if (SecondModeFiringType == WeaponFiringType.semi)
        {
			PlayerInputManager.instance.IsAuto = false;
		}
		CurrentFiringType = SecondModeFiringType;
	}

    public void SetModules(bool muzzle, bool acog, bool reddot, bool sil)
    {
        if (muzzle)
        {
            if (!Muzzle_break)
            {
                if (Muzzle_br != null)
                {
                    Muzzle_break = true;
                    Muzzle_br.SetActive(true);
                    WeaponCurrentFireSound = WeaponFirePrimarySound;
                    PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("SetModules", PhotonTargets.All, true, false, false, false);
                    if (Silencer)
                    {
                        Silencer = false;
                        Sil.SetActive(false);
                    }
                }
            }
        }
        if (acog)
        {
            if (!ACOG)
            {
                if (Acog != null)
                {
                    if (Red_dot)
                    {
                        Red_dot = false;
                        Red_Dot.SetActive(false);
                    }
                    ACOG = true;
                    Acog.SetActive(true);
                    AimPosition = ACOGAimPosition;
                    PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("SetModules", PhotonTargets.All, false, true, false, false);
                    if (NeedToHide && Scope != null && HidenScope != null)
                    {
                        Scope.SetActive(false);
                        HidenScope.SetActive(true);
                    }
                    if (NeedToHide && Scope == null && HidenScope != null)
                    {
                        Scope.SetActive(false);
                    }
                }
            }
        }
        if (reddot)
        {
            if (!Red_dot)
            {
                if (Red_Dot != null)
                {
                    if (ACOG)
                    {
                        ACOG = false;
                        Acog.SetActive(false);
                    }
                    Red_dot = true;
                    Red_Dot.SetActive(true);
                    AimPosition = RedDotAimPosition;
                    PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("SetModules", PhotonTargets.All, false, false, true, false);
                    if (NeedToHide && Scope != null && HidenScope != null)
                    {
                        Scope.SetActive(false);
                        HidenScope.SetActive(true);
                    }
                    if (NeedToHide && Scope == null && HidenScope != null)
                    {
                        Scope.SetActive(false);
                    }
                }
            }
        }
        if (sil)
        {
            if (!Silencer)
            {
                if (Sil != null)
                {
                    Silencer = true;
                    Sil.SetActive(true);
                    WeaponCurrentFireSound = GameManager.instance.SilSound;
                    PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("SetModules", PhotonTargets.All, false, false, false, true);
                    if (Muzzle_break)
                    {
                        Muzzle_break = false;
                        Muzzle_br.SetActive(false);
                    }
                }
            }
        }
    }

	public void ReloadWeapon()
	{
		if (!WeaponAnimationComponent.IsPlaying (WeaponReloadAnimation.name) && WeaponReloadAnimation != null)
        {
			PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("ThirdPersonReload", PhotonTargets.All, null);
			if (WeaponReloadSound != null)
            {
				WeaponAudiosource.PlayOneShot (WeaponReloadSound, 1f);
			}
			WeaponAnimationComponent.Play (WeaponReloadAnimation.name);
			Invoke ("WaitTillReload", WeaponReloadAnimation.length);
		}
	}

	void WaitTillReload()
	{
        PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("FinishReload", PhotonTargets.Others, null);
        if (bulletType == BulletType.Nato)
        {
            if ((CurrentAmmo - ClipSize + Ammoleft) < 0)
            {
                Ammoleft = Ammoleft + CurrentAmmo;
                PlayerStats.NatoAmmo = 0;
            }
            else
            {
                PlayerStats.NatoAmmo = CurrentAmmo - ClipSize + Ammoleft;
                Ammoleft = ClipSize;
            }
        }
        else if (bulletType == BulletType.Rifle)
        {
            if ((CurrentAmmo - ClipSize + Ammoleft) < 0)
            {
                Ammoleft = Ammoleft + CurrentAmmo;
                PlayerStats.RifleAmmo = 0;
            }
            else
            {
                PlayerStats.RifleAmmo = CurrentAmmo - ClipSize + Ammoleft;
                Ammoleft = ClipSize;
            }
        }
        else
        {
            if ((CurrentAmmo - ClipSize + Ammoleft) < 0)
            {
                Ammoleft = Ammoleft + CurrentAmmo;
                PlayerStats.PistolAmmo = 0;
            }
            else
            {
                PlayerStats.PistolAmmo = CurrentAmmo - ClipSize + Ammoleft;
                Ammoleft = ClipSize;
            }
        }
    }
    
	void AimWeapon(bool State)
	{
        ToggleScopeCamera(State);
        if (State)
        {
			IsAiming = true;
			WeaponHolder.localPosition = Vector3.Lerp (WeaponHolder.localPosition, AimPosition, 0.25f);
			PlayerCamera.fieldOfView = Mathf.Lerp (PlayerCamera.fieldOfView, AimFov, 0.25f);
			InGameUI.instance.Crosshair.SetActive (false);
		}
        else if (!State && !PlayerInputManager.instance.Zoom)
        {
			IsAiming = false;
            WeaponHolder.localPosition = Vector3.Lerp(WeaponHolder.localPosition, WeaponOffset, 0.25f);
			PlayerCamera.fieldOfView = Mathf.Lerp(PlayerCamera.fieldOfView, 65, 0.25f);
			InGameUI.instance.Crosshair.SetActive (true);
		}
        else if (PlayerInputManager.instance.Zoom)
        {
            IsAiming = false;
            WeaponHolder.localPosition = Vector3.Lerp(WeaponHolder.localPosition, WeaponOffset, 0.25f);
            PlayerCamera.fieldOfView = Mathf.Lerp(PlayerCamera.fieldOfView, 40, 0.25f);
            InGameUI.instance.Crosshair.SetActive(true);
        }
	}
	
	void WeaponFire()
	{
		if (ShootTimer <= Time.time)
        {
			ShootTimer = Time.time + FireRate;
			WeaponAudiosource.PlayOneShot (WeaponCurrentFireSound);
			CurrentRecoil1 += new Vector3(RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y));
			CurrentRecoil3 += new Vector3(Random.Range(-RecoilKickback.x, RecoilKickback.x), Random.Range(-RecoilKickback.y, RecoilKickback.y), RecoilKickback.z);

			if (CurrentMode == FirstMode)
            {
				WeaponAnimationComponent.CrossFadeQueued (WeaponFireAnimation.name, 0.01f, QueueMode.PlayNow);
			}
			else if (CurrentMode == SecondMode)
            {
				WeaponAnimationComponent.CrossFadeQueued (WeaponFireSecondaryAnimation.name, 0.01f, QueueMode.PlayNow);
			}
			if (WeaponShellEject != null)
            {
				WeaponShellEject.Play ();
			}
			if (WeaponPrimaryMuzzleFlash != null)
            {
				WeaponPrimaryMuzzleFlash.Play ();
			}
			PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC ("ThirdPersonFireWeapon", PhotonTargets.Others, null);
			FireBullet ();
			Ammoleft--;
		}
	}

	void WeaponBurstFire()
	{
		if (ShootTimer <= Time.time && !Bursting)
        {
			ShootTimer = Time.time + FireRate;
			StartCoroutine (Burst ());
		}
	}

	IEnumerator Burst()
	{
		int shotcounter = 0;
		while (shotcounter < shotsPerBurst)
        {
			Bursting = true;
			shotcounter++;
			if (Ammoleft > 0)
            {
				CurrentRecoil1 += new Vector3(RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y));
				CurrentRecoil3 += new Vector3(Random.Range(-RecoilKickback.x, RecoilKickback.x), Random.Range(-RecoilKickback.y, RecoilKickback.y), RecoilKickback.z);

				if (CurrentMode == FirstMode)
                {
					WeaponAnimationComponent.CrossFadeQueued (WeaponFireAnimation.name, 0.01f, QueueMode.PlayNow);
				}
				else if (CurrentMode == SecondMode)
                {
					WeaponAnimationComponent.CrossFadeQueued (WeaponFireSecondaryAnimation.name, 0.01f, QueueMode.PlayNow);
				}
				WeaponAudiosource.PlayOneShot (WeaponCurrentFireSound);
				if (WeaponShellEject != null)
                {
					WeaponShellEject.Play ();
				}
				if (WeaponPrimaryMuzzleFlash != null)
                {
					WeaponPrimaryMuzzleFlash.Play ();
				}
				PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC ("ThirdPersonFireWeapon", PhotonTargets.All, null);
				FireBullet ();
				Ammoleft--;
			}
			yield return new WaitForSeconds (BurstTime);
		}
		yield return new WaitForSeconds (0.2f);
		Bursting = false;
	}

    void FireBullet()
    {
        int CurrentDamage = Damage;
        Vector3 Spread = new Vector3 (Random.Range (-0.01f, 0.01f) * triggerTime, Random.Range (-0.01f, 0.01f) * triggerTime, 1f);
		Vector3 Direction = Weaponbarrel.TransformDirection(Spread);
		Ray WeaponRay =	 new Ray(Weaponbarrel.position, Direction);
		RaycastHit WeaponHit;
        PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("ThirdPersonImpacts", PhotonTargets.Others, Spread, Direction, Weaponbarrel.position);
        //WeaponHit = Physics.RaycastAll(WeaponRay, WeaponRange, WeaponLayerMask).OrderBy(h => h.distance).ToArray();
        //for (int i = 0; i < WeaponHit.Length; i++)
        //{
        if (Physics.Raycast(WeaponRay, out WeaponHit, WeaponRange, WeaponLayerMask))
        {
            Debug.Log(WeaponHit.distance);
            if ((int)WeaponHit.distance > ShootDistance)
            {
                int CurrentDistance = ((int)WeaponHit.distance - ShootDistance) / 7;
                CurrentDamage -= CurrentDistance;
            }
            if (CurrentDamage < 5)
            {
                CurrentDamage = 5;
            }
            Debug.Log(string.Format("{0} {1}", CurrentDamage, Damage));
            if (WeaponHit.transform.GetComponent<Rigidbody>() != null)
            {
                WeaponHit.transform.GetComponent<Rigidbody>().AddForce(WeaponImpactForce * Direction, ForceMode.Impulse);
                //break;
            }
            if (WeaponHit.collider.tag == "Untagged" || WeaponHit.collider.tag == "Concrete")
            {
                GameObject ConcreteHole = Instantiate(ConcreteImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                ConcreteHole.transform.parent = WeaponHit.transform;
                //BulletHP -= 4;
                PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("ThirdPersonImpacts", PhotonTargets.Others, Spread, Direction, Weaponbarrel.position);
            }
            if (WeaponHit.collider.tag == "Metal" || WeaponHit.collider.tag == "Vehicle" || WeaponHit.collider.tag == "MetalDoor")
            {
                GameObject MetalHole = Instantiate(MetalImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                MetalHole.transform.parent = WeaponHit.transform;
                //BulletHP -= 3;
                PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("ThirdPersonImpacts", PhotonTargets.Others, Spread, Direction, Weaponbarrel.position);
            }
            if (WeaponHit.collider.tag == "Sand")
            {
                GameObject SandHole = Instantiate(SandImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                SandHole.transform.parent = WeaponHit.transform;
                //BulletHP -= 5;
            }
            if (WeaponHit.collider.tag == "Wood" || WeaponHit.collider.tag == "WoodenDoor")
            {
                GameObject WoodHole = Instantiate(WoodImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                WoodHole.transform.parent = WeaponHit.transform;
                //BulletHP -= 2;
                PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("ThirdPersonImpacts", PhotonTargets.Others, Spread, Direction, Weaponbarrel.position);
            }
            if (WeaponHit.collider.tag == "Water")
            {
                GameObject WaterHole = Instantiate(WaterImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                WaterHole.transform.parent = WeaponHit.transform;
                //BulletHP -= 1;
            }
            if (WeaponHit.collider.tag == "Glass")
            {
                GameObject GlassHole = Instantiate(GlassImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                GlassHole.transform.parent = WeaponHit.transform;
                WeaponHit.transform.GetComponent<PhotonView>().RPC("ApplyDamage", PhotonTargets.All, CurrentDamage, PhotonNetwork.player);
                //BulletHP -= 1;
                PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("ThirdPersonImpacts", PhotonTargets.Others, Spread, Direction, Weaponbarrel.position);
            }
            if (WeaponHit.collider.tag == "BreakedGlass")
            {
                GameObject GlassHole = Instantiate(GlassImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                GlassHole.transform.parent = WeaponHit.transform;
                //BulletHP -= 1;
                PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("ThirdPersonImpacts", PhotonTargets.Others, Spread, Direction, Weaponbarrel.position);
            }
            if (WeaponHit.collider.tag == "Ground")
            {
                GameObject SandHole = Instantiate(SandImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                SandHole.transform.parent = WeaponHit.transform;
                //BulletHP -= 3;
            }
            //if (WeaponHit.collider.tag == "Vehicle") 
            //{ //&& WeaponHit.transform.root.GetComponent<VehicleStats> ().VehicleAlive == true
            //	GameObject MetalHole = Instantiate (MetalImpact , WeaponHit.point, Quaternion.FromToRotation (Vector3.forward, WeaponHit.normal));
            //	MetalHole.transform.parent = WeaponHit.transform;
            //	WeaponHit.transform.root.GetComponent<VehicleStats> ().VehiclePhotonView.RPC ("FinishVehicleDamage", PhotonTargets.All, CurrentDamage ,WeaponName, PhotonNetwork.player);
            //}
            if (WeaponHit.collider.tag == "DestroyableObject")
            {
                WeaponHit.transform.GetComponent<PhotonView>().RPC("ApplyDamage", PhotonTargets.All, CurrentDamage);
                //break;
            }
            if (WeaponHit.collider.tag == "Bot")
            {
                GameObject FleshHole = Instantiate(FleshImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                FleshHole.transform.parent = WeaponHit.transform;
                InGameUI.instance.DoHitMarker();
                WeaponHit.transform.root.GetComponent<PhotonView>().RPC("ApplyBotDamage", PhotonTargets.All, CurrentDamage, WeaponName, PhotonNetwork.player, 1.0f, false);
                //BulletHP -= 2;
            }
            if (WeaponHit.collider.tag == "PlayerHitbox")
            {
                GameObject FleshHole = Instantiate(FleshImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                FleshHole.transform.parent = WeaponHit.transform;
                if (WeaponHit.transform.tag == "PlayerHitbox" && WeaponHit.transform.root.GetComponent<PlayerStats>() != null && WeaponHit.transform.root.GetComponent<PlayerStats>().isAlive)
                {
                    InGameUI.instance.DoHitMarker();
                    WeaponHit.transform.root.GetComponent<PhotonView>().RPC("ApplyPlayerDamage", PhotonTargets.All, CurrentDamage, WeaponName, PhotonNetwork.player, WeaponHit.transform.GetComponent<PlayerBodyPartMultiplier>().DamageModifier, false);
                }
                //BulletHP -= 2;
                PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("ThirdPersonImpacts", PhotonTargets.Others, Spread, Direction, Weaponbarrel.position);
            }
            if (WeaponHit.collider.tag == "PolygonTarget")
            {
                GameObject WoodHole = Instantiate(WoodImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                WoodHole.transform.parent = WeaponHit.transform;
                InGameUI.instance.DoHitMarker();
                PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("ThirdPersonImpacts", PhotonTargets.Others, Spread, Direction, Weaponbarrel.position);
            }
        }
        //else
        //{
        //    BulletHP = 5;
        //    break;
        //}
        //}
    }

	void WeaponFireShotgun()
	{
		if (ShootTimer <= Time.time)
        {
			ShootTimer = Time.time + FireRate;
			WeaponAudiosource.PlayOneShot (WeaponCurrentFireSound);
			CurrentRecoil1 += new Vector3 (RecoilRotation.x, Random.Range (-RecoilRotation.y, RecoilRotation.y));
			CurrentRecoil3 += new Vector3 (Random.Range (-RecoilKickback.x, RecoilKickback.x), Random.Range (-RecoilKickback.y, RecoilKickback.y), RecoilKickback.z);

			if (CurrentMode == FirstMode)
            {
				WeaponAnimationComponent.CrossFadeQueued (WeaponFireAnimation.name, 0.01f, QueueMode.PlayNow);
			}
            else if (CurrentMode == SecondMode)
            {
				WeaponAnimationComponent.CrossFadeQueued (WeaponFireSecondaryAnimation.name, 0.01f, QueueMode.PlayNow);
			}
			if (WeaponShellEject != null)
            {
				WeaponShellEject.Play ();
			}
			if (WeaponPrimaryMuzzleFlash != null)
            {
				WeaponPrimaryMuzzleFlash.Play ();
			}
			int pellets = 0;
			while (pellets < pelletsPerShot)
            {
				FirePellet ();
				pellets++;
			}
            PlayerMovementController.PlayerThirdPersonController.ThirdPersonPhotonView.RPC("ThirdPersonFireWeapon", PhotonTargets.Others, null);
            FireBullet();
            Ammoleft--;
		}
	}

	void FirePellet()
	{
		Vector3 Spread = new Vector3 (Random.Range (-0.01f, 0.01f) * 5f, Random.Range (-0.01f, 0.01f) * 5f, 1f);
		Vector3 Direction = Weaponbarrel.TransformDirection(Spread);
		Ray WeaponRay =	 new Ray(Weaponbarrel.position, Direction);
		RaycastHit WeaponHit;
		if (Physics.Raycast (WeaponRay, out WeaponHit, WeaponRange, WeaponLayerMask))
        {
			if (WeaponHit.transform.GetComponent<Rigidbody> () != null)
            {
				WeaponHit.transform.GetComponent<Rigidbody> ().AddForce (WeaponImpactForce * Direction, ForceMode.Impulse);
			}
			if (WeaponHit.collider.tag == "Untagged" || WeaponHit.collider.tag == "Concrete")
            {
				GameObject ConcreteHole = Instantiate (ConcreteImpact , WeaponHit.point, Quaternion.FromToRotation (Vector3.forward, WeaponHit.normal));
				ConcreteHole.transform.parent = WeaponHit.transform;
			}
			if (WeaponHit.collider.tag == "Metal" || WeaponHit.collider.tag == "Vehicle" || WeaponHit.collider.tag == "MetalDoor")
            {
				GameObject MetalHole = Instantiate (MetalImpact , WeaponHit.point, Quaternion.FromToRotation (Vector3.forward, WeaponHit.normal));
				MetalHole.transform.parent = WeaponHit.transform;
			}
			if (WeaponHit.collider.tag == "Sand")
            {
				GameObject SandHole = Instantiate (SandImpact , WeaponHit.point, Quaternion.FromToRotation (Vector3.forward, WeaponHit.normal));
				SandHole.transform.parent = WeaponHit.transform;
			}
			if (WeaponHit.collider.tag == "Wood" || WeaponHit.collider.tag == "WoodenDoor")
            {
				GameObject WoodHole = Instantiate (WoodImpact , WeaponHit.point, Quaternion.FromToRotation (Vector3.forward, WeaponHit.normal));
				WoodHole.transform.parent = WeaponHit.transform;
			}
			if (WeaponHit.collider.tag == "Water")
            {
				GameObject WaterHole = Instantiate (WaterImpact , WeaponHit.point, Quaternion.FromToRotation (Vector3.forward, WeaponHit.normal));
				WaterHole.transform.parent = WeaponHit.transform;
			}
            if (WeaponHit.collider.tag == "Glass")
            {
                GameObject GlassHole = Instantiate(GlassImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                GlassHole.transform.parent = WeaponHit.transform;
                WeaponHit.transform.GetComponent<PhotonView>().RPC("ApplyDamage", PhotonTargets.All, Damage, PhotonNetwork.player);
            }
            if (WeaponHit.collider.tag == "BreakedGlass")
            {
                GameObject GlassHole = Instantiate(GlassImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                GlassHole.transform.parent = WeaponHit.transform;
            }
            //if (WeaponHit.collider.tag == "Vehicle") 
            //{ //&& WeaponHit.transform.root.GetComponent<VehicleStats> ().VehicleAlive == true
            //	GameObject MetalHole = Instantiate (MetalImpact , WeaponHit.point, Quaternion.FromToRotation (Vector3.forward, WeaponHit.normal));
            //	MetalHole.transform.parent = WeaponHit.transform;
            //	WeaponHit.transform.root.GetComponent<VehicleStats> ().VehiclePhotonView.RPC ("FinishVehicleDamage", PhotonTargets.All, Damage ,WeaponName, PhotonNetwork.player);
            //}
            if (WeaponHit.collider.tag == "DestroyableObject")
            {
                WeaponHit.transform.GetComponent<PhotonView>().RPC("ApplyDamage", PhotonTargets.All, Damage);
            }
            if (WeaponHit.collider.tag == "Bot")
            {
                GameObject FleshHole = Instantiate(FleshImpact, WeaponHit.point, Quaternion.FromToRotation(Vector3.forward, WeaponHit.normal));
                FleshHole.transform.parent = WeaponHit.transform;
                InGameUI.instance.DoHitMarker();
                WeaponHit.transform.root.GetComponent<PhotonView>().RPC("ApplyBotDamage", PhotonTargets.All, Damage, WeaponName, PhotonNetwork.player, 1.0f, false);
            }
            if (WeaponHit.collider.tag == "PlayerHitbox")
            {
				GameObject FleshHole = Instantiate (FleshImpact , WeaponHit.point, Quaternion.FromToRotation (Vector3.forward, WeaponHit.normal));
				FleshHole.transform.parent = WeaponHit.transform;
				if (WeaponHit.transform.tag == "PlayerHitbox" && WeaponHit.transform.root.GetComponent<PlayerStats> () != null && WeaponHit.transform.root.GetComponent<PlayerStats> ().isAlive)
                {
					InGameUI.instance.DoHitMarker ();
					WeaponHit.transform.root.GetComponent<PhotonView> ().RPC ("ApplyPlayerDamage", PhotonTargets.All, Damage, WeaponName, PhotonNetwork.player, WeaponHit.transform.GetComponent<PlayerBodyPartMultiplier>().DamageModifier, false);
				}
			}
		}
	}
    
	public void RecoilController()
	{
		CurrentRecoil1 = Vector3.Lerp(CurrentRecoil1, Vector3.zero, 0.1f);
		CurrentRecoil2 = Vector3.Lerp(CurrentRecoil2, CurrentRecoil1, 0.1f);
		CurrentRecoil3 = Vector3.Lerp(CurrentRecoil3, Vector3.zero, 0.1f);
		CurrentRecoil4 = Vector3.Lerp(CurrentRecoil4, CurrentRecoil3, 0.1f);

		WeaponRecoilHolder.localEulerAngles = CurrentRecoil2 * RecoilModifier;
		WeaponRecoilHolder.localPosition = CurrentRecoil4 * RecoilModifier;
		PlayerCamera.transform.localEulerAngles = CurrentRecoil2 / 1.2f * RecoilModifier;
	}

	void UpdateWeaponHud()
	{
		InGameUI.instance.WeaponNameText.text = WeaponName;
		InGameUI.instance.WeaponCurrentMagazineAmmoText.text = Ammoleft.ToString();
		InGameUI.instance.WeaponReserveClipsText.text = CurrentAmmo.ToString();
	}

    void ToggleScopeCamera(bool toggle)
    {
        if (ACOG)
        {
            Acog.GetComponent<ScopeCamera>().RenderTexture.SetActive(toggle);
            Acog.GetComponent<ScopeCamera>().Camera.SetActive(toggle);
        }
    }

    [System.Serializable]
    public enum BulletType
    {
        Nato, Rifle, Pistol
    }
}
