using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public WheelCollider frontDriverW, frontPassengerW;
    public WheelCollider rearDriverW, rearPassengerW;
    public Transform frontDriverT, frontPassengerT;
    public Transform rearDriverT, rearPassengerT;
    public float maxSteerAngle = 30;
    public float motorForce = 50;

    public bool inTrigger = false;
    public Transform Driver;
    public Transform Passenger;
    public Transform DriverExit;
    public Transform PassengerExit;
    public GameObject[] inCar;
    PhotonView PV;


    private void Start()
    {
        PV = gameObject.GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (inTrigger)
        {
            if (inCar[0] == null)
            {

            }
        }
    }

    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
    }

    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontDriverW.steerAngle = m_steeringAngle;
        frontPassengerW.steerAngle = m_steeringAngle;
    }

    private void Accelerate()
    {
        frontDriverW.motorTorque = m_verticalInput * motorForce;
        frontPassengerW.motorTorque = m_verticalInput * motorForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassengerW, frontPassengerT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(rearPassengerW, rearPassengerT);
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void FixedUpdate()
    {
        if (inCar[0] != null)
        {
            GetInput();
            Steer();
            Accelerate();
            UpdateWheelPoses();
        }
    }

    [PunRPC]
    public void EnterDriver()
    {
        inCar[0].transform.position = Driver.transform.position;
        inCar[0].transform.rotation = Driver.transform.rotation;
        inCar[0].transform.parent = Driver.transform;
    }

    [PunRPC]
    public void ExitDriver()
    {
        inCar[0].transform.parent = null;
        inCar[0] = null;
    }

    private void OnTriggerStay(Collider other)
    {
        inTrigger = true;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (inCar[0] == null)
            {
                inCar[0] = other.gameObject;
                PV.RPC("EnterDriver", PhotonTargets.AllBuffered);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }
}
