using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public static PlayerCamera instance;

    public Camera cameraObject;
    private GameObject player; // Reference to the player transform
    [SerializeField] private GameObject target; // Reference to the player transform
    [SerializeField] private float distance = 5f; // Distance from the player
    [SerializeField] private float rotationSpeed = 5f; // Speed of camera rotation
    [SerializeField] private float followSpeed = 10f; // Speed of camera following the player
    [SerializeField] private float minDistance = 2f; // Minimum distance from the player
    [SerializeField] private float maxDistance = 10f; // Maximum distance from the player
    [SerializeField] private float zoomSpeed = 2f; // Speed of zooming in and out

    private float currentXRotation; // Current X rotation of the camera
    private float currentYRotation; // Current Y rotation of the camera
    private float currentDistance; // Current distance from the player

    private bool reset = true; // Last camera movement time

    private float cameraResetTime = 1f; // Time for camera reset

    private float cameraTimer = 0f;

    public bool isCameraLocked = false; // Flag to check if the camera is locked to a target

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target object not found. Please assign the target object in the inspector or ensure it has the 'Player' tag.");
            return;
        }
    }


    // Update is called once per frame
    void LateUpdate()
    {
        if(!isCameraLocked)
        {
             // Get mouse input for rotation
          float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
          float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

          currentYRotation += mouseX; // Update the X rotation based on mouse input
          currentXRotation -= mouseY; // Update the Y rotation based on mouse input

          if (mouseX == 0 && mouseY == 0)
          {
              cameraTimer += Time.deltaTime; // Increment the timer if no mouse movement
          }
          else
          {
              reset = false; // Reset the camera position if there is mouse movement
              cameraTimer = 0f; // Reset the timer if there is mouse movement
          }

          if (cameraTimer >= cameraResetTime)
          {
              reset = true; // Set the reset flag if the timer exceeds the reset time
          }

          // Clamp the vertical rotation to prevent flipping
          currentXRotation = Mathf.Clamp(currentXRotation, -30f, 60f);

          // Get keyboard input for zooming
          float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
          currentDistance = Mathf.Clamp(currentDistance - scroll, minDistance, maxDistance);    

          Quaternion rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);
          if(reset)
          {
              rotation = Quaternion.Euler(target.transform.parent.rotation.eulerAngles.x, target.transform.parent.rotation.eulerAngles.y, 0);
              currentXRotation = target.transform.parent.rotation.eulerAngles.x;
              currentYRotation = target.transform.parent.rotation.eulerAngles.y;
              currentDistance = distance;
          }

          Vector3 direction = new Vector3(0, 0, currentDistance); // Calculate the direction from the player to the camera
          Vector3 targetPosition = target.transform.position - rotation * direction;
          transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

          transform.LookAt(target.transform.position);
        } 
        else 
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            if (boss != null)
            {
                currentXRotation += Input.GetAxis("Mouse X") * rotationSpeed;

                Vector3 directionToBoss = (boss.transform.position - player.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(directionToBoss);

                Vector3 offset = -directionToBoss * distance + Vector3.up * height;
                Vector3 targetPosition = player.transform.position + offset;

                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
                transform.LookAt(boss.transform.position);
            }
        }
    }

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
}
