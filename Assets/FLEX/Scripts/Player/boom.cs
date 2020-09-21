﻿using UnityEngine;
[RequireComponent(typeof(PhotonView))]

public class boom : MonoBehaviour
{
    [SerializeField] Transform deton;
    [Range(0, 15)] public float time = 5f;
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip Collision;
    int counter = 0;

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Instantiate(deton, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    [PunRPC]
    public void AddForce(int Force)
    {
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (counter > 0)
        {
            _audio.PlayOneShot(Collision);
        }
        counter++;
    }
}
