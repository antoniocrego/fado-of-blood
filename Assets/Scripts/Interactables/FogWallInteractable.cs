using UnityEngine;

public class FogWallInteractable : MonoBehaviour
{
    [Header("Fog")]
    [SerializeField] GameObject[] fogGameObjects;

    [Header("ID")]
    public int fogWallID;

    [Header("Active")]
    public bool isActive = false;

    private void Awake()
    {
        WorldObjectManager.instance.AddFogWall(this);
    }

    private void Update()
    {
        foreach (GameObject fog in fogGameObjects)
        {
            fog.SetActive(isActive);
        }
    }
}
