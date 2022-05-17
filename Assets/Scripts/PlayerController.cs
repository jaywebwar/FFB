using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidBody;

    [SerializeField] float accelerationForce;
    [SerializeField] float topSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && _rigidBody.velocity.magnitude < topSpeed)
        {
            _rigidBody.AddForce(Vector3.forward * accelerationForce);
        }
        if(_rigidBody.velocity.magnitude > topSpeed)
        {
            _rigidBody.velocity = Vector3.forward * topSpeed;
        }
    }
}
