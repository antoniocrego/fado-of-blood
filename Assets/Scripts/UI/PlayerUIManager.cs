using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{

    [Header("STAT BARS")]
    // [SerializeField] UI_StatBar healthBar;
    // [SerializeField] UI_StatBar staminaBar;

    [Header("QUICK SLOTS")]
    [SerializeField] Image rightWeaponQuickSlotIcon;
    [SerializeField] Image leftWeaponQuickSlotIcon;

    public static PlayerUIManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [HideInInspector] public PlayerManager playerManager;

    public PlayerUIHudManager playerUIHudManager;

    public PlayerUIPopUpManager playerUIPopUpManager;

    public PlayerUICharacterMenuManager playerUICharacterMenuManager;

    public PlayerUIEquipmentManager playerUIEquipmentManager;

    public PlayerUIInventoryManager playerUIInventoryManager;

    public bool menuWindowIsOpen = false;
    public bool popUpWindowIsOpen = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
        playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
        playerUICharacterMenuManager = GetComponentInChildren<PlayerUICharacterMenuManager>();
        playerUIEquipmentManager = GetComponentInChildren<PlayerUIEquipmentManager>();
        playerUIInventoryManager = GetComponentInChildren<PlayerUIInventoryManager>();
        playerManager = FindFirstObjectByType<PlayerManager>();
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseAllMenuWindows()
    {
        playerUICharacterMenuManager.CloseCharacterMenu();
        playerUIEquipmentManager.CloseEquipmentManagerMenu();
        playerUIInventoryManager.CloseInventoryManagerMenu();
        
    }
    
    public void SetRightWeaponQuickSlotIcon(int weaponID)
    {
        WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);

        if (weapon == null)
        {
            Debug.Log("ITEM IS NULL");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if (weapon.itemIcon == null)
        {
            Debug.Log("ITEM HAS NO ICON");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        //  THIS IS WHERE YOU WOULD CHECK TO SEE IF YOU MEET THE ITEMS REQUIREMENTS IF YOU WANT TO CREATE THE WARNING FOR NOT BEING ABLE TO WIELD IT IN THE UI

        rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        rightWeaponQuickSlotIcon.enabled = true;
    }

    public void SetLeftWeaponQuickSlotIcon(int weaponID)
    {
        WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);

        if (weapon == null)
        {
            Debug.Log("ITEM IS NULL");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        if (weapon.itemIcon == null)
        {
            Debug.Log("ITEM HAS NO ICON");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        //  THIS IS WHERE YOU WOULD CHECK TO SEE IF YOU MEET THE ITEMS REQUIREMENTS IF YOU WANT TO CREATE THE WARNING FOR NOT BEING ABLE TO WIELD IT IN THE UI

        leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        leftWeaponQuickSlotIcon.enabled = true;
    }
}
