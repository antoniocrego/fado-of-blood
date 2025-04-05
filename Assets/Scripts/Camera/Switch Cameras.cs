using UnityEngine;

public class SwitchCameras : MonoBehaviour
{

    [SerializeField] private Camera[] cameras; // Array of cameras

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cameras.Length == 0)
        {
            Debug.LogError("No cameras assigned! Please assign cameras in the inspector.");
            return;
        }
        
        // Disable all cameras except the first one
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].enabled = false;
        }
        cameras[0].enabled = true; // Enable the first camera
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)) // Check if the "C" key is pressed
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                if(cameras[i].enabled){
                    cameras[i].enabled = false; // Disable the current camera
                    int nextCameraIndex = (i + 1) % cameras.Length; // Get the next camera index
                    cameras[nextCameraIndex].enabled = true; // Enable the next camera
                    break; // Exit the loop after switching cameras
                }
            }
        }
    }
}
