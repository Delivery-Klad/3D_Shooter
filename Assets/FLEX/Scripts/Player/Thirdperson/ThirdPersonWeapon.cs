using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonWeapon : MonoBehaviour
{
    [Header("Настройки оружия")]
    public Transform LeftHandTransform;
    public Transform RightHandTransform;
    public Transform Weaponbarrel;
    public LayerMask WeaponLayerMask;
    public bool IgnoreLeftHandRotation = true;
    public ParticleSystem MuzzleFlash;
    public AudioSource ThirdPersonAudioSource;
    public AudioClip[] WeaponFireSound;
    public AudioClip[] WeaponReloadSound;
    public AudioClip[] WeaponEmptySound;
    public AudioClip[] WeaponSwitchSound;
    public AudioClip WeaponFireLoopSound;
    public float WeaponRange = 1400f;
    public float WeaponImpactForce = 25f;
    public float baseInaccuracyhip = 1.2f;
    public int WeaponHoldType = 0;
    public float triggerTime = 3.6f;
    [Header("Следы пуль")]
    public GameObject ConcreteImpact;
    public GameObject WoodImpact;
    public GameObject MetalImpact;
    public GameObject SandImpact;
    public GameObject WaterImpact;
    public GameObject GlassImpact;
    [Header("Модули")]
    public GameObject flashlight;
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
    public GameObject Clip;
}
