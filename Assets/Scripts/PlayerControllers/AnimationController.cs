using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    public GameObject playerState;
    private Animator animator;
    private Transform mainCamera;

    // Animation parameters
    private readonly int runningParam = Animator.StringToHash("Running");
    private readonly int leftParam = Animator.StringToHash("Left");
    private readonly int rightParam = Animator.StringToHash("Right");
    private readonly int backwardsParam = Animator.StringToHash("Backwards");
    private readonly int jumpingParam = Animator.StringToHash("Jumping");
    private readonly int wallParam = Animator.StringToHash("Wallrun");
    private readonly int angleParam = Animator.StringToHash("Wallangle");

    // Movement variables
    private Vector3 movement;
    private bool isJumping;
    private float initialYPosition;

    [SerializeField] private Vector3 backwardsOffset = new Vector3(0, 1, -1.5f);

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the character prefab.");
        }

        mainCamera = Camera.main.transform;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found in the scene.");
        }

        initialYPosition = transform.position.y;

        // Make the character a child of the camera for position synchronization
        // transform.parent = mainCamera;
        // transform.localPosition = backwardsOffset; // Center the character on the camera
        transform.localRotation = Quaternion.identity; // Reset rotation
        
    }

    void Update()
    {
        HandleMovementInput();
        HandleJumpInput();
        UpdateAnimator();
        SyncRotationWithCamera();
    }

    void HandleMovementInput()
    {
        // Get input from the player
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate movement direction
        movement = new Vector3(horizontal, 0, vertical).normalized;

        // Update animator parameters based on movement
        animator.SetBool(runningParam, movement.magnitude > 0.1f);
        animator.SetBool(leftParam, horizontal < -0.1f);
        animator.SetBool(rightParam, horizontal > 0.1f);
        animator.SetBool(backwardsParam, vertical < -0.1f);

        animator.SetBool(wallParam, playerState.GetComponent<PlayerMovement>().onWall);

        animator.SetBool(angleParam, playerState.GetComponent<PlayerMovement>().wallRunRotation > 0);
    }

    void HandleJumpInput()
    {
        if (Input.GetButton("Jump") && !isJumping)
        {
            isJumping = true;
            animator.SetBool(jumpingParam, isJumping);
        }
    }

    void UpdateAnimator()
    {
        // Reset jumping state when landing (you can detect landing using raycasts or colliders)
        if (isJumping && playerState.GetComponent<PlayerMovement>().readyToJump)
        {
            isJumping = false;
            animator.SetBool(jumpingParam, isJumping);
        }
    }

    void SyncRotationWithCamera()
    {
        // Sync the character's X and Z position with the camera
        // Vector3 cameraPosition = mainCamera.position;
        // transform.position = new Vector3(cameraPosition.x, cameraPosition.y-backwardsOffset.y, cameraPosition.z);

        // Sync the character's Y rotation (yaw) with the camera's Y rotation
        Vector3 cameraEuler = mainCamera.eulerAngles;
        transform.rotation = Quaternion.Euler(0, cameraEuler.y, 0);
    }
}