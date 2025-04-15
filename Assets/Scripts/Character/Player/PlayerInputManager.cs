using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    PlayerControls playerControls;

    [SerializeField] Vector2 movementInput;

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
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        SceneManager.activeSceneChanged += OnSceneChange;   
        
        instance.enabled = true;
    }

    private void OnSceneChange(Scene current, Scene next)
    {
        // TODO: CHANGE THIS TO USE SCENE NUMBERS DEFINED ELSEWHERE
        if (next.buildIndex == 0){
            instance.enabled = false;
        }
        else{
            instance.enabled = true;
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
}
