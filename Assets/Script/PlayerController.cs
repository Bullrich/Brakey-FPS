using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[SelectionBaseAttribute]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 7f)]
    private float
    speed = 5f,
    lookSensitivity = 3f;
    [SerializeField]
    private float thrusterForce = 1000f;

    [HeaderAttribute("Joint Options")]
    [SerializeField]
    private float
    jointSpring = 20f,
    jointMaxForce = 40f;

    // Component caching
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();
        SetJointSettings(jointSpring);
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {

        // Calculate movement velocity as a 3D vector
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _moveHorizontal = transform.right * _xMov;
        Vector3 _moveVertical = transform.forward * _zMov;

        // Final movement vector
        Vector3 _velocity = (_moveHorizontal + _moveVertical) * speed;

        // Animate movement
        animator.SetFloat("ForwardVelocity", _zMov);

        // Apply movement
        motor.Move(_velocity);

        // Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        // Apply rotation
        motor.Rotate(_rotation);

        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * lookSensitivity;

        // Apply camera rotation
        motor.RotateCamera(_cameraRotationX);

        //Calculate thruster force
        Vector3 _thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        }else{
            SetJointSettings(jointSpring);
        }

        // Apply the thruster force
        motor.ApplyThruster(_thrusterForce);
    }

    private void SetJointSettings(float _jointSpring){
        joint.yDrive = new JointDrive{
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }
}
