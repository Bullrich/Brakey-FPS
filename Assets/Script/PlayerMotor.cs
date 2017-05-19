using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by @Bullrich

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Rigidbody rb;

    private Vector3
        velocity = Vector3.zero,
        rotation = Vector3.zero,
        thrusterForce = Vector3.zero;

    private float
        cameraRotationX = 0f,
        currentCameraRotationX = 0f;

    [SerializeField]
    private float cameraRotationLimit = 85;

    public bool invertedY;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        PerformMovement(velocity);
        PerformRotation(rotation, cameraRotationX);
    }

    private void PerformMovement(Vector3 _velocity)
    {
        if (_velocity != Vector3.zero)
            rb.MovePosition(rb.position + _velocity * Time.fixedDeltaTime);

        if (thrusterForce != Vector3.zero)
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
    }

    private void PerformRotation(Vector3 _rotation, float _cameraRotX)
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(_rotation));
        if (cam != null)
        {
            currentCameraRotationX -= _cameraRotX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }




    #region public methods
    public void RotateCamera(float _cmRotationX)
    {
        cameraRotationX = _cmRotationX * (invertedY ? -1 : 1);
    }
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    ///<summary>Get a movement vector</summary>
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    ///<summary>Get a force vector for the thrusters</summary>
    public void ApplyThruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }
    #endregion
}
