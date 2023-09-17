using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    private bool isActiveLampStopSignals = false;
    private Rigidbody rigidbody;

    [Header("Settings Wheel")]
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    [Header("Settings Lamp")]
    [SerializeField] private List<LampStopSignal> lampStopSignals;
    [SerializeField] private GameObject frontLeftLamp, frontRightLamp;
    [SerializeField] private KeyCode keyLamp;

    [Header("Rigibody")]
    [SerializeField] private Transform centerOfMass;

    public Action<float> GetSpeedCar;

    [SerializeField] private Text speedText;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = centerOfMass.localPosition;
    }

    public void Initialize()
    {
        
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyLamp))
            ChangeActiveFrontLamp();
        if (Input.GetKey(KeyCode.Space) && isActiveLampStopSignals == false)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeActiveStopSignals(lampStopSignals, 1.5f));
            isActiveLampStopSignals = true;
        }

        speedText.text = $"Speed - {Math.Round((rigidbody.velocity.magnitude * 3.6f) / 2, 0)} km/h";
    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);

    }

    private void HandleMotor()
    {
        if (transform.up.y < -0.7f)
            transform.up = Vector3.up;

        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;

        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;

        if (wheelTransform.localEulerAngles.y == 90)
        {
            wheelTransform.Rotate(new Vector3(0, 0, wheelCollider.rpm / 60 * 360 * Time.deltaTime), Space.Self);
        }
        else if (wheelTransform.localEulerAngles.y == 270)
        {
            wheelTransform.Rotate(new Vector3(0, 0, -wheelCollider.rpm / 60 * 360 * Time.deltaTime), Space.Self);
        }
    }

    private void ChangeActiveLamp(GameObject lamp)
    {
        if (lamp.activeInHierarchy)
            lamp.SetActive(false);
        else
            lamp.SetActive(true);
    }

    public void ChangeActiveFrontLamp()
    {
        ChangeActiveLamp(frontLeftLamp);
        ChangeActiveLamp(frontRightLamp);
    }

    private IEnumerator ChangeActiveStopSignals(List<LampStopSignal> lampStopSignals, float time)
    {
        foreach(var stopSignal in lampStopSignals)
        {
            stopSignal.ChangeActiveStopSignalLamp();
        }
        yield return new WaitForSeconds(time);
        isActiveLampStopSignals = false;

        foreach (var stopSignal in lampStopSignals)
        {
            stopSignal.ChangeActiveStopSignalLamp();
        }
    }
}
