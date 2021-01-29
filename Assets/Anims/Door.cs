using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PhotonView))]
public class Door : MonoBehaviour
{
    Animator _animator = null;
    public bool on = false;
    public bool Open = false;
    [SerializeField] AudioSource AU = null;
    AudioClip Open_Clip;
    AudioClip Close_Clip;


    void Start()
    {
        AU = gameObject.GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        Open_Clip = GameManager.instance.Open_Clip;
        Close_Clip = GameManager.instance.Close_Clip;
    }

    void Update()
    {
        if (on == true)
        {
            if (Input.GetKeyDown(PlayerInputManager.instance.Use_Button) && _animator.GetBool("isOpen") == false)
            {
                Open = true;
                PhotonView PV = GetComponent<PhotonView>();
                PV.RPC("syncTrue", PhotonTargets.AllBuffered, true);
            }
            else if (Input.GetKeyDown(PlayerInputManager.instance.Use_Button) && _animator.GetBool("isOpen") == true)
            {
                Open = false;
                PhotonView PV = GetComponent<PhotonView>();
                PV.RPC("syncTrue", PhotonTargets.AllBuffered, false);
            }
        }
        if (Open == true)
        {
            _animator.SetBool("isOpen", true);
            if (Open_Clip)
            {
                AU.PlayOneShot(Open_Clip);
            }
        }
        if (Open == false)
        {
            _animator.SetBool("isOpen", false);
            if (Close_Clip)
            {
                AU.PlayOneShot(Close_Clip);
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        on = true;
    }

    void OnTriggerExit(Collider collider)
    {
        on = false;
    }

    [PunRPC]
    void syncTrue(bool isOpen)
    {
        Open = isOpen;
    }
}