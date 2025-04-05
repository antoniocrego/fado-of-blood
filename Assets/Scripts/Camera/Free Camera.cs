using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public float moveSpeed = 10f; // Movement speed
    public float lookSpeedX = 2f; // Horizontal mouse look speed
    public float lookSpeedY = 2f; // Vertical mouse look speed
    public float upperLimit = 80f; // Max vertical rotation angle
    public float lowerLimit = 80f; // Min vertical rotation angle

    private float rotationX = 0f; // Current rotation along X (vertical)
    private float rotationY = 0f; // Current rotation along Y (horizontal)

    public GameObject player; 

    public bool followPLayer = true;
    
    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void cameraFollowPlayer() 
    {
        if(player != null) 
        {
            Vector3 playerPosition = player.transform.position; 
            transform.position = new Vector3(playerPosition.x, playerPosition.y + 2f, playerPosition.z - 5f); 
        }
    }
    void Update()
    {
        if(followPLayer) 
        {
            cameraFollowPlayer(); 
        }
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;

        // Apply rotation to the camera
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -lowerLimit, upperLimit); // Prevent over-rotation
        rotationY += mouseX;

        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);

        // Get keyboard input for movement
        float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        // Move the camera
        transform.Translate(horizontal, 0, vertical);
    }
}
