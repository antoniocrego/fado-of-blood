using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;
    public VideoPlayer videoPlayer;
    public RawImage videoImage;
    public GameObject cutsceneUI;

    public VideoClip introCutscene;
    public VideoClip goodEndingCutscene;
    public VideoClip badEndingCutscene;

    private bool isPlaying = false;
    private Action onCutsceneComplete; // callback
    // public AudioListener audioListener;

    public enum CutsceneType
    {
        Intro,
        GoodEnding,
        BadEnding
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

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        if (isPlaying && Input.GetKeyDown(KeyCode.Escape))
        {
            SkipCutscene();
        }
    }

    public void PlayCutscene(CutsceneType type, Action onComplete = null)
    {
        WorldSoundtrackManager.instance.StopTrack();
        // audioListener.enabled = true;
        onCutsceneComplete = onComplete;

        switch(type)
        {
            case CutsceneType.Intro:
                videoPlayer.clip = introCutscene;
                break;
            case CutsceneType.GoodEnding:
                videoPlayer.clip = goodEndingCutscene;
                break;
            case CutsceneType.BadEnding:
                videoPlayer.clip = badEndingCutscene;
                break;
        }
        cutsceneUI.SetActive(true);
        videoPlayer.Play();
        isPlaying = true;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        EndCutscene();
    }
    
    void SkipCutscene()
    {
        videoPlayer.Stop();
        EndCutscene();
    }

    void EndCutscene()
    {
        isPlaying = false;
        cutsceneUI.SetActive(false);
        // audioListener.enabled = false;

        onCutsceneComplete?.Invoke();
    }
}