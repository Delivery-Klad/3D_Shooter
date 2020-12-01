using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PhotonView))]
public class Door : MonoBehaviour
{
    private Animator _animator = null;
    public bool on = false;
    public bool Open = false;
    [SerializeField] AudioSource AU;
    AudioClip Open_Clip;
    AudioClip Close_Clip;


    void Start()
    {
        AU = gameObject.GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
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
        }
        if (Open == false)
        {
            _animator.SetBool("isOpen", false);
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