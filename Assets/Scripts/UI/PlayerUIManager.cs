using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{

    public static PlayerUIManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public PlayerUIHudManager playerUIHudManager;

    private void Awake()
    {
        if(instance == null) 
        {
            instance = this; 
        }
        else 
        {
            Destroy(gameObject);
        }
        playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
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
