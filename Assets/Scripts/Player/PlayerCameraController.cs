using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private PlayerInputReader inputReader;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float gamepadSensitivity = 100f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 60f;

    [Header("Smoothing")]
    [Tooltip("Seconds for the camera to catch up to input. 0.05–0.12 feels cinematic.")]
    [SerializeField] private float lookSmoothTime = 0.08f;

    [Header("Lock-On")]
    [SerializeField] private Transform lockOnTarget; // set/cleared by your targeting system
    [SerializeField] private Transform followPivot;  // the player-follow point this script orbits
    [Range(0f, 1f)]
    [SerializeField] private float lockOnBlend = 0.6f; // how strongly to bias toward target vs free-look

    private float targetYaw;
    private float targetPitch;
    private float yaw;
    private float pitch;
    private float yawVelocity;
    private float pitchVelocity;

    private void Awake()
    {
        targetYaw = yaw = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (inputReader == null) return;

        Vector2 look = inputReader.PlayerLookValue;
        float sensitivity = inputReader.IsUsingMouseLook
            ? mouseSensitivity
            : gamepadSensitivity * Time.deltaTime;

        targetYaw += look.x * sensitivity;
        targetPitch = Mathf.Clamp(targetPitch - look.y * sensitivity, minPitch, maxPitch);

        yaw = Mathf.SmoothDampAngle(yaw, targetYaw, ref yawVelocity, lookSmoothTime);
        pitch = Mathf.SmoothDampAngle(pitch, targetPitch, ref pitchVelocity, lookSmoothTime);

        Quaternion freeLookRotation = Quaternion.Euler(pitch, yaw, 0f);

        if (lockOnTarget != null)
        {
            Vector3 toTarget = lockOnTarget.position - followPivot.position;
            Quaternion lockRotation = Quaternion.LookRotation(toTarget.normalized);

            transform.rotation = Quaternion.Slerp(freeLookRotation, lockRotation, lockOnBlend);

            // keep targetYaw/targetPitch in sync so releasing lock-on doesn't snap
            Vector3 euler = transform.rotation.eulerAngles;
            targetYaw = yaw = euler.y;
            targetPitch = pitch = euler.x > 180f ? euler.x - 360f : euler.x;
        }
        else
        {
            transform.rotation = freeLookRotation;
        }
    }

    public void SetLookAtTarget(Transform target)
    {
        lockOnTarget = target;
    }

    public void ClearTarget()
    {
        lockOnTarget = null;
    }

}