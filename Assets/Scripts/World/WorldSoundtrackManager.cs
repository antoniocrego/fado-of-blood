using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class WorldSoundtrackManager : MonoBehaviour
{
    public static WorldSoundtrackManager instance;
    [SerializeField] private string defaultEventPath = "event:/Music/Fire Boss Music"; // main menu ost

    private EventInstance currentTrack;
    private string currentEventPath = "";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }   
    }

    public void PlayTrack(string eventPath, bool restartIfPlaying = false)
    {
        if (currentEventPath == eventPath && !restartIfPlaying)
            return;

        StopTrack();

        currentTrack = RuntimeManager.CreateInstance(eventPath);
        currentTrack.start();
        currentTrack.release(); // Optional: release handle so FMOD can manage memory
        currentEventPath = eventPath;
    }

    public void StopTrack()
    {
        if (currentTrack.isValid())
        {
            currentTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentTrack.release();
        }

        currentEventPath = "";
    }

    public void PauseTrack()
    {
        if (currentTrack.isValid())
        {
            currentTrack.setPaused(true);
        }
    }

    public void ResumeTrack()
    {
        if (currentTrack.isValid())
        {
            currentTrack.setPaused(false);
        }
    }

    public void SetVolume(float volume) // 0.0 to 1.0
    {
        if (currentTrack.isValid())
        {
            currentTrack.setVolume(Mathf.Clamp01(volume));
        }
    }

    public bool IsPlaying()
    {
        if (currentTrack.isValid())
        {
            PLAYBACK_STATE state;
            currentTrack.getPlaybackState(out state);
            return state == PLAYBACK_STATE.PLAYING;
        }
        return false;
    }
}
