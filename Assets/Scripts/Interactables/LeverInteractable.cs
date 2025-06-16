using UnityEngine;

public class LeverInteractable : Interactable
{
    [Header("Lever Settings")]
    [SerializeField] private int id = 0; // Unique ID for the lever
    [SerializeField] private Animator leverAnimator;
    [SerializeField] private string activateAnimationName = "LeverUp";
    [SerializeField] private string deactivateAnimationName = "LeverDeactivate"; 
    [SerializeField] private bool isActivated = false;
    [SerializeField] private bool canToggle = false; 

    [Header("Target Doors")]
    [SerializeField] private Door[] targetDoors;

    [Header("Sounds")]
    [SerializeField] private AudioClip activateSound;
    [SerializeField] private AudioClip deactivateSound;

    [Header("Interaction Texts")]
    [SerializeField] private string textToActivate = "Pull Lever";
    [SerializeField] private string textToDeactivate = "Reset Lever";
    [SerializeField] private string textWhenDone = "Lever Pulled";

    protected override void Awake()
    {
        base.Awake();
        if (leverAnimator == null)
        {
            leverAnimator = GetComponent<Animator>();
        }

        bool saveSaysActivated = false;

        if (WorldSaveGameManager.instance.currentCharacterData.leversPulled.ContainsKey(id))
        {
            saveSaysActivated = WorldSaveGameManager.instance.currentCharacterData.leversPulled[id];
        }
        else
        {
            WorldSaveGameManager.instance.currentCharacterData.leversPulled.Add(id, isActivated);
        }

        if (saveSaysActivated)
        {
            isActivated = true;
            interactableCollider.enabled = false; // Disable collider if lever is already activated
        }

        //Debug.Log(gameObject.name + ": LeverInteractable Awake. Target doors count: " + (targetDoors != null ? targetDoors.Length : 0));
    }

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);
        //Debug.Log(gameObject.name + ": Lever Interact called.");

        if (!canToggle && isActivated)
        {
            //Debug.Log(gameObject.name + ": Lever cannot toggle and is already activated. Returning.");
            return;
        }

        isActivated = !isActivated;
        //Debug.Log(gameObject.name + ": Lever isActivated state now: " + isActivated);

        if (leverAnimator != null)
        {
            if (isActivated && !string.IsNullOrEmpty(activateAnimationName))
            {
                leverAnimator.Play(activateAnimationName);
            }
            else if (!isActivated && !string.IsNullOrEmpty(deactivateAnimationName)) 
            {
                leverAnimator.Play(deactivateAnimationName);
            }
            else if (!isActivated && string.IsNullOrEmpty(deactivateAnimationName) && !string.IsNullOrEmpty(activateAnimationName))
            {
                 leverAnimator.Play(activateAnimationName); 
            }
        }
        

        if (targetDoors == null || targetDoors.Length == 0)
        {
            Debug.LogWarning(gameObject.name + ": No target doors assigned to this lever!");
            return;
        }

        //Debug.Log(gameObject.name + ": Processing " + targetDoors.Length + " target door(s).");
        foreach (Door door in targetDoors)
        {
            if (door != null)
            {
                door.SetLockState(!isActivated); 
                door.OperateByExternal();
            }
            else
            {
                Debug.LogWarning(gameObject.name + ": Found a null entry in targetDoors array.");
            }
        }
    }

    public override string GetInteractableText()
    {
        if (!canToggle && isActivated) return textWhenDone;
        return isActivated ? textToDeactivate : textToActivate;
    }
}