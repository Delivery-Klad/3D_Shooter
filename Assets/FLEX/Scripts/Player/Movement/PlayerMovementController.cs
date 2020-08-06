using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovementController: MonoBehaviour {

	[Header("Player References")]
	public MouseLook PlayerMouseLook;
	public Transform PlayerCamera;
	public Transform PlayerWeaponHolder;
	public PlayerWeaponManager PlayerWeaponManager;
	public PlayerThirdPersonController PlayerThirdPersonController;
	public PlayerStats PlayerStatistics;
	public GameObject PlayerLegs;
	public Animator PlayerLegsAnimator;
	public PhotonView PlayerPhotonView;

	[Header("Player Movement Variables")]
	public float walkSpeed = 6.0f;
	public float runSpeed = 11.0f;
	public bool InWater = false;
    public bool InWood = false;
    public bool InWood2 = false;
    public bool InSand = false;
    public bool InMetal = false;
    public bool InStone = false;
    public bool InStone2 = false;
    private int matIndex1 = 0;
    private int matIndex2 = 0;
    public bool limitDiagonalSpeed = true;
	private Vector3 moveDirection = Vector3.zero;
	[SerializeField]private bool grounded = false;
	public CharacterController controller;
	private Transform myTransform;
	[SerializeField]private float PlayerMovementSpeed;
	public float PlayerMovementVelocity;
	private RaycastHit hit;
	public float NormalGravity;
	public WalkingState PlayerWalkingState;
	private float DistanceToObstacle;

	[Header("Player Jump Variables")]
	public float jumpSpeed = 8.0f;
	public float gravity = 20.0f;
	public float fallingDamageThreshold = 10.0f;
	private float fallStartLevel;
	public bool falling;
	public bool WasStanding = false;
	private float slideLimit;

	[Header("Player Crouch Behaviour")]
	public PlayerStance PlayerStanceState;
	public Vector3 PlayerCenterOffset;
	public Vector3 PlayerCameraOffset;
	public float PlayerNormalHeight = 1.8f;
	public float PlayerCrouchSpeed = 3f;
	public float PlayerCrouchedHeight = 0.8f;

	[Header("Player Leaning Behaviour")]
	public Transform LeaningPivotTransform;
	public bool isLeaning = false;
	public float LeanSpeed = 100f;
	public float MaxLeanAngle = 20f;
	private float CurrentLeanAngle = 0f;

	[Header("Player Sliding Variables")]
	public bool slideWhenOverSlopeLimit = false;
	public bool slideOnTaggedObjects = false;
	public float slideSpeed = 12.0f;
	public bool airControl = false;
	public float antiBumpFactor = .75f;
	public int antiBunnyHopFactor = 1;
	private float rayDistance;
	private Vector3 contactPoint;
	private bool playerControl = false;
	private int jumpTimer;

	[Header("Player Camera Animation")]
	public Animation PlayerCameraAnimationComponent;
	public AnimationClip PlayerCameraLand;
	public AnimationClip PlayerCameraRun;

    public Transform RayPoint;
    public RaycastHit StepHit;
    public int RayTimer = 0;
    public string StepTag = "null";
    public TerrainLayer Stone;

    public RaycastHit FallHit;
    public int FallTimer = 0;
    private bool StaminaTimerOn = false;
    private float StaminaTimer = 0;

    void Awake()
	{
		PlayerMouseLook.Init (transform, PlayerWeaponHolder);
	}

	void Start() {
		//controller = GetComponent<CharacterController>();
		PlayerLegs.SetActive(true);
		myTransform = transform;
		PlayerMovementSpeed = walkSpeed;
		rayDistance = controller.height * .5f + controller.radius;
		slideLimit = controller.slopeLimit - .1f;
		jumpTimer = antiBunnyHopFactor;
	}

	void Update()
    {
		RotateView ();
		PlayerLeaning ();
		PlayerMovement ();
        if (Input.GetKeyDown(KeyCode.L) && !PlayerInputManager.instance.Console)
        {
            LOG();
        }
        if (RayTimer%17==0)
        {
            Debug.DrawRay(RayPoint.transform.position, -transform.up * 1f, Color.red);
            if (Physics.Raycast(RayPoint.transform.position, -transform.up, out StepHit, 1f))
            {
                if (StepHit.transform.tag != StepTag && StepHit.transform.tag=="Metal")
                {
                    StepTag = StepHit.transform.tag;
                    matIndex1 = -1;
                    matIndex2 = -1;
                    PlayerExitWater();
                    PlayerExitWood();
                    PlayerExitStone();
                    PlayerExitSand();
                    PlayerEnterMetal();
                }
                if (StepHit.transform.tag != StepTag && StepHit.transform.tag == "Untagged")
                {
                    StepTag = StepHit.transform.tag;
                    matIndex1 = -1;
                    matIndex2 = -1;
                    PlayerExitWater();
                    PlayerExitWood();
                    PlayerExitSand();
                    PlayerExitMetal();
                    PlayerEnterStone();
                }
                if (StepHit.transform.tag != StepTag && StepHit.transform.tag == "Wood")
                {
                    StepTag = StepHit.transform.tag;
                    matIndex1 = -1;
                    matIndex2 = -1;
                    PlayerExitWater();
                    PlayerExitStone();
                    PlayerExitSand();
                    PlayerExitMetal();
                    PlayerEnterWood();
                }
                if (StepHit.transform.tag != StepTag && StepHit.transform.tag == "Sand")
                {
                    StepTag = StepHit.transform.tag;
                    matIndex1 = -1;
                    matIndex2 = -1;
                    PlayerExitWater();
                    PlayerExitWood();
                    PlayerExitStone();
                    PlayerExitMetal();
                    PlayerEnterSand();
                }
                if (StepHit.transform.tag != StepTag && StepHit.transform.tag == "Water")
                {
                    StepTag = StepHit.transform.tag;
                    matIndex1 = -1;
                    matIndex2 = -1;
                    PlayerExitWood();
                    PlayerExitStone();
                    PlayerExitSand();
                    PlayerExitMetal();
                    PlayerEnterWater();
                }
                if (StepHit.transform.tag == "Ground") // если тэг земля
                {
                    StepTag = StepHit.transform.tag;
                    Terr t = hit.transform.GetComponent<Terr>();// скрипт с террэйна
                    if (t != null)
                    {
                        matIndex1 = t.GetMaterialIndex(hit.point);// индекс текстуры
                    }
                    if (matIndex1 != matIndex2)
                    {
                        matIndex2 = matIndex1;
                        if (matIndex2 == 0)
                        {
                            PlayerExitWood();
                            PlayerExitStone();
                            PlayerExitSand();
                            PlayerExitMetal();
                            PlayerExitWater();
                        }
                        else if (matIndex2 == 1)
                        {
                            PlayerExitWood();
                            PlayerExitStone();
                            PlayerExitSand();
                            PlayerExitMetal();
                            PlayerExitWater();
                        }
                        else if (matIndex2 == 2)
                        {
                            PlayerExitWater();
                            PlayerExitWood();
                            PlayerExitSand();
                            PlayerExitMetal();
                            PlayerEnterStone();
                        }
                    }
                }
            }
        }
        RayTimer++;
        if (StaminaTimerOn)
        {
            StaminaTimer += 1.0f * Time.deltaTime;
        }
        if (StaminaTimer > 10.0f)
        {
            StaminaTimerOn = false;
        }
    }

    void LOG()
    {
        print(StepHit.transform.tag);
        print(StepHit.transform.name);
        print(StepHit.distance);
        print(StepTag);
    }

	void FixedUpdate()
	{
		PlayerMovementVelocity = controller.velocity.magnitude;
	}

	private void PlayerMovement()
	{
		float inputX = PlayerInputManager.instance.InputX;
		float inputY = PlayerInputManager.instance.InputY;
		float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed)? .7071f : 1.0f;

		PlayerThirdPersonController.PlayerThirdPersonAnimator.SetBool ("Grounded", grounded);
		PlayerLegsAnimator.SetBool ("Grounded", grounded);

		if (grounded) {
			PlayerThirdPersonController.PlayerThirdPersonAnimator.SetFloat ("Vertical", inputY, 0.1f, Time.deltaTime); 
			PlayerThirdPersonController.PlayerThirdPersonAnimator.SetFloat ("Horizontal", inputX, 0.1f, Time.deltaTime); 
			PlayerLegsAnimator.SetFloat ("Vertical", inputY, 0.1f, Time.deltaTime); 
			PlayerLegsAnimator.SetFloat ("Horizontal", inputX, 0.1f, Time.deltaTime); 
			gravity = NormalGravity; 

			bool sliding = false;
			if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, rayDistance)) {
				if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
					sliding = true;
			}
			else {
				Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit);
				if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
					sliding = true;
			}

			if (falling) {
				falling = false;
				PlayerLegsAnimator.SetBool ("Jump", false);
				PlayerThirdPersonController.PlayerThirdPersonAnimator.SetBool ("Jump", false);
				if (controller.height == PlayerNormalHeight || controller.height == PlayerCrouchedHeight) {	
					if (myTransform.position.y < fallStartLevel - 0.3f && !InWater) {
						PlayerStatistics.PlayerGearAudioSource.PlayOneShot (PlayerStatistics.GearPlayerLandSound, PlayerStatistics.GearSoundVolume);	
						PlayerCameraAnimationComponent.CrossFade (PlayerCameraLand.name, 0.1f);		
					}
                }
				if (myTransform.position.y < fallStartLevel - fallingDamageThreshold)
					FallingDamageAlert (fallStartLevel - myTransform.position.y);
			}
			if (WasStanding && !grounded)
			{
				WasStanding = false;
				PlayerThirdPersonController.PlayerThirdPersonAnimator.SetBool ("Jump", true);
			}
			else if (!WasStanding && grounded)
			{
				WasStanding = true;
				PlayerThirdPersonController.PlayerThirdPersonAnimator.SetBool ("Jump", false);
			} 

			if (PlayerInputManager.instance.Crouch && !PlayerInputManager.instance.Sprint) {
				PlayerStanceState = PlayerStance.Crouching;
				PlayerWeaponHolder.localPosition = Vector3.Lerp(PlayerWeaponHolder.localPosition, PlayerCameraOffset, Time.deltaTime * 10);
				controller.height = PlayerCrouchedHeight;
				controller.center = PlayerCenterOffset;
				PlayerThirdPersonController.PlayerThirdPersonAnimator.SetBool ("Crouched", true);
				PlayerLegsAnimator.SetBool ("Crouched", true);
			}
            else
            {
				PlayerStanceState = PlayerStance.Standing;
				PlayerWeaponHolder.localPosition = Vector3.Lerp(PlayerWeaponHolder.localPosition, Vector3.zero, Time.deltaTime * 10);
				controller.height = PlayerNormalHeight;
				controller.center = Vector3.zero;
				PlayerThirdPersonController.PlayerThirdPersonAnimator.SetBool ("Crouched", false);
				PlayerLegsAnimator.SetBool ("Crouched", false);
			}

			if (inputX != 0 && PlayerMovementVelocity > 0.1f || inputY != 0 && PlayerMovementVelocity > 0.1f)
            {
				if (PlayerInputManager.instance.Sprint && PlayerStatistics.PlayerStamina > 0.0f && !StaminaTimerOn)
                {
                    if (PlayerStatistics.PlayerStamina < 1.0f)
                    {
                        StaminaTimerOn = true;
                    }
                    PlayerStatistics.PlayerStamina -= 5 * Time.deltaTime;
                    InGameUI.instance.Stamina.value = PlayerStatistics.PlayerStamina;
                    PlayerWalkingState = WalkingState.Running;
					PlayerMovementSpeed = runSpeed;
					PlayerThirdPersonController.PlayerThirdPersonAnimator.SetBool ("Sprinting", true);
					PlayerLegsAnimator.SetBool ("Sprinting", true);
					PlayerStatistics.RunMultiplier = 1.3f;
					if (!PlayerCameraAnimationComponent.isPlaying)
                    {
						PlayerCameraAnimationComponent.Play (PlayerCameraRun.name);
					}
				}
                else
                {
                    PlayerWalkingState = WalkingState.Walking;
					if (PlayerStanceState == PlayerStance.Crouching)
                    {
						PlayerMovementSpeed = PlayerCrouchSpeed;
					}
                    else
                    {
						PlayerMovementSpeed = walkSpeed;
					}
					PlayerStatistics.RunMultiplier = 1f;
					PlayerThirdPersonController.PlayerThirdPersonAnimator.SetBool ("Sprinting", false); 
					PlayerLegsAnimator.SetBool ("Sprinting", false);
				}
			}
            else
            {
				PlayerWalkingState = WalkingState.Idle;
				PlayerStatistics.RunMultiplier = 1f;
				PlayerThirdPersonController.PlayerThirdPersonAnimator.SetBool ("Sprinting", false);
				PlayerLegsAnimator.SetBool ("Sprinting", false);
			}

            if (!PlayerInputManager.instance.Sprint)
            {
                if (PlayerStatistics.PlayerStamina < 100.0f)
                {
                    if (StaminaTimerOn)
                    {
                        if (StaminaTimer > 5.0f)
                        {
                            PlayerStatistics.PlayerStamina += 5 * Time.deltaTime;
                        }
                    }
                    else
                    {
                        PlayerStatistics.PlayerStamina += 5 * Time.deltaTime;
                    }
                    InGameUI.instance.Stamina.value = PlayerStatistics.PlayerStamina;
                }
                else if (PlayerStatistics.PlayerStamina > 100.0f)
                {
                    PlayerStatistics.PlayerStamina = 100.0f;
                    InGameUI.instance.Stamina.value = 100.0f;
                }
            }

			if ( (sliding && slideWhenOverSlopeLimit) || (slideOnTaggedObjects && hit.collider.tag == "Slide") ) {
				Vector3 hitNormal = hit.normal;
				moveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
				Vector3.OrthoNormalize (ref hitNormal, ref moveDirection);
				moveDirection *= slideSpeed;
				playerControl = false;
			}
			else
            {
				moveDirection = new Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);
				moveDirection = myTransform.TransformDirection(moveDirection) * PlayerMovementSpeed;
				playerControl = true;
			}

			if (grounded && controller.velocity.magnitude > 0.1 && PlayerStatistics.StepTimer <= Time.time && !GameManager.instance.InVehicle && !InWater && !InWood && !InMetal && !InSand &&!InStone) {
				PlayerStatistics.StepTimer = Time.time + (PlayerStatistics.StepInterval / PlayerMovementSpeed * PlayerStatistics.RunMultiplier); 
				PlayerStatistics.FootstepAudiosource.PlayOneShot (PlayerStatistics.FootstepSounds [Random.Range (0, PlayerStatistics.FootstepSounds.Length)], PlayerStatistics.FootStepVolume); 
				PlayerPhotonView.RPC("PlayFootstepSoundNetwork", PhotonTargets.Others, "Normal");
			}
            else if (grounded && controller.velocity.magnitude > 0.1 && PlayerStatistics.StepTimer <= Time.time && !GameManager.instance.InVehicle && InWater) {
                PlayerStatistics.StepTimer = Time.time + (PlayerStatistics.WaterStepInterval / PlayerMovementSpeed * PlayerStatistics.RunMultiplier); 
                PlayerStatistics.FootstepAudiosource.PlayOneShot(PlayerStatistics.WaterstepSounds[Random.Range(0, PlayerStatistics.WaterstepSounds.Length)], PlayerStatistics.WaterStepVolume); 
                PlayerPhotonView.RPC("PlayFootstepSoundNetwork", PhotonTargets.Others, "Water");
			}
            else if (grounded && controller.velocity.magnitude > 0.1 && PlayerStatistics.StepTimer <= Time.time && !GameManager.instance.InVehicle && InWood)
            { 
                PlayerStatistics.StepTimer = Time.time + (PlayerStatistics.WoodStepInterval / PlayerMovementSpeed * PlayerStatistics.RunMultiplier);
                PlayerStatistics.FootstepAudiosource.PlayOneShot(PlayerStatistics.WoodstepSounds[Random.Range(0, PlayerStatistics.WoodstepSounds.Length)], PlayerStatistics.WoodStepVolume); 
                PlayerPhotonView.RPC("PlayFootstepSoundNetwork", PhotonTargets.Others, "Wood");
            }
            else if (grounded && controller.velocity.magnitude > 0.1 && PlayerStatistics.StepTimer <= Time.time && !GameManager.instance.InVehicle && InWood)
            { 
                PlayerStatistics.StepTimer = Time.time + (PlayerStatistics.WoodStepInterval / PlayerMovementSpeed * PlayerStatistics.RunMultiplier);
                PlayerStatistics.FootstepAudiosource.PlayOneShot(PlayerStatistics.WoodstepSounds[Random.Range(0, PlayerStatistics.WoodstepSounds.Length)], PlayerStatistics.WoodStepVolume);
                PlayerPhotonView.RPC("PlayFootstepSoundNetwork", PhotonTargets.Others, "Wood2");
            }
            else if (grounded && controller.velocity.magnitude > 0.1 && PlayerStatistics.StepTimer <= Time.time && !GameManager.instance.InVehicle && InSand)
            {
                PlayerStatistics.StepTimer = Time.time + (PlayerStatistics.SandStepInterval / PlayerMovementSpeed * PlayerStatistics.RunMultiplier);
                PlayerStatistics.FootstepAudiosource.PlayOneShot(PlayerStatistics.SandstepSounds[Random.Range(0, PlayerStatistics.SandstepSounds.Length)], PlayerStatistics.SandStepVolume);
                PlayerPhotonView.RPC("PlayFootstepSoundNetwork", PhotonTargets.Others, "Sand");
            }
            else if (grounded && controller.velocity.magnitude > 0.1 && PlayerStatistics.StepTimer <= Time.time && !GameManager.instance.InVehicle && InMetal)
            {
                PlayerStatistics.StepTimer = Time.time + (PlayerStatistics.MetalStepInterval / PlayerMovementSpeed * PlayerStatistics.RunMultiplier); 
                PlayerStatistics.FootstepAudiosource.PlayOneShot(PlayerStatistics.MetalstepSounds[Random.Range(0, PlayerStatistics.MetalstepSounds.Length)], PlayerStatistics.MetalStepVolume);
                PlayerPhotonView.RPC("PlayFootstepSoundNetwork", PhotonTargets.Others, "Metal");
            }
            else if (grounded && controller.velocity.magnitude > 0.1 && PlayerStatistics.StepTimer <= Time.time && !GameManager.instance.InVehicle && InStone)
            { 
                PlayerStatistics.StepTimer = Time.time + (PlayerStatistics.StoneStepInterval / PlayerMovementSpeed * PlayerStatistics.RunMultiplier); 
                PlayerStatistics.FootstepAudiosource.PlayOneShot(PlayerStatistics.StonestepSounds[Random.Range(0, PlayerStatistics.StonestepSounds.Length)], PlayerStatistics.StoneStepVolume); 
                PlayerPhotonView.RPC("PlayFootstepSoundNetwork", PhotonTargets.Others, "Stone");
            }
            else if (grounded && controller.velocity.magnitude > 0.1 && PlayerStatistics.StepTimer <= Time.time && !GameManager.instance.InVehicle && InStone)
            { 
                PlayerStatistics.StepTimer = Time.time + (PlayerStatistics.StoneStepInterval / PlayerMovementSpeed * PlayerStatistics.RunMultiplier); 
                PlayerStatistics.FootstepAudiosource.PlayOneShot(PlayerStatistics.StonestepSounds[Random.Range(0, PlayerStatistics.StonestepSounds.Length)], PlayerStatistics.StoneStepVolume);
                PlayerPhotonView.RPC("PlayFootstepSoundNetwork", PhotonTargets.Others, "Stone2");
            }

            if (grounded && controller.velocity.magnitude > 0 && PlayerWalkingState == WalkingState.Running && PlayerStatistics.SprintBreathTimer <= Time.time && !GameManager.instance.InVehicle)
            {
				PlayerStatistics.SprintBreathTimer = Time.time + PlayerStatistics.SprintBreathInterval; 
				PlayerStatistics.PlayerGearAudioSource.PlayOneShot (PlayerStatistics.PlayerSprintBreathSounds [Random.Range (0, PlayerStatistics.PlayerSprintBreathSounds.Length)], PlayerStatistics.BreathSoundVolume);
			}

			if (!PlayerInputManager.instance.Jump)
				jumpTimer++;
			else if (jumpTimer >= antiBunnyHopFactor) {
				moveDirection.y = jumpSpeed;
				jumpTimer = 0;
			}

		}
		else if(!grounded)
		{
			if (PlayerWalkingState == WalkingState.Running || PlayerWalkingState == WalkingState.Walking)
            {
				PlayerWalkingState = WalkingState.Idle;
			}

			if (!falling && PlayerStanceState != PlayerStance.Crouching) {
				falling = true;
				fallStartLevel = myTransform.position.y;
				PlayerThirdPersonController.PlayerThirdPersonAnimator.SetBool ("Jump", true);
				PlayerLegsAnimator.SetBool ("Jump", true);
			}

			if (airControl && playerControl) {
				moveDirection.x = inputX * PlayerMovementSpeed * inputModifyFactor;
				moveDirection.z = inputY * PlayerMovementSpeed * inputModifyFactor;
				moveDirection = myTransform.TransformDirection(moveDirection);
			}
		}
		moveDirection.y -= gravity * Time.deltaTime;

		if (GameManager.instance.IsAlive && !GameManager.instance.InVehicle) {
			grounded = (controller.Move (moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
		} else {
			grounded = false;
		}
	}

	void OnControllerColliderHit (ControllerColliderHit hit) {
		contactPoint = hit.point;
	}

	void PlayerEnterWater()
	{
		InWater = true;
	}

	void PlayerExitWater()
	{
		InWater = false;
	}

    void PlayerEnterWood()
    {
        InWood = true;
    }

    void PlayerExitWood()
    {
        InWood = false;
    }

    void PlayerEnterSand()
    {
        InSand = true;
    }

    void PlayerExitSand()
    {
        InSand = false;
    }

    void PlayerEnterMetal()
    {
        InMetal = true;
    }

    void PlayerExitMetal()
    {
        InMetal = false;
    }

    void PlayerEnterStone()
    {
        InStone = true;
    }

    void PlayerExitStone()
    {
        InStone = false;
    }

    void FallingDamageAlert (float fallDistance)
    {
		if (fallDistance > 2.5f)
        {
			int dmg = Mathf.RoundToInt (fallDistance * 7f);
            PlayerPhotonView.RPC("ApplyPlayerDamage", PhotonTargets.All , dmg, "Falling", PhotonNetwork.player, 1f, false);
		}
	}

	private void PlayerLeaning()
	{
		if (PlayerInputManager.instance.LeanLeft && !Input.GetKey(KeyCode.LeftShift))
        {
			isLeaning = true;
			CurrentLeanAngle = Mathf.MoveTowardsAngle(CurrentLeanAngle, MaxLeanAngle, LeanSpeed * Time.deltaTime);
		}
		else if (PlayerInputManager.instance.LeanRight && !Input.GetKey(KeyCode.LeftShift))
        {
			isLeaning = true;
			CurrentLeanAngle = Mathf.MoveTowardsAngle(CurrentLeanAngle, -MaxLeanAngle, LeanSpeed * Time.deltaTime);
		}
		else
        {
			isLeaning = false;
			CurrentLeanAngle = Mathf.MoveTowardsAngle(CurrentLeanAngle, 0f, LeanSpeed * Time.deltaTime);
		}
		LeaningPivotTransform.localRotation = Quaternion.AngleAxis(CurrentLeanAngle, Vector3.forward);
	}

	private void RotateView()
	{
		PlayerMouseLook.LookRotation (transform, PlayerWeaponHolder.transform);
	}
}

public enum WalkingState
{
	Idle,
	Walking,
	Running
}

public enum PlayerStance
{
	Standing,
	Crouching
}
	