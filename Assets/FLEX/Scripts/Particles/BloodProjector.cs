using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodProjector : MonoBehaviour
{
    private RaycastHit RayHit;
    public LayerMask BloodLayer;
    public GameObject BloodOnWall;

    void Start()
    {
        if (Physics.Raycast(gameObject.transform.position, -gameObject.transform.forward, out RayHit, 3f, BloodLayer))
        {
            if (RayHit.transform.tag == "Wood" || RayHit.transform.tag == "Untagged" || RayHit.transform.tag == "Sand" || RayHit.transform.tag == "Metal")
            {
                Debug.Log("Print");
                GameObject BloodStrafe = Instantiate(BloodOnWall, RayHit.point - new Ray(gameObject.transform.position, -gameObject.transform.forward).direction * 0.001f, Quaternion.FromToRotation(Vector3.forward, RayHit.normal));
                BloodStrafe.transform.parent = RayHit.transform;
            }
        }
    }
    
    void Update()
    {
        
    }
}
