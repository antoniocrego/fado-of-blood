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
    public VideoClip goodEndingCutscene;
    public VideoClip badEndingCutscene;

    private bool isPlaying = false;
    private Action onCutsceneComplete; // callback

    public enum CutsceneType
    {
        Intro,
        GoodEnding,
        BadEnding
    }

    public PlayerInput playerInput;
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

        playerMovement = playerInput.actions.FindActionMap("PlayerMovement");
        playerActions = playerInput.actions.FindActionMap("PlayerActions");
        cameraMovement = playerInput.actions.FindActionMap("CameraMovement");
        UI = playerInput.actions.FindActionMap("UI");
        cutsceneMap = playerInput.actions.FindActionMap("Cutscene");
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
    }

    public void PlayCutscene(CutsceneType type, Action onComplete = null)
    {
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
        if (!isPlaying) return;
        videoPlayer.Stop();
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