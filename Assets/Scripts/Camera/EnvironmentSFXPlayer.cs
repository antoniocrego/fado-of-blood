using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

public class EnvironmentSFXPlayer : MonoBehaviour
{
    [Header("Environment SFX Events Per Area")]
    [SerializeField] List<string> environmentSFXEventsPerArea;
    private EventInstance _instance;
    private string _currentEventPath;

    void Start()
    {
        SetAreaEvent(WorldSaveGameManager.instance.currentCharacterData.currentAreaID);
    }

    void Update()
    {
        int areaID = WorldSaveGameManager.instance.currentCharacterData.currentAreaID;
        string newPath = environmentSFXEventsPerArea[areaID];
        if (newPath != _currentEventPath)
            SetAreaEvent(areaID);
    }

    void SetAreaEvent(int areaID)
    {
        // stop & release old
        if (_instance.isValid())
        {
            _instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _instance.release();
        }

        _currentEventPath = environmentSFXEventsPerArea[areaID];
        _instance = RuntimeManager.CreateInstance(_currentEventPath);
        RuntimeManager.AttachInstanceToGameObject(_instance, gameObject);
        _instance.start();
    }
}
