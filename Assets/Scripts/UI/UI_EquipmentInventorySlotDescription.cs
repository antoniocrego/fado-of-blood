using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_EquipmentInventorySlotDescription : MonoBehaviour
{
    public Image itemIcon;
    public Image highlightedIcon;

    public TextMeshProUGUI itemDescriptionText; // The text component for the description

    // This is likely the background for the icon, or a general background for the selected slot
    public Image itemIconBackground; 
    [SerializeField] public Item currentItem;

    void Awake()
    {
        // Ensure all elements start in a deselected state.
        // Crucially, hide the description text's GameObject.
        DeselectSlot(); 
    }

    public void AddItem(Item item)
    {
        if (item == null)
        {
            if (itemIcon != null) itemIcon.enabled = false;
            if (itemDescriptionText != null) itemDescriptionText.text = ""; 
            currentItem = null;
            DeselectSlot();
            return;
        }

        if (itemIcon != null) 
        {
            itemIcon.enabled = true;
            itemIcon.sprite = item.itemIcon;
        }
        
        if (itemDescriptionText != null)
        {
            itemDescriptionText.text = item.itemDescription;
        }
        currentItem = item;

        if (itemDescriptionText != null && itemDescriptionText.gameObject.activeSelf)
        {
             itemDescriptionText.gameObject.SetActive(false);
        }
        if (highlightedIcon != null) highlightedIcon.enabled = false;
        if (itemIconBackground != null) itemIconBackground.enabled = false;
    }

    public void SelectSlot()
    {
        if (highlightedIcon != null) highlightedIcon.enabled = true;
        if (itemIconBackground != null) itemIconBackground.enabled = true;

        if (itemDescriptionText != null)
        {
            itemDescriptionText.gameObject.SetActive(true);
        }
    }

    public void DeselectSlot()
    {
        if (highlightedIcon != null) highlightedIcon.enabled = false;
        if (itemIconBackground != null) itemIconBackground.enabled = false;

        if (itemDescriptionText != null)
        {
            itemDescriptionText.gameObject.SetActive(false);
        }
    }
}