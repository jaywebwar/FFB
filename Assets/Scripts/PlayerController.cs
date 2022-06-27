using Cinemachine;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rigidBody;

    GameObject _engineFX;

    [Header("Referenced Game Objects")]
    [SerializeField] CinemachineVirtualCamera _playerFollowCamera;

    [Header("Vehicle Controls")]
    [SerializeField] float accelerationForce;
    [SerializeField] float topDirectionalSpeed;
    [SerializeField] float maxRotationalSpeed;
    [SerializeField] float accelerationTorque;
    [SerializeField] float torqueEndSmoothness;


    float adjustVehiclePitch;
    float adjustVehicleRoll;
    float adjustVehicleYaw;
    bool isCameraViewBallFocused;
    bool isAccelerating;
    float threshold = 0.01f;
    float cameraYaw = 0f;
    float cameraPitch = 0f;

    [Header("Camera Controls")]
    [SerializeField] GameObject CinemachineCameraTarget;
    [SerializeField] float YawBottomClamp = -90f;
    [SerializeField] float YawTopClamp = 90f;
    [SerializeField] float PitchBottomClamp = -90f;
    [SerializeField] float PitchTopClamp = 90f;


    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        foreach (Transform t in transform)
        {
            if (t.gameObject.name == "EngineFX")
            {
                _engineFX = t.gameObject;
            }
        }
        _engineFX.SetActive(false);
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
        if ((Vector3.Dot(_rigidBody.velocity, transform.forward) < topDirectionalSpeed) && isAccelerating)
        {
            _rigidBody.AddForce(transform.forward * accelerationForce, ForceMode.Acceleration);
        }
        else if (Input.GetButton("Accelerate"))
        {
            Vector3 currentDirectionalSpeed = Vector3.Dot(_rigidBody.velocity, transform.forward) * transform.forward;
            Vector3 currentNonDirectionalSpeed = _rigidBody.velocity - currentDirectionalSpeed;
            _rigidBody.velocity = currentNonDirectionalSpeed + transform.forward * topDirectionalSpeed;
        }
    }

    void AdjustPitchYawRollFromInput()
    {
        //Adjust Pitch, Yaw, Roll using physics
        if (adjustVehicleYaw == 0f && adjustVehiclePitch == 0f && adjustVehicleRoll == 0f)
        {
            _rigidBody.angularVelocity = Vector3.Lerp(_rigidBody.angularVelocity, Vector3.zero, torqueEndSmoothness);
        }
        else if (_rigidBody.angularVelocity.magnitude < maxRotationalSpeed)
        {
            _rigidBody.AddTorque((transform.right * adjustVehiclePitch) * accelerationTorque, ForceMode.Acceleration);
            _rigidBody.AddTorque((transform.up * adjustVehicleYaw) * accelerationTorque, ForceMode.Acceleration);
            _rigidBody.AddTorque((transform.forward * -adjustVehicleRoll) * accelerationTorque, ForceMode.Acceleration);
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

        //Get Acceleration input
        GetAccelerationInput();

        //Handle Engine Visual FX
        HandleEngineVisualFX();
    }



    void HandleEngineVisualFX()
    {
        if (isAccelerating)
        {
            _engineFX.SetActive(true);
        }
        else
        {
            _engineFX.SetActive(false);
        }
    }

    void GetAccelerationInput()
    {
        if (Input.GetAxis("Accelerate") > 0f || Input.GetButton("Accelerate"))
        {
            isAccelerating = true;
        }
        else
        {
            isAccelerating = false;
        }
    }

    void GetPitchYawRollFromInput()
    {
        //Input for adjusting pitch
        adjustVehiclePitch = Input.GetAxis("Vertical");

        //Input for adjusting yaw and roll
        if (Input.GetButton("RollButton"))
        {
            adjustVehicleRoll = Input.GetAxis("Horizontal");
            adjustVehicleYaw = 0f;
        }
        else
        {
            adjustVehicleYaw = Input.GetAxis("Horizontal");
            adjustVehicleRoll = 0f;
        }
    }

    void LateUpdate()
    {
        //Input for switching camera focus
        SwitchCamera();

        //Handles Camera rotation from input
        //CameraRotation();
        TrackCameraBehindShip();
    }

    void TrackCameraBehindShip()
    {
        CinemachineCameraTarget.transform.rotation = _rigidBody.transform.rotation;
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
                isCameraViewBallFocused = false;
            }
            else
            {
                //TODO
                //switch camera to focus ball
                _playerFollowCamera.gameObject.SetActive(false);
                isCameraViewBallFocused = true;
            }
        }
    }

    void CameraRotation()
    {
        MouseCameraRotation();
        //JoystickCameraRotation();

        // clamp our rotations so our values are limited 360 degrees
        cameraYaw = ClampAngle(cameraYaw, YawBottomClamp, YawTopClamp);
        cameraPitch = ClampAngle(cameraPitch, PitchBottomClamp, PitchTopClamp);

        Debug.Log("CameraTarget Rotation: X = "+ CinemachineCameraTarget.transform.rotation.x+" Y = "+ CinemachineCameraTarget.transform.rotation.y);
        Debug.Log("Player Object Rotation: X = "+ transform.rotation.x+" Y = "+ transform.rotation.y);
        Debug.Log("Rigidbody Rotation: X = "+ _rigidBody.transform.rotation.x+" Y = "+ _rigidBody.transform.rotation.y);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
    }

    void MouseCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        if (mouseX >= threshold || mouseX <= -threshold)
        {
            cameraYaw += mouseX;
        }
        if (mouseY >= threshold || mouseY <= -threshold)
        {
            cameraPitch = mouseY;
        }
    }

    void JoystickCameraRotation()
    {
        float joystickX = Input.GetAxis("Right Joystick X");
        float joystickY = Input.GetAxis("Right Joystick Y");

        if (joystickX >= threshold || joystickX <= -threshold)
        {
            cameraYaw += joystickX;
        }

        if (joystickY >= threshold || joystickY <= -threshold)
        {
            cameraPitch += joystickY;
        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        //prevents getting stuck on a max magnitude float
        if (angle < -360f)
        {
            angle += 360f;
        }
        if (angle > 360f)
        {
            angle -= 360f;
        }

        return Mathf.Clamp(angle, min, max);
    }
}
