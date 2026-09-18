using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    private static readonly int IsSprintingHash = Animator.StringToHash("IsSprinting");
    private static readonly int YHash = Animator.StringToHash("Y");
    private static readonly int XHash = Animator.StringToHash("X");



    [SerializeField] private PlayerInputReader inputReader;
    
    [SerializeField] private Transform cameraTransform;
    
    [SerializeField] private Animator animator;

    [SerializeField] private float walkSpeed = 2.2f;
    
    [SerializeField] private float runSpeed = 5.5f;
    [SerializeField] private float acceleration = 14f;

    [SerializeField] private float rotationSmoothTime = 0.06f;
    
    [SerializeField] private bool rotateWhileIdle = true;

    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundedStickForce = -2f;

    private CharacterController controller;

    private float verticalVelocity;
    private float turnSmoothVelocity;
    private Vector3 planarVelocity;

    public Vector2 StrafeBlend { get; private set; }
    
    public float CurrentSpeed => planarVelocity.magnitude;
    public float NormalizedSpeed => runSpeed > 0f ? CurrentSpeed / runSpeed : 0f;
    public bool IsMoving => CurrentSpeed > 0.01f;
    public bool IsRunning => inputReader != null && inputReader.IsRunning && IsMoving;
    public bool IsGrounded => controller.isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (inputReader == null)
            inputReader = GetComponent<PlayerInputReader>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        Vector2 input = inputReader != null
            ? inputReader.PlayerMoveValue
            : Vector2.zero;

        ApplyGravity();
        HandleRotation(input);
        HandleMovement(input);
        UpdateStrafeBlend();
    }

    private void HandleRotation(Vector2 input)
    {
        if (cameraTransform == null)
            return;

        if (!rotateWhileIdle && input.sqrMagnitude < 0.01f)
            return;

        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;

        if (cameraForward.sqrMagnitude < 0.0001f)
            return;

        float targetYaw = Quaternion.LookRotation(cameraForward).eulerAngles.y;

        float smoothedYaw = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            targetYaw,
            ref turnSmoothVelocity,
            rotationSmoothTime
        );

        transform.rotation = Quaternion.Euler(0f, smoothedYaw, 0f);
    }

    private void HandleMovement(Vector2 input)
    {
        float inputMagnitude = Mathf.Clamp01(input.magnitude);

        if (animator != null)
        {
            animator.SetFloat(XHash, input.x, 0.25f, Time.deltaTime);
            animator.SetFloat(YHash, input.y, 0.25f, Time.deltaTime);
            animator.SetBool(
                IsSprintingHash,
                inputReader != null && inputReader.IsRunning
            );
        }

        Vector3 targetVelocity = Vector3.zero;

        if (inputMagnitude > 0.01f)
        {
            float speed = inputReader != null && inputReader.IsRunning
                ? runSpeed
                : walkSpeed;

            speed *= inputMagnitude;

            targetVelocity = GetCameraRelativeDirection(input) * speed;
        }

        planarVelocity = Vector3.MoveTowards(
            planarVelocity,
            targetVelocity,
            acceleration * Time.deltaTime
        );

        Vector3 velocity = planarVelocity;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    private Vector3 GetCameraRelativeDirection(Vector2 input)
    {
        if (cameraTransform == null)
            return new Vector3(input.x, 0f, input.y).normalized;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return (forward * input.y + right * input.x).normalized;
    }

    private void UpdateStrafeBlend()
    {
        Vector3 local = transform.InverseTransformDirection(planarVelocity);

        float divisor = Mathf.Max(runSpeed, 0.0001f);

        StrafeBlend = new Vector2(
            local.x / divisor,
            local.z / divisor
        );
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundedStickForce;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }
}