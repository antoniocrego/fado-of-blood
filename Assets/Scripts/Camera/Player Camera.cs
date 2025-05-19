
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

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
    private float defaultCameraHeight = 1.7f;
    private float lockedCameraHeight = 2f;

    [Header("Lock On")]

    public CharacterManager currentLockOnTarget;
    public CharacterManager leftLockOnTarget;
    public CharacterManager rightLockOnTarget;
    [SerializeField] float lockOnTargetFollowSpeed = 0.2f;
    [SerializeField] float setHeightSpeed = 0.07f;


    private List<CharacterManager> availableTargets = new List<CharacterManager>();
    private Coroutine cameraHeightCoroutine;


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
        Vector3 targetPosition;
        targetPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraFollowSpeed * Time.deltaTime);
        transform.position = targetPosition;
    }

    private void HandleRotation()
    {
        if (player.isLockedOn)
        {
            Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - transform.position;
            rotationDirection.Normalize();
            rotationDirection.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraFollowSpeed * Time.deltaTime);

            rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;
            rotationDirection.Normalize();

            targetRotation = Quaternion.LookRotation(rotationDirection);
            cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

            rotationAngle = transform.eulerAngles.y;
            pivotAngle = transform.eulerAngles.x;

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

    public void HandleLockOnTarget()
    {/*
        availableTargets.Clear();
        
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].transform == player.transform || colliders[i].transform.IsChildOf(player.transform))
            {
                continue;
            }

            float currentAngle = Vector3.Angle(cameraObject.transform.forward, (colliders[i].gameObject.transform.position - cameraObject.transform.position).normalized);
            if (currentAngle <= player.fieldOfView)
            {
                RaycastHit hit;
                if (Physics.Linecast(player.transform.position, colliders[i].gameObject.transform.position, out hit, 0))
                {
                    continue;
                }
                availableTargets.Add(colliders[i].gameObject);
            }
        }
        availableTargets.Sort((x, y) => Vector3.Distance(player.transform.position, x.transform.position).CompareTo(Vector3.Distance(player.transform.position, y.transform.position)));
       */
    }

    public void HandleLocatingLockOnTargets()
    {
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, player.lockOnRange, WorldUtilityManager.Instance.GetCharacterLayer());

        float closestDistance = Mathf.Infinity;
        float closestLeft = -Mathf.Infinity;
        float closestRight = Mathf.Infinity;
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();
            if (lockOnTarget == null)
            {
                continue;
            }
            Vector3 lockOnTargetDirection = lockOnTarget.transform.position - player.transform.position;
            float angle = Vector3.SignedAngle(player.transform.forward, lockOnTargetDirection, Vector3.up);

            if (lockOnTarget.isDead)
            {
                continue;
            }

            if (lockOnTarget.transform.root == player.transform.root)
            {
                continue;
            }

            if (player.minFov > angle || player.maxFov < angle)
            {
                continue;
            }

            RaycastHit hit;
            if (Physics.Linecast(player.playerCombatManager.lockOnTransform.position, lockOnTarget.characterCombatManager.lockOnTransform.position, out hit, WorldUtilityManager.Instance.GetEnviroLayer()))
            {
                continue;
            }
            availableTargets.Add(lockOnTarget);
        }

        for (int i = 0; i < availableTargets.Count; i++)
        {
            if (availableTargets[i] == null)
            {
                ClearLockOnTargets();
                player.isLockedOn = false;
                continue;
            }
            float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[i].transform.position);
            if (distanceFromTarget < closestDistance)
            {
                closestDistance = distanceFromTarget;
                currentLockOnTarget = availableTargets[i];
            }

            if (player.isLockedOn)
            {
                if (availableTargets[i] == currentLockOnTarget || availableTargets[i] == player.playerCombatManager.currentTarget)
                {
                    continue;
                }

                Vector3 relativePosition = player.transform.InverseTransformPoint(availableTargets[i].transform.position);
                if (relativePosition.x <= 0.00 && relativePosition.x > closestLeft)
                {
                    closestLeft = relativePosition.x;
                    leftLockOnTarget = availableTargets[i];
                }
                else if (relativePosition.x >= 0.00 && relativePosition.x < closestRight)
                {
                    closestRight = relativePosition.x;
                    rightLockOnTarget = availableTargets[i];
                }


            }

        }
    }

    public void ClearLockOnTargets()
    {
        currentLockOnTarget = null;
        leftLockOnTarget = null;
        rightLockOnTarget = null;
        availableTargets.Clear();
    }

    public IEnumerator WaitThenFindNewTarget()
    {
        while (player.isPerformingAction)
        {
            yield return null;
        }

        ClearLockOnTargets();
        HandleLocatingLockOnTargets();

        if (currentLockOnTarget != null)
        {
            Debug.Log("changing target");
            player.playerCombatManager.SetTarget(currentLockOnTarget);
            player.isLockedOn = true;
        }

        yield return null;

    }

    public void SetLockCameraHeight()
    {
        if (cameraHeightCoroutine != null)
        {
            StopCoroutine(cameraHeightCoroutine);
        }
        cameraHeightCoroutine = StartCoroutine(SetCameraHeight());
    }

    private IEnumerator SetCameraHeight()
    {
        float duration = 1f;
        float timer = 0f;

        Vector3 velocity = Vector3.zero;
        Vector3 lockedPosition = new Vector3(cameraPivotTransform.localPosition.x, lockedCameraHeight, cameraPivotTransform.localPosition.z);
        Vector3 defaultPosition = new Vector3(cameraPivotTransform.localPosition.x, defaultCameraHeight, cameraPivotTransform.localPosition.z);
        while (timer < duration)
        {
            timer += Time.deltaTime;
            if (player != null)
            {
                if (player.playerCombatManager.currentTarget != null)
                {
                    cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, lockedPosition, ref velocity, 0.05f);
                    cameraPivotTransform.transform.localRotation = Quaternion.Slerp(cameraPivotTransform.localRotation, Quaternion.Euler(0, 0, 0), lockOnTargetFollowSpeed);
                }
                else
                {
                    cameraPivotTransform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, defaultPosition, ref velocity, 0.05f);
                }
            }
            yield return null;

        }

        if (player != null)
        {
            if (player.playerCombatManager.currentTarget != null)
            {
                cameraPivotTransform.transform.localPosition = lockedPosition;
                cameraPivotTransform.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                cameraPivotTransform.localPosition = defaultPosition;
            }
        }
    }


}