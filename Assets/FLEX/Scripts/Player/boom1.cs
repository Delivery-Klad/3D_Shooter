using UnityEngine;
[RequireComponent(typeof(PhotonView))]

public class boom1 : MonoBehaviour
{
    public Transform deton;
    [Range(0, 15)] public float time = 5f;
    public bool enter = false;
    public GameObject[] Players;
    public float[] Distances;

    void Start()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        Distances = new float[Players.Length];
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Instantiate(deton, transform.position, transform.rotation);
            for (int i = 0; i < Players.Length; i++)
            {
                Distances[i] = Vector3.Distance(gameObject.transform.position, Players[i].transform.position);
            }
            ApplyDamage();
            Destroy(gameObject);
        }     
    }

    public void ApplyDamage()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            if (Distances[i] < 30f)
            {
                Players[i].GetComponent<PhotonView>().RPC("ApplyPlayerDamage", PhotonTargets.All, (100 - ((int)Distances[i] * 3)), "Grenade", PhotonNetwork.player, 1.0f, false);
                Debug.Log(Distances[i]);
            }
        }
    }

    [PunRPC]
    public void AddForce(int Force)
    {
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * Force);
    }
}
