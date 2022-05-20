using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidBody;

    [SerializeField] float accelerationForce;
    [SerializeField] float topDirectionalSpeed;
    [SerializeField] float maxRotationalSpeed;
    [SerializeField] float accelerationTorque;
    [SerializeField] float torqueEndSmoothness;

    float adjustPitch;
    float adjustRoll;
    float adjustYaw;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Always active standard thrust using physics
        if (Vector3.Dot(_rigidBody.velocity, transform.forward) < topDirectionalSpeed)
        {
            _rigidBody.AddForce(transform.forward * accelerationForce, ForceMode.Acceleration);
        }
        else
        {
            Vector3 currentDirectionalSpeed = Vector3.Dot(_rigidBody.velocity, transform.forward) * transform.forward;
            Vector3 currentNonDirectionalSpeed = _rigidBody.velocity - currentDirectionalSpeed;
            _rigidBody.velocity = currentNonDirectionalSpeed + transform.forward * topDirectionalSpeed;
        }

        //Input for adjusting pitch
        adjustPitch = Input.GetAxis("Vertical");

        //Input for adjusting yaw and roll
        if (Input.GetButton("RollButton"))
        {
            adjustRoll = Input.GetAxis("Horizontal");
            adjustYaw = 0.0f;
        }
        else
        {
            adjustYaw = Input.GetAxis("Horizontal");
            adjustRoll = 0.0f;
        }

        //Adjust Pitch, Yaw, Roll using physics
        if (adjustYaw == 0.0f && adjustPitch == 0.0f && adjustRoll == 0.0f)
        {
            _rigidBody.angularVelocity = Vector3.Lerp(_rigidBody.angularVelocity, Vector3.zero, torqueEndSmoothness);
        }
        else if(_rigidBody.angularVelocity.magnitude < maxRotationalSpeed)
        {
            Vector3 adjustVector = new Vector3(adjustPitch, adjustYaw, adjustRoll);


            Debug.DrawRay(transform.position, transform.forward*10, Color.yellow);
            Debug.DrawRay(transform.position, transform.right*10, Color.green);
            Debug.DrawRay(transform.position, transform.up*10, Color.red);


            _rigidBody.AddTorque((transform.right * adjustPitch) * accelerationTorque, ForceMode.Acceleration);
            _rigidBody.AddTorque((transform.up * adjustYaw) * accelerationTorque, ForceMode.Acceleration);
            _rigidBody.AddTorque((transform.forward * adjustRoll) * accelerationTorque, ForceMode.Acceleration);
        }
        else
        {
            _rigidBody.angularVelocity = _rigidBody.angularVelocity.normalized * maxRotationalSpeed;
        }
    }
}
