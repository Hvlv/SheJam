using UnityEngine;

public class PanelVoiceController : MonoBehaviour
{
    private AudioSource voiceOver;

    void Awake()
    {
        voiceOver = GetComponent<AudioSource>();
    }

    void OnEnable() // This runs when the panel is shown
    {
        if (voiceOver != null)
        {
            voiceOver.Play();  // Play when panel appears
        }
    }

    void OnDisable() // Optional: stop it when panel hides
    {
        if (voiceOver != null)
        {
            voiceOver.Stop();
        }
    }
}
