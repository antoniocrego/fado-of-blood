using UnityEngine;

public class Door : Interactable
{
    [Header("Door Settings")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string openAnimationName = "DoorOpen";
    [SerializeField] private string closeAnimationName = "DoorClose";
    [SerializeField] private bool isOpen = false;
    [SerializeField] private Collider physicalBarrierCollider; 


    [Header("Lock Settings")]
    [SerializeField] private bool isLockedByDefault = true;
    [SerializeField] private bool requiresKeyToUnlock = false;
    [SerializeField] private int requiredKeyID = 0; 
    [SerializeField] private string keyItemNameForMessage = "a specific key"; 
    [SerializeField] private bool consumeKeyOnUnlock = false;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioClip lockedSound;
    [SerializeField] private AudioClip unlockSound;

    [Header("Interaction Texts")]
    [SerializeField] private string textWhenOpen = "Close Door";
    [SerializeField] private string textWhenClosed = "Open Door";
    [SerializeField] private string textWhenLockedNeedKey = "Unlock Door";
    [SerializeField] private string textWhenLockedNoKey = "Locked";

    private bool currentLockState; 
    private bool isPermanentlyUnlockedByKey = false; 

    protected override void Awake()
    {
        base.Awake(); 
        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
        }
        currentLockState = isLockedByDefault;

        if (physicalBarrierCollider == null)
        {
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                if (col != interactableCollider) 
                {
                    physicalBarrierCollider = col;
                    break;
                }
            }
        }

        if (physicalBarrierCollider != null)
        {
            physicalBarrierCollider.isTrigger = false; 
            physicalBarrierCollider.enabled = !isOpen; 
        }
    }

    public void OperateByExternal()
    {
        Debug.Log(gameObject.name + ": OperateByExternal called. isPermanentlyUnlockedByKey = " + isPermanentlyUnlockedByKey + ", currentLockState = " + currentLockState); 
        if (isPermanentlyUnlockedByKey || !currentLockState)
        {
            // Deactivate the collider of the door 
            interactableCollider.enabled = false;
            ToggleDoorState();
        }
        else
        {
            Debug.Log(gameObject.name + " is locked.");
        }
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
            if (physicalBarrierCollider != null)
                physicalBarrierCollider.enabled = false; 
        }
        else
        {
            if (physicalBarrierCollider != null)
                physicalBarrierCollider.enabled = true; 
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
        if (isOpen) return textWhenOpen;
        if (currentLockState && !isPermanentlyUnlockedByKey)
        {
            return requiresKeyToUnlock ? textWhenLockedNeedKey : textWhenLockedNoKey;
        }
        return textWhenClosed;
    }

}