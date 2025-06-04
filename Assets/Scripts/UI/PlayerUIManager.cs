using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{

    public static PlayerUIManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public PlayerUIHudManager playerUIHudManager;

    public PlayerUIPopUpManager playerUIPopUpManager;
    
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
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
