
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using System.Net.Http.Headers;

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
    private float defaultYPosition;

    private List<GameObject> availableTargets = new List<GameObject>();
    public GameObject currentLockOnTarget;



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
        defaultYPosition = cameraPivotTransform.localPosition.y;
    }

    public void HandleCamera()
    {
        HandleFollow();
        HandleRotation();
        HandleCollision();
    }

    private void HandleFollow()
    {
        Vector3 targetPosition;
        if(isCameraLocked && currentLockOnTarget != null)
        {
            targetPosition = Vector3.SmoothDamp(transform.position, currentLockOnTarget.transform.position, ref cameraVelocity, cameraFollowSpeed * Time.deltaTime);
        }
        else 
        {
            targetPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraFollowSpeed * Time.deltaTime);
        }
        transform.position = targetPosition;
    }

    private void HandleRotation()
    {
        if (isCameraLocked)
        {
            if (currentLockOnTarget != null)
            {
                Vector3 pivotPos = cameraPivotTransform.localPosition;
                float targetY = player.transform.position.y - currentLockOnTarget.transform.position.y;
                pivotPos.y = Mathf.Lerp(pivotPos.y, targetY+defaultYPosition, 0.2f);
                cameraPivotTransform.localPosition = pivotPos;

                Vector3 direction = currentLockOnTarget.transform.position - player.transform.position;
                float dist = -Vector3.Distance(player.transform.position, currentLockOnTarget.transform.position);
                dist += defaultZPosition;
                cameraObject.transform.localPosition = new Vector3(cameraObject.transform.localPosition.x, cameraObject.transform.localPosition.y, dist);
                direction.Normalize();
                direction.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraFollowSpeed*5 * Time.deltaTime);

                direction = currentLockOnTarget.transform.position - cameraPivotTransform.position;
                direction.Normalize();
                targetRotation = Quaternion.LookRotation(direction);
                targetRotation.x = 0;
                targetRotation.z = 0;
                cameraPivotTransform.localRotation = Quaternion.Slerp(cameraPivotTransform.localRotation, targetRotation, cameraFollowSpeed * Time.deltaTime);

                rotationAngle = transform.eulerAngles.y;
                pivotAngle = cameraPivotTransform.eulerAngles.x;
                pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

            }
            else
            {
                isCameraLocked = false;
                currentLockOnTarget = null;
            }
        }
        else
        {
            Vector3 pivotPos = cameraPivotTransform.localPosition;
            pivotPos.y = Mathf.Lerp(pivotPos.y, defaultYPosition, 0.2f);
            cameraPivotTransform.localPosition = pivotPos;
            cameraObject.transform.localPosition = new Vector3(cameraObject.transform.localPosition.x, cameraObject.transform.localPosition.y, defaultZPosition);
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

    public GameObject FindLockOnTarget()
    {
        availableTargets.Clear();
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, player.lockOnRange, player.lockOnLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Debug.Log("Found "+colliders.Length+" colliders ");
            if (colliders[i].transform == player.transform || colliders[i].transform.IsChildOf(player.transform))
            {
                Debug.Log("Found player: ");
                continue;
            }
            availableTargets.Add(colliders[i].gameObject);

            
            /*
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();
            
            if (character == null || character == player || character.isDead)
            {
                continue;
            }

            float currentAngle = Vector3.Angle(player.transform.forward, (character.transform.position - player.transform.position).normalized);
            if (currentAngle <= player.fieldOfView / 2)
            {
                RaycastHit hit;
                if (Physics.Linecast(player.transform.position, character.transform.position, out hit, 0))
                {
                    continue;
                }
                availableTargets.Add(character);
            }
            */
        }
        availableTargets.Sort((x, y) => Vector3.Distance(player.transform.position, x.transform.position).CompareTo(Vector3.Distance(player.transform.position, y.transform.position)));
        if (availableTargets.Count > 0)
        {
            currentLockOnTarget = availableTargets[0];
        }
        else
        {
            currentLockOnTarget = null;
        }
        return currentLockOnTarget;

    }


}