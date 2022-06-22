using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidBody;

    [SerializeField] CinemachineVirtualCamera _playerFollowCamera;
    [SerializeField] GameObject gameBall;

    [SerializeField] float accelerationForce;
    [SerializeField] float topDirectionalSpeed;
    [SerializeField] float maxRotationalSpeed;
    [SerializeField] float accelerationTorque;
    [SerializeField] float torqueEndSmoothness;
    

    float adjustPitch;
    float adjustRoll;
    float adjustYaw;
    bool isCameraViewBallFocused;


    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AccelerateFromInput();

        AdjustPitchYawRollFromInput();
    }

    void AccelerateFromInput()
    {
        //Accelerate using physics when prompted by input.
        //Contain to a maximum directional speed.
        if ((Vector3.Dot(_rigidBody.velocity, transform.forward) < topDirectionalSpeed) && Input.GetAxis("Accelerate") > 0.0f)
        {
            _rigidBody.AddForce(transform.forward * accelerationForce, ForceMode.Acceleration);
            Debug.Log("Reached top directional velocity.");
        }
        else if (Input.GetButton("Accelerate"))
        {
            Vector3 currentDirectionalSpeed = Vector3.Dot(_rigidBody.velocity, transform.forward) * transform.forward;
            Vector3 currentNonDirectionalSpeed = _rigidBody.velocity - currentDirectionalSpeed;
            _rigidBody.velocity = currentNonDirectionalSpeed + transform.forward * topDirectionalSpeed;
            Debug.Log("Accelerate.");
        }
        else
        {
            Debug.Log("No longer accelerating.");
        }
    }

    void AdjustPitchYawRollFromInput()
    {
        //Adjust Pitch, Yaw, Roll using physics
        if (adjustYaw == 0.0f && adjustPitch == 0.0f && adjustRoll == 0.0f)
        {
            _rigidBody.angularVelocity = Vector3.Lerp(_rigidBody.angularVelocity, Vector3.zero, torqueEndSmoothness);
        }
        else if (_rigidBody.angularVelocity.magnitude < maxRotationalSpeed)
        {
            Vector3 adjustVector = new Vector3(adjustPitch, adjustYaw, adjustRoll);


            Debug.DrawRay(transform.position, transform.forward * 10, Color.yellow);
            Debug.DrawRay(transform.position, transform.right * 10, Color.green);
            Debug.DrawRay(transform.position, transform.up * 10, Color.red);


            _rigidBody.AddTorque((transform.right * adjustPitch) * accelerationTorque, ForceMode.Acceleration);
            _rigidBody.AddTorque((transform.up * adjustYaw) * accelerationTorque, ForceMode.Acceleration);
            _rigidBody.AddTorque((transform.forward * -adjustRoll) * accelerationTorque, ForceMode.Acceleration);
        }
        else
        {
            _rigidBody.angularVelocity = _rigidBody.angularVelocity.normalized * maxRotationalSpeed;
        }
    }

    void Update()
    {
        //Input for adjusting pitch, yaw, and roll.
        GetPitchYawRollFromInput();
    }

    void LateUpdate()
    {
        //Input for switching camera focus
        SwitchCamera();
    }

    void SwitchCamera()
    {
        if (Input.GetButtonDown("SwitchCamera"))
        {
            if (isCameraViewBallFocused)
            {
                //TODO
                //switch camera to out in front
                _playerFollowCamera.gameObject.SetActive(true);
                Debug.Log("Camera not focused on ball");
                isCameraViewBallFocused = false;
            }
            else
            {
                //TODO
                //switch camera to focus ball
                _playerFollowCamera.gameObject.SetActive(false);
                Debug.Log("Camera focused on ball");
                isCameraViewBallFocused = true;
            }
        }
    }

    void GetPitchYawRollFromInput()
    {
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
    }
}
