using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour {

	[Header("Контроллер игрока")]
	public CharacterController PlayerCharacterController;
	public PlayerMovementController PlayerMovementController;
	public PlayerThirdPersonController ThirdPersonController;
	public PhotonView PlayerPhotonView;
	public AudioSource WeaponAudioSource;

	[Header("Контроллер оружия")]
	public Animator WeaponMovementAnimator;
	public Transform WeaponRecoilHolder;
	public Transform WeaponOffsetTransform;

	[Header("Инвентарь")]
    public List<GameObject> BackWeaponHolder = new List<GameObject>();
    public List<GameObject> PlayerInventory = new List<GameObject>();
	public Transform WeaponHolder;
	[HideInInspector]public GameObject CurrentWeapon;
	[HideInInspector]public int CurrentWeaponInt = 1;
	[HideInInspector]public Animation CurrentWeaponAnimationComponent;
	[HideInInspector]public AnimationClip CurrentWeaponReload;
	[HideInInspector]public AnimationClip CurrentWeaponDraw;

	void Update()
	{
		//WeaponMovementAnimationController ();
		SwitchController ();
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
		} else {
			WeaponMovementAnimator.SetFloat ("Movement", 0f, 0.2f, Time.deltaTime);
		}
	}

	public void GiveWeapon(int WeaponID)
	{
		if (GameManager.instance.AllGameWeapons [WeaponID] != null)
        {
			GameObject newWep = Instantiate (GameManager.instance.AllGameWeapons[WeaponID].FirstPersonPrefab, WeaponHolder) as GameObject;
			PlayerInventory.Add (newWep);
			newWep.SetActive (false);
		}
        else if(GameManager.instance.AllGameWeapons [WeaponID] == null)
        {
			Debug.LogError ("Cant find: unknown of type weaponfile with that name!");
		}
	}

    public void GiveWeapon2(int WeaponID,int ind)//???????????????????????????????????????????????
    {
        if (GameManager.instance.AllGameWeapons[WeaponID] != null)
        {
            Destroy(PlayerInventory[ind]);
            GameObject newWep = Instantiate(GameManager.instance.AllGameWeapons[WeaponID].FirstPersonPrefab, WeaponHolder) as GameObject;
            PlayerInventory[ind] = newWep;
            newWep.SetActive(false);
        }
        else if (GameManager.instance.AllGameWeapons[WeaponID] == null)
        {
            Debug.LogError("Cant find: unknown of type weaponfile with that name!");
        }
    }

    public void SwitchController()
	{
		if (PlayerInputManager.instance.NextWeapon && !CurrentWeaponAnimationComponent.IsPlaying(CurrentWeaponDraw.name) && PlayerInputManager.instance.Aim == false && PlayerInventory.Count > 1)
        {
			if (CurrentWeaponInt == PlayerInventory.Count - 1)
            {
				CurrentWeaponInt = 0;
			}
            else 
			{
				CurrentWeaponInt++;
			}
			if (CurrentWeaponAnimationComponent.IsPlaying (CurrentWeaponReload.name))
            {
				CancelInvoke ();
			}
			EquipWeapon (CurrentWeaponInt);
		}
	}

	public void TakeWeapon(int WeaponSlot)
	{
		if (PlayerInventory [WeaponSlot] != null)
        {
			DisableAllWeapons ();
			Destroy (PlayerInventory [WeaponSlot]);
			PlayerInventory.RemoveAt (WeaponSlot);
		}
	}

	void DisableAllWeapons()
	{
		foreach (GameObject Weapon in PlayerInventory)
        {
			Weapon.SetActive (false);
		}
	}

	public void EquipWeapon(int WeaponSlot)
	{
        //if (PlayerInventory[WeaponSlot].GetComponent<PlayerWeapon>().WeaponMovementType != PlayerWeapon.MovementType.Pistol)
        //{
        //    if (WeaponSlot == 0)
        //    {
        //        GameObject GO = PhotonNetwork.Instantiate(GameManager.instance.WeaponModels[PlayerInventory[1].GetComponent<PlayerWeapon>().WeaponID].name, BackWeaponHolder[1].transform.position, new Quaternion(-90, -90, 0, 0), 0);
        //        GO.transform.parent = BackWeaponHolder[1].transform;
        //        BackWeaponHolder[1].GetComponent<SpineWeaponHolder>().Weapon = GO;
        //        PhotonNetwork.Destroy(BackWeaponHolder[0].GetComponent<SpineWeaponHolder>().Weapon);
        //    }
        //    else
        //    {
        //        GameObject GO = PhotonNetwork.Instantiate(GameManager.instance.WeaponModels[PlayerInventory[0].GetComponent<PlayerWeapon>().WeaponID].name, BackWeaponHolder[0].transform.position, new Quaternion(-90, -90, 0, 0), 0);
        //        GO.transform.parent = BackWeaponHolder[0].transform;
        //        BackWeaponHolder[0].GetComponent<SpineWeaponHolder>().Weapon = GO;
        //        PhotonNetwork.Destroy(BackWeaponHolder[1].GetComponent<SpineWeaponHolder>().Weapon);
        //    }
        //}
        CurrentWeaponInt = WeaponSlot;
		DisableAllWeapons ();
		CurrentWeapon = PlayerInventory [WeaponSlot];
		CurrentWeapon.SetActive (true);
		WeaponOffsetTransform.localPosition = CurrentWeapon.GetComponent<PlayerWeapon>().WeaponOffset;
		CurrentWeaponAnimationComponent = CurrentWeapon.GetComponent<PlayerWeapon> ().WeaponAnimationComponent;
		CurrentWeapon.GetComponent<PlayerWeapon>().Bursting = false;
		CurrentWeaponReload = CurrentWeapon.GetComponent<PlayerWeapon> ().WeaponReloadAnimation;
		CurrentWeaponDraw = CurrentWeapon.GetComponent<PlayerWeapon> ().WeaponDrawAnimation;
		WeaponAudioSource.PlayOneShot(CurrentWeapon.GetComponent<PlayerWeapon> ().WeaponDrawSound, 0.05f);
		ThirdPersonController.ThirdPersonPhotonView.RPC("SetThirdPersonWeapon", PhotonTargets.AllBuffered ,CurrentWeapon.GetComponent<PlayerWeapon> ().WeaponID);
	}
}

