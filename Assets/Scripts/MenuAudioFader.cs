using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MenuAudioFader : MonoBehaviour
{
    [SerializeField] private float fadeOutDuration = 1.5f;
    private AudioSource menuAudio;

    void Start()
    {
        menuAudio = GetComponent<AudioSource>();
        menuAudio.Play();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Fade out audio only if we’re leaving the menu scene
        if (scene.buildIndex != SceneManager.GetActiveScene().buildIndex)
        {
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float startVolume = menuAudio.volume;
        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            menuAudio.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeOutDuration);
            yield return null;
        }

        menuAudio.Stop();
        Destroy(gameObject); // Clean up to avoid leftovers
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
