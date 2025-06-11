using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Interactable : MonoBehaviour
    {
        public string interactableText; 
        [SerializeField] protected Collider interactableCollider;   

        protected virtual void Awake()
        {
            if (interactableCollider == null)
                interactableCollider = GetComponent<Collider>();
        }
        protected virtual void Start()
        {

        }

    public virtual void Interact(PlayerManager player)
    {
        Debug.Log("YOU HAVE INTERACTED!");

        player.playerInteractionManager.RemoveInteractionFromList(this);
        PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();    
        interactableCollider.enabled = false;

    }

    public virtual string GetInteractableText()
    {
        return interactableText;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();
        Debug.Log("OnTriggerEnter Interactable");
        if (player != null)
        {
            Debug.Log("PlayerManager found in Interactable");
            player.playerInteractionManager.AddInteractionToList(this);
        }
    }

        public virtual void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                player.playerInteractionManager.RemoveInteractionFromList(this);
                PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();
            }
        }
    }

