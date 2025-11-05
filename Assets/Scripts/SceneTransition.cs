using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;
    public CanvasGroup fadePanel;
    public float fadeSpeed = 1.5f;

    void Awake()
    {
        // Singleton
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

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn()
    {
        // Si pas de fadePanel assigné, cherche dans la scène
        if (fadePanel == null)
        {
            fadePanel = GameObject.FindFirstObjectByType<CanvasGroup>();
            if (fadePanel == null)
            {
                Debug.LogWarning("FadePanel non trouvé, skip fade in.");
                yield break;
            }
        }

        fadePanel.blocksRaycasts = true; // bloque les clics pendant le fade
        fadePanel.alpha = 1;

        while (fadePanel.alpha > 0)
        {
            fadePanel.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        fadePanel.blocksRaycasts = false; // réactive les clics
    }

    IEnumerator FadeOut(string sceneName)
    {
        // Si pas de fadePanel assigné, cherche dans la scène
        if (fadePanel == null)
        {
            fadePanel = GameObject.FindFirstObjectByType<CanvasGroup>();
            if (fadePanel == null)
            {
                Debug.LogWarning("FadePanel non trouvé, charge directement la scène.");
                SceneManager.LoadScene(sceneName);
                yield break;
            }
        }

        fadePanel.blocksRaycasts = true; // bloque les clics
        fadePanel.alpha = 0;

        while (fadePanel.alpha < 1)
        {
            fadePanel.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
