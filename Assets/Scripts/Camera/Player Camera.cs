using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private GameObject player; // Reference to the player transform
    [SerializeField] private float distance = 5f; // Distance from the player
    [SerializeField] private float height = 2f; // Height above the player
    [SerializeField] private float rotationSpeed = 5f; // Speed of camera rotation
    [SerializeField] private float followSpeed = 10f; // Speed of camera following the player
    [SerializeField] private float minDistance = 2f; // Minimum distance from the player
    [SerializeField] private float maxDistance = 10f; // Maximum distance from the player
    [SerializeField] private float zoomSpeed = 2f; // Speed of zooming in and out

    private float currentXRotation; // Current X rotation of the camera
    private float currentYRotation; // Current Y rotation of the camera

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the player object in the scene
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }

        // Set the initial position of the camera
        Vector3 initialPosition = player.transform.position - transform.forward * distance + Vector3.up * height;
        transform.position = initialPosition;
        transform.LookAt(player.transform.position);

    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Get mouse input for rotation
        currentXRotation += Input.GetAxis("Mouse X") * rotationSpeed;
        currentYRotation -= Input.GetAxis("Mouse Y") * rotationSpeed;

        // Clamp the vertical rotation to prevent flipping
        currentYRotation = Mathf.Clamp(currentYRotation, -30f, 60f);

        // Get keyboard input for zooming
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distance = Mathf.Clamp(distance - scroll, minDistance, maxDistance);    

        Quaternion rotation = Quaternion.Euler(currentYRotation, currentXRotation, 0);
        Vector3 direction = new Vector3(0, 0, -distance);
        Vector3 targetPosition = player.transform.position + rotation * direction;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);


        // Look at the player
        transform.LookAt(player.transform.position);

    }
}
