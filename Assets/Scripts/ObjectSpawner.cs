using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] GameObject objectGameObject;
    [SerializeField] GameObject instantiatedGameObject;

    private void Awake()
    {

    }
    private void Start()
    {
        WorldObjectManager.instance.SpawnObject(this);
        gameObject.SetActive(false);
    }

    public void AttemptToSpawnObject()
    {
        if (objectGameObject != null)
        {
            instantiatedGameObject = Instantiate(objectGameObject);
            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
        }
    }
}
