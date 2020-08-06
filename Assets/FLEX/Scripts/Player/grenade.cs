using UnityEngine;
[RequireComponent(typeof(PhotonView))]

public class grenade : MonoBehaviour
{
    public Transform Rsp;
    public Transform GranadeSmoke;
    public Transform GranadeHE;
    public int Force = 500;
    [Range(0, 10)] public int GranadeSm = 3;
    [Range(0, 10)] public int GranadeHe = 3;
    private KeyCode ThrowGrenade = KeyCode.G;
    public KeyCode Change;
    public bool Smoke;
    public PhotonView PV;
    private PhotonView Player;

    private void Start()
    {
        Player = gameObject.GetComponent<PhotonView>();
        ThrowGrenade = PlayerInputManager.instance.ThrowButton;
    }

    void Update()
    {
        if (Player.isMine)
        {
            if (!PlayerInputManager.instance.Console)
            {
                if (Input.GetKeyDown(Change))
                {
                    Smoke = !Smoke;
                }
                if (Smoke == true)
                {
                    if (Input.GetKeyDown(ThrowGrenade) & GranadeSm > 0)
                    {
                        GameObject go = PhotonNetwork.Instantiate(GranadeSmoke.name, Rsp.transform.position, gameObject.transform.rotation, 0, null);
                        PV = go.GetComponent<PhotonView>();
                        PV.RPC("AddForce", PhotonTargets.All, Force);
                        GranadeSm -= 1;
                    }
                }
                else
                {
                    if (Input.GetKeyDown(ThrowGrenade) & GranadeHe > 0)
                    {
                        GameObject go = PhotonNetwork.Instantiate(GranadeHE.name, Rsp.transform.position, gameObject.transform.rotation, 0, null);
                        PV = go.GetComponent<PhotonView>();
                        PV.RPC("AddForce", PhotonTargets.All, Force);
                        GranadeHe -= 1;
                    }
                }
            }
        }
    }
}
