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

    [Header("Bonfires")]
    public List<BonfireInteractable> bonfires;

    [Header("Ending Triggers")]
    public GameObject goodEndingTrigger;
    public GameObject badEndingTrigger;

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
        UpdateEndingTriggers();
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

    public void AddBonfire(BonfireInteractable bonfire)
    {
        if (!bonfires.Contains(bonfire))
        {
            bonfires.Add(bonfire);
        }
    }

    public void RemoveBonfire(BonfireInteractable bonfire)
    {
        if (bonfires.Contains(bonfire))
        {
            bonfires.Remove(bonfire);
        }
    }

    public BonfireInteractable GetBonfireByID(int bonfireID)
    {
        return bonfires.Find(b => b.bonfireID == bonfireID);
    }

    public void UpdateEndingTriggers()
    {
        if (WorldSaveGameManager.instance.currentCharacterData.hasKilledTheFinalBoss && WorldSaveGameManager.instance.currentCharacterData.endingID == 0)
        {
            goodEndingTrigger.SetActive(true);
            badEndingTrigger.SetActive(true);
        }
        else
        {
            goodEndingTrigger.SetActive(false);
            badEndingTrigger.SetActive(false);
        }
    }
}
