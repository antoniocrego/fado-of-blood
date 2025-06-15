using UnityEngine;
using System.Collections;
using FMODUnity;

public class Door : Interactable
{
    PlayerManager player;

    [Header("Door Settings")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string openAnimationName = "DoorOpen";
    [SerializeField] private string closeAnimationName = "DoorClose";
    [SerializeField] private bool isOpen = false;
    [SerializeField] Transform doorToBeRotated;
    [SerializeField] bool openToTheRight = true; // Determines the direction of the door opening


    [Header("Lock Settings")]
    [SerializeField] private bool isLockedByDefault = true;
    [SerializeField] private bool requiresKeyToUnlock = false;
    [SerializeField] private int requiredKeyID = 0; 
    [SerializeField] private string keyItemNameForMessage = "a specific key"; 
    [SerializeField] private bool consumeKeyOnUnlock = false;

    [Header("Sounds")]
    [SerializeField] private string doorOpenSound = "event:/Environment/Doors/Wooden Door Open";

    [Header("Interaction Texts")]
    [SerializeField] private string textWhenOpen = "Close Door";
    [SerializeField] private string textWhenClosed = "Open Door";
    [SerializeField] private string textWhenLockedNeedKey = "You need a key!";
    [SerializeField] private string textWhenLockedNoKey = "The door is locked by a device!";

    private bool currentLockState; 
    private bool isPermanentlyUnlockedByKey = false; 
    
    private Coroutine _activeRotationCoroutine; 

    [SerializeField] private float manualRotationDuration = 1.0f;


    protected override void Awake()
    {
        base.Awake();
        // Get the player from the scene 
        player = FindFirstObjectByType<PlayerManager>();

        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
        }
        currentLockState = isLockedByDefault;
    }

    public void OperateByExternal()
    {
        // Debug.Log(gameObject.name + ": OperateByExternal called. isPermanentlyUnlockedByKey = " + isPermanentlyUnlockedByKey + ", currentLockState = " + currentLockState); 
        if (isPermanentlyUnlockedByKey || !currentLockState)
        {
            if (interactableCollider != null)
            {
                interactableCollider.enabled = false;
            }
            ToggleDoorState(); 
        }
        else
        {
            Debug.Log(gameObject.name + " is locked.");
        }
    }

    private IEnumerator SmoothlyRotate(Transform objectToRotate, Quaternion endRotation, float duration)
    {
        if (objectToRotate == null) yield break;

        Quaternion startRotation = objectToRotate.rotation;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            objectToRotate.rotation = Quaternion.Slerp(startRotation, endRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        objectToRotate.rotation = endRotation;
        _activeRotationCoroutine = null;
    }

    public override void Interact(PlayerManager player)
    {
        
        if (isPermanentlyUnlockedByKey || !currentLockState)
        {
            base.Interact(player);
            ToggleDoorState();
        }
        else if (currentLockState && requiresKeyToUnlock)
        {
            if (player.playerInventoryManager == null || WorldItemDatabase.Instance == null)
            {
                Debug.LogError("PlayerInventoryManager or WorldItemDatabase not found!");
                return;
            }

            if (player.playerInventoryManager.HasItem(requiredKeyID))
            {
                isPermanentlyUnlockedByKey = true;
                currentLockState = false;

                if (consumeKeyOnUnlock)
                {
                    ConsumableItem keyItem = WorldItemDatabase.Instance.GetItemByID(requiredKeyID);
                    Debug.Log("Unlocking door with key: " + keyItem);
                    if (keyItem != null)
                    {
                        Debug.Log("Using key: " + keyItem.itemName);
                        player.playerInventoryManager.RemoveItemFromInventory(keyItem);
                    }
                }
                base.Interact(player);
                ToggleDoorState();
            }
        }
    }

     private void ToggleDoorState()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            Transform doorRotationObjectTransform = doorToBeRotated;
            if (doorRotationObjectTransform != null)
            {
                if (_activeRotationCoroutine != null)
                {
                    StopCoroutine(_activeRotationCoroutine);
                }
                float neededRotation = openToTheRight ? 90f : -90f;
                Quaternion targetLocalRotation = Quaternion.Euler(doorRotationObjectTransform.transform.localEulerAngles.x, doorRotationObjectTransform.transform.localEulerAngles.y + neededRotation, doorRotationObjectTransform.transform.localEulerAngles.z);
                Quaternion targetRotation = doorRotationObjectTransform.parent.rotation * targetLocalRotation;
                _activeRotationCoroutine = StartCoroutine(SmoothlyRotate(doorRotationObjectTransform, targetRotation, manualRotationDuration));
            }

            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true, hideWeapons: true);
            RuntimeManager.PlayOneShot(doorOpenSound, transform.position);
        }
    }
    
    
    public void SetLockState(bool lockDoor)
    {
        if (!isPermanentlyUnlockedByKey) 
        {
            currentLockState = lockDoor;
            Debug.Log(gameObject.name + " lock state set to: " + currentLockState);
        }
    }

    public override string GetInteractableText()
    {
        if(counter == 0) 
        {
            interactableText = "Open the door";
            counter++; 
            return interactableText; 
        }
        if (isOpen)
        {
            interactableText = textWhenOpen;
            return textWhenOpen;
        }
        if (currentLockState && !isPermanentlyUnlockedByKey)
        {
            interactableText = requiresKeyToUnlock ? textWhenLockedNeedKey : textWhenLockedNoKey;
            return requiresKeyToUnlock ? textWhenLockedNeedKey : textWhenLockedNoKey;
        }
        return textWhenClosed;
    }

}