using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneSequenceManager : MonoBehaviour
{
    [Header("Panels (ordered)")]
    public CanvasGroup[] panels;          // assign 4 CanvasGroups, one per illustration panel

    [Header("UI")]
    public TextMeshProUGUI promptText;    // optional "Press Space / Enter" prompt

    [Header("Timing")]
    public float fadeDuration = 0.5f;     // fade in/out duration (seconds)
    public float minDisplayTime = 0.2f;   // minimal time panel stays visible before advancing

    [Header("Next scene")]
    public string nextSceneName = "SampleScene";

    int index = 0;
    bool isTransitioning = false;

    KeyCode[] advanceKeys = new KeyCode[] {
        KeyCode.Space, KeyCode.Return, KeyCode.KeypadEnter, KeyCode.E
    };

    void Start()
    {
        // sanity
        if (panels == null || panels.Length == 0)
        {
            Debug.LogError("SceneSequenceManager: no panels assigned.");
            return;
        }

        // Initialize: hide all panels (alpha = 0, blocksRaycasts = false)
        foreach (var p in panels)
        {
            if (p == null) continue;
            p.alpha = 0f;
            p.interactable = false;
            p.blocksRaycasts = false;
        }

        // show first panel
        StartCoroutine(ShowPanelRoutine(0));
        if (promptText != null) promptText.gameObject.SetActive(true);
    }

    void Update()
    {
        if (isTransitioning) return;
        foreach (var k in advanceKeys)
        {
            if (Input.GetKeyDown(k))
            {
                Next();
                return;
            }
        }
    }

    public void Next()
    {
        if (isTransitioning) return;
        int nextIndex = index + 1;
        if (nextIndex >= panels.Length)
        {
            // finish sequence and load next scene
            StartCoroutine(FinishAndLoad());
        }
        else
        {
            StartCoroutine(TransitionPanels(index, nextIndex));
        }
    }

    IEnumerator ShowPanelRoutine(int panelIndex)
    {
        index = panelIndex;
        CanvasGroup cg = panels[panelIndex];
        isTransitioning = true;
        cg.blocksRaycasts = true;
        cg.interactable = true;

        // fade in
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        cg.alpha = 1f;
        isTransitioning = false;
        // ensure a tiny minimum display time to avoid accidental skips
        yield return new WaitForSecondsRealtime(minDisplayTime);
    }

    IEnumerator TransitionPanels(int fromIndex, int toIndex)
    {
        isTransitioning = true;

        CanvasGroup from = panels[fromIndex];
        CanvasGroup to = panels[toIndex];

        // prepare 'to' panel hidden
        to.alpha = 0f;
        to.interactable = false;
        to.blocksRaycasts = false;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float f = Mathf.Clamp01(t / fadeDuration);
            from.alpha = 1f - f;
            to.alpha = f;
            yield return null;
        }

        from.alpha = 0f;
        from.interactable = false;
        from.blocksRaycasts = false;

        to.alpha = 1f;
        to.interactable = true;
        to.blocksRaycasts = true;

        index = toIndex;
        isTransitioning = false;
        yield return new WaitForSecondsRealtime(minDisplayTime);
    }

    IEnumerator FinishAndLoad()
    {
        isTransitioning = true;

        // Fade out final panel
        CanvasGroup last = panels[index];
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            last.alpha = 1f - Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        last.alpha = 0f;

        // optional small delay
        yield return new WaitForSecondsRealtime(0.12f);

        // load gameplay scene
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("SceneSequenceManager: nextSceneName not set.");
            yield break;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
