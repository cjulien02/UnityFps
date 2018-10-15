using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float xLookSensitivity = 3f;

    [SerializeField]
    private float yLookSensitivity = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

    Animator animator;

    [Header("Joint Options")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;


    private PlayerMotor motor;
    private ConfigurableJoint joint;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();
        SetJointSetting(jointSpring);
    }

    private void Update()
    {

        float _xMove = Input.GetAxisRaw("Horizontal");
        float _zMove = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _xMove;
        Vector3 _moveVertical = transform.forward * _zMove;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * speed;

        animator.SetFloat("ForwardVelocity", _zMove);

        motor.Move(_velocity);

        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3  _rotation = new Vector3(0, _yRot, 0) * xLookSensitivity;

        motor.Rotate(_rotation);

        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * yLookSensitivity;

        motor.RotateCamera(_cameraRotationX);

        Vector3 _thrusterForce = Vector3.zero;

        if(Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;

            SetJointSetting(0f);
        }
        else
        {
            SetJointSetting(jointSpring);
        }
        motor.ApplyThruster(_thrusterForce);
    }

    private void SetJointSetting(float jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            maximumForce = jointMaxForce,
            positionSpring = jointSpring
        };
    }
}
