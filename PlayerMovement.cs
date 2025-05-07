using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float mouseSensitivity = 2f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Transform cameraHolder;
    private float verticalRotation = 0f;

    private Vector3 velocity;
    private bool isGrounded;

    [SerializeField] private Transform headBone;
    [SerializeField] private float headRotationLimit = 45f;

    private float cameraPitch = 0f;

    private PlayerStats playerStats;
    private Vector3 impact = Vector3.zero;

    public AudioClip footstepSound;
    public float walkStepDelay = 0.5f;
    public float runStepDelay = 0.3f;

    private AudioSource audioSource;
    private bool isMoving = false;
    private bool isPlayingFootsteps = false;

    public Animator bodyAnimator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraHolder = transform.Find("CameraHolder");
        playerStats = GetComponent<PlayerStats>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.spatialBlend = 1f;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        Move();
        Look();
    }

    void Move()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && playerStats.HasStamina();
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        controller.Move(move * currentSpeed * Time.deltaTime);

        if (isRunning)
            playerStats.DrainStamina(playerStats.staminaDrainRate);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (impact.magnitude > 0.2f)
        {
            controller.Move(impact * Time.deltaTime);
            impact = Vector3.Lerp(impact, Vector3.zero, 5f * Time.deltaTime);
        }

        isMoving = move.magnitude > 0.1f && isGrounded;

        if (isMoving && !isPlayingFootsteps)
            StartCoroutine(PlayFootsteps(isRunning));

        if (bodyAnimator != null)
        {
            if (!isMoving)
                bodyAnimator.Play("Idle");
            else if (isRunning)
                bodyAnimator.Play("Run");
            else
                bodyAnimator.Play("Walk");
        }
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -headRotationLimit, headRotationLimit);

        cameraHolder.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

        if (headBone != null)
            headBone.localRotation = Quaternion.Euler(0f, 0f, cameraPitch);
    }

    public void AddImpact(Vector3 force)
    {
        impact += force;
    }

    IEnumerator PlayFootsteps(bool running)
    {
        isPlayingFootsteps = true;

        if (footstepSound != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(footstepSound);
        }

        float delay = running ? runStepDelay : walkStepDelay;

        yield return new WaitForSeconds(delay);

        isPlayingFootsteps = false;
    }
}
