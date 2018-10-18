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

    [SerializeField]
    private float thrusterBurnSpeed = 0.6f;

    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;
    private float thrusterFuelAmount = 1f;

    [SerializeField]
    private LayerMask environementMask;

    public float GetThrusterAmount()
    {
        return thrusterFuelAmount;
    }

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
        if (PauseMenu.isON)
        {
            return;
        }
        RaycastHit _hit;

        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 100f, environementMask))
        {
            joint.targetPosition = new Vector3(0f, _hit.point.y, 0f);
        }
        else
        {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
        }

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

        if(Input.GetButton("Jump") && thrusterFuelAmount > 0f)
        {
            thrusterFuelAmount -= thrusterBurnSpeed * Time.deltaTime;

            if(thrusterFuelAmount > 0.01f)
            {
                _thrusterForce = Vector3.up * thrusterForce;

                SetJointSetting(0f);
            }
        }
        else
        {
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;

            SetJointSetting(jointSpring);
        }
        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

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
