using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using  UnityEngine.InputSystem;
using System;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;
    public VideoPlayer videoPlayer;
    public RawImage videoImage;
    public GameObject cutsceneUI;

    public VideoClip introCutscene;
    [SerializeField] string introAudioFMOD = "event:/Cutscenes/Intro";
    public VideoClip goodEndingCutscene;
    [SerializeField] string goodEndingAudioFMOD = "event:/Cutscenes/Good_Ending";
    public VideoClip badEndingCutscene;
    [SerializeField] string badEndingAudioFMOD = "event:/Cutscenes/Bad_Ending";
    [Header("Audio")]
    private FMOD.Studio.EventInstance audioInstance;

    private bool isPlaying = false;
    private Action onCutsceneComplete; // callback

    public enum CutsceneType
    {
        Intro,
        GoodEnding,
        BadEnding
    }

    public InputActionAsset playerInput;
    InputActionMap playerMovement;
    InputActionMap playerActions;
    InputActionMap cameraMovement;
    InputActionMap UI;
    InputActionMap cutsceneMap;
    private InputAction skipAction;

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

        playerMovement = playerInput.FindActionMap("Player Movement");
        playerActions = playerInput.FindActionMap("Player Actions");
        cameraMovement = playerInput.FindActionMap("Camera Movement");
        UI = playerInput.FindActionMap("UI");
        cutsceneMap = playerInput.FindActionMap("Cutscene");
        skipAction = cutsceneMap.FindAction("Skip");

        skipAction.performed += ctx => SkipCutscene();
        
        skipAction.Disable();
    }

    void OnDisable()
    {
        skipAction.Disable();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.errorReceived += (source, message) => Debug.LogError("VideoPlayer Error: " + message);
    }

    public void PlayCutscene(CutsceneType type, Action onComplete = null)
    {
        if (isPlaying) return;
        playerMovement.Disable();
        playerActions.Disable();
        cameraMovement.Disable();
        UI.Disable();
        skipAction.Enable();

        WorldSoundtrackManager.instance.StopTrack();
        onCutsceneComplete = onComplete;

        switch(type)
        {
            case CutsceneType.Intro:
                videoPlayer.clip = introCutscene;
                audioInstance = FMODUnity.RuntimeManager.CreateInstance(introAudioFMOD);
                audioInstance.start();
                break;
            case CutsceneType.GoodEnding:
                videoPlayer.clip = goodEndingCutscene;
                audioInstance = FMODUnity.RuntimeManager.CreateInstance(goodEndingAudioFMOD);
                audioInstance.start();
                break;
            case CutsceneType.BadEnding:
                videoPlayer.clip = badEndingCutscene;
                audioInstance = FMODUnity.RuntimeManager.CreateInstance(badEndingAudioFMOD);
                audioInstance.start();
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
        if (!isPlaying) return;
        videoPlayer.Stop();
        audioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        audioInstance.release();

        EndCutscene();
    }

    void EndCutscene()
    {
        playerMovement.Enable();
        playerActions.Enable();
        cameraMovement.Enable();
        UI.Enable();
        skipAction.Disable();

        isPlaying = false;
        cutsceneUI.SetActive(false);

        onCutsceneComplete?.Invoke();
    }
}