using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required for EventTrigger
using TMPro; // Make sure to import TextMeshPro

public class PlayerUIInventoryManager : PlayerUIMenu
{
    [Header("Menu & Main Inventory UI")]
    [SerializeField] GameObject inventoryWindow; 
    [SerializeField] Transform inventoryContentWindow; 
    [SerializeField] GameObject equipmentInventorySlotPrefab;

    [Header("Item Details Panel UI Elements")]
    [SerializeField] private Image detailItemIcon;
    [SerializeField] private TextMeshProUGUI detailItemNameText;
    [SerializeField] private TextMeshProUGUI detailItemDescriptionText;

    private List<UI_EquipmentInventorySlotDescription> currentSlotScripts = new List<UI_EquipmentInventorySlotDescription>();
    private UI_EquipmentInventorySlotDescription currentlySelectedSlotScript;

    // Corrected RefreshMenu
    public void RefreshMenu()
    {
        ClearInventoryWindow(); 
        LoadInventoryItems();
        if (currentlySelectedSlotScript == null)
        {
            ClearAndHideDetailsPanel();
        }
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
        ClearAndHideDetailsPanel(); 
    }

    public override void OpenMenu()
    {
        base.OpenMenu();
        ClearInventoryWindow();
        LoadInventoryItems();
        if (currentlySelectedSlotScript == null)
        {
             ClearAndHideDetailsPanel();
        }
    }

    private void LoadInventoryItems()
    {
        PlayerManager player = FindAnyObjectByType<PlayerManager>();

        ClearInventoryWindow(); 

        if (player == null || player.playerInventoryManager == null)
        {
            Debug.LogError("PlayerManager or PlayerInventoryManager not found.");
            ClearAndHideDetailsPanel(); 
            return;
        }

        List<Item> allItemsInInventory = new List<Item>();
        for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            Item item = player.playerInventoryManager.itemsInInventory[i];
            if (item != null)
            {
                allItemsInInventory.Add(item);
            }
        }

        if (allItemsInInventory.Count <= 0)
        {
            Debug.Log("Inventory is empty.");
            ClearAndHideDetailsPanel(); 
            return;
        }

        bool hasAutoSelectedFirst = false;

        for (int i = 0; i < allItemsInInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, inventoryContentWindow);
            UI_EquipmentInventorySlotDescription slotScript = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlotDescription>();

            if (slotScript != null)
            {
                slotScript.AddItem(allItemsInInventory[i]);
                currentSlotScripts.Add(slotScript); 

                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                EventTrigger trigger = inventorySlotGameObject.GetComponent<EventTrigger>() ?? inventorySlotGameObject.AddComponent<EventTrigger>();

                if (inventorySlotButton != null)
                {
                    inventorySlotButton.onClick.AddListener(() => HandleSlotSelection(slotScript));

                    EventTrigger.Entry entrySelect = new EventTrigger.Entry { eventID = EventTriggerType.Select };
                    entrySelect.callback.AddListener((eventData) => { HandleSlotSelection(slotScript); });
                    trigger.triggers.Add(entrySelect);

                    EventTrigger.Entry entryDeselect = new EventTrigger.Entry { eventID = EventTriggerType.Deselect };
                    entryDeselect.callback.AddListener((eventData) => {
                    });
                    trigger.triggers.Add(entryDeselect);


                    if (!hasAutoSelectedFirst)
                    {
                        inventorySlotButton.Select(); 
                        hasAutoSelectedFirst = true;
                    }
                }
            }
            else
            {
                Debug.LogError("Instantiated inventory slot prefab does not have a UI_EquipmentInventorySlotDescription component.");
            }
        }
        Debug.Log("The total number of items in the inventory is: " + allItemsInInventory.Count);
    }

    private void HandleSlotSelection(UI_EquipmentInventorySlotDescription selectedSlotScript)
    {
        if (currentlySelectedSlotScript != null && currentlySelectedSlotScript != selectedSlotScript)
        {
            currentlySelectedSlotScript.DeselectSlot(); 
        }

        currentlySelectedSlotScript = selectedSlotScript;

        if (currentlySelectedSlotScript != null)
        {
            currentlySelectedSlotScript.SelectSlot(); 
            PopulateDetailsPanel(currentlySelectedSlotScript.currentItem);
        }
        else
        {
            ClearAndHideDetailsPanel();
        }
    }

    private void PopulateDetailsPanel(Item itemToDisplay)
    {
        if (inventoryWindow == null)
        {
            Debug.LogError("Inventory Window (Details Panel) is not assigned in the Inspector!");
            return;
        }

        if (itemToDisplay == null)
        {
            ClearAndHideDetailsPanel();
            return;
        }

        inventoryWindow.SetActive(true); 

        if (detailItemIcon != null)
        {
            detailItemIcon.sprite = itemToDisplay.itemIcon;
            detailItemIcon.enabled = (itemToDisplay.itemIcon != null);
        }
        if (detailItemNameText != null)
        {
            detailItemNameText.text = itemToDisplay.itemName; 
        }
        if (detailItemDescriptionText != null)
        {
            detailItemDescriptionText.text = itemToDisplay.itemDescription;
        }
    }

    private void ClearAndHideDetailsPanel()
    {
        if (inventoryWindow != null)
        {
            inventoryWindow.SetActive(false); 
        }

        if (detailItemIcon != null)
        {
            detailItemIcon.sprite = null;
            detailItemIcon.enabled = false;
        }
        if (detailItemNameText != null)
        {
            detailItemNameText.text = "";
        }
        if (detailItemDescriptionText != null)
        {
            detailItemDescriptionText.text = ""; 
        }
    }

    private void ClearInventoryWindow()
    {
        foreach (Transform itemTransform in inventoryContentWindow) 
        {
            Destroy(itemTransform.gameObject);
        }
        currentSlotScripts.Clear();
        currentlySelectedSlotScript = null;
    }
}