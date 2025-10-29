using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    [Header("Settings")]
    [SerializeField] private float fadeInDuration = 2f;
    [SerializeField] private float targetVolume = 0.5f;

    void Awake()
    {
        // Singleton pattern: only one instance allowed
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        InitializeAudio();
    }

    private void InitializeAudio()
    {
        if (audioSource.clip == null)
        {
            Debug.LogWarning("MusicManager: No AudioClip assigned to AudioSource.");
            return;
        }

        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;
        audioSource.Play();

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, elapsed / fadeInDuration);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    // Optional: public method to fade out when changing scenes
    public IEnumerator FadeOut(float duration = 1f)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}
