using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public PlayerManager player;
    public bool isCameraLocked = false;
    private Vector3 cameraVelocity = Vector3.zero;
    private float rotationAngle;
    private float pivotAngle;

    public Camera cameraObject;

    [SerializeField] Transform cameraPivotTransform;
    [SerializeField] float cameraFollowSpeed = 1f;
    [SerializeField] float cameraRotationSpeed = 1f;
    [SerializeField] float cameraPivotSpeed = 1f;
    [SerializeField] float minPivot = -30f;
    [SerializeField] float maxPivot = 60f;

    [SerializeField] float cameraCollisionRadius = 0.2f;

    [SerializeField] LayerMask cameraCollisionLayerMask;
    private float defaultZPosition;
    private float targetZPosition;

    Vector3 cameraPosition;

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
        defaultZPosition = cameraObject.transform.localPosition.z;
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
            Quaternion targetRotation;
            
            cameraRotation.y = rotationAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.localRotation = targetRotation;

            cameraRotation = Vector3.zero;
            cameraRotation.x = pivotAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
            
        }
        
    }

    private void HandleCollision()
    {
        
        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();
        
        targetZPosition = defaultZPosition;

        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetZPosition), cameraCollisionLayerMask))
        {
            float hitDistance = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetZPosition = -(hitDistance - cameraCollisionRadius);
        }

        if (Mathf.Abs(targetZPosition) < cameraCollisionRadius)
        {
            targetZPosition = -cameraCollisionRadius;

        }

        cameraPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraPosition;
        
    }


}