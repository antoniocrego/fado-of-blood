using Unity.Mathematics;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public PlayerManager player;
    public bool isCameraLocked = false;
    private Vector3 cameraVelocity = Vector3.zero;
    private float rotationAngle = 0f;
    private float pivotAngle = 0f;

    public Camera cameraObject;

    [SerializeField] Transform cameraPivotTransform;
    [SerializeField] float cameraFollowSpeed = 1f;
    [SerializeField] float cameraRotationSpeed = 1f;
    [SerializeField] float cameraPivotSpeed = 1f;
    [SerializeField] float minPivot = -30f;
    [SerializeField] float maxPivot = 60f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (player == null)
            {
                player = FindAnyObjectByType<PlayerManager>();
                cameraObject = GetComponentInChildren<Camera>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void HandleCamera()
    {
        HandleFollow();
        HandleRotation();
        HandleCollision();
    }

    private void HandleFollow()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraFollowSpeed * Time.deltaTime);
        transform.position = targetPosition;
    }

    private void HandleRotation()
    {
        if (isCameraLocked)
        {
            return;
        }
        else
        {
            rotationAngle += PlayerInputManager.instance.cameraHorizontalInput * cameraRotationSpeed * Time.deltaTime;
            pivotAngle -= PlayerInputManager.instance.cameraVerticalInput * cameraPivotSpeed * Time.deltaTime;
            pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

            Vector3 cameraRotation = Vector3.zero;
            cameraRotation.y = rotationAngle;
            Quaternion targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            Vector3 cameraPivot = Vector3.zero;
            cameraPivot.x = pivotAngle;
            Quaternion pivotRotation = Quaternion.Euler(cameraPivot);
            cameraPivotTransform.localRotation = pivotRotation;
            
        }
        
    }

    private void HandleCollision()
    {

    }


}