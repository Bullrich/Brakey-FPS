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
        cameraRotation = Vector3.zero;

		public bool invertedY;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        PerformMovement(velocity);
        PerformRotation(rotation, cameraRotation);
    }

    private void PerformMovement(Vector3 _velocity)
    {
        if (_velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + _velocity * Time.fixedDeltaTime);
        }
    }

    private void PerformRotation(Vector3 _rotation, Vector3 _cameraRot)
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(_rotation));
		if(cam!= null)
		cam.transform.Rotate(_cameraRot);
    }

    

	
	#region public methods
	public void RotateCamera(Vector3 _cmRotation){
		cameraRotation = _cmRotation * (invertedY ? 1:-1);
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
	#endregion
}
