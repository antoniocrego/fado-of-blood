using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldObjectManager : MonoBehaviour
{
    public static WorldObjectManager instance;

    [Header("Objects")]
    [SerializeField] List<ObjectSpawner> objectSpawners;
    [SerializeField] List<GameObject> spawnedObjects;

    [Header("Fog Walls")]
    public List<FogWallInteractable> fogWalls;

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

    }

    public void SpawnObject(ObjectSpawner spawner)
    {
        objectSpawners.Add(spawner);
        spawner.AttemptToSpawnObject();
    }

    private void DespawnAllObjects()
    {
        foreach (ObjectSpawner spawner in objectSpawners)
        {
            Destroy(spawner);
        }
        objectSpawners.Clear();
    }

    public void AddFogWall(FogWallInteractable fogWall)
    {
        if (!fogWalls.Contains(fogWall))
        {
            fogWalls.Add(fogWall);
        }
    }
    
    public void RemoveFogWall(FogWallInteractable fogWall)
    {
        if (fogWalls.Contains(fogWall))
        {
            fogWalls.Remove(fogWall);
        }
    }
}
