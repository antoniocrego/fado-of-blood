using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager instance;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] CanvasGroup canvasGroup;
    private Coroutine fadeLoadingScreenCoroutine;

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

        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        DeactivateLoadingScreen();
    }

    public void ActivateLoadingScreen()
    {
        if (loadingScreen.activeSelf) return;
        loadingScreen.SetActive(true);
        canvasGroup.alpha = 1f;
        Debug.Log("Activating loading screen");
        WorldSoundtrackManager.instance.StopTrack();
    }

    public void DeactivateLoadingScreen(float delay = 1)
    {
        if (!loadingScreen.activeSelf) return;

        if (fadeLoadingScreenCoroutine != null)
        {
            return;
        }

        fadeLoadingScreenCoroutine = StartCoroutine(FadeOutLoadingScreen(1, delay));
    }

    private IEnumerator FadeOutLoadingScreen(float duration, float delay)
    {
        while (WorldAIManager.instance.isPerformingLoadingOperation)
        {
            yield return null;
        }

        loadingScreen.SetActive(true);

        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;
        canvasGroup.alpha = 1;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            yield return null;
        }

        loadingScreen.SetActive(false);
        canvasGroup.alpha = 0f;
        fadeLoadingScreenCoroutine = null;
        yield return null;
    }
}
