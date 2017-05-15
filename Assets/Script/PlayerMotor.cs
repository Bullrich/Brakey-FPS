using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by @Bullrich

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	private Rigidbody rb;
	private Vector3 velocity = Vector3.zero;

	private void Start(){
		rb = GetComponent<Rigidbody>();
	}

	///<summary>Get a movement vector</summary>
	public void Move(Vector3 _velocity){
		velocity = _velocity;
	}

	void FixedUpdate(){
		PerformMovement(velocity);
	}

	private void PerformMovement(Vector3 _velocity){
		if (_velocity != Vector3.zero){
			rb.MovePosition(rb.position + _velocity * Time.fixedDeltaTime);
		}
	}
}
