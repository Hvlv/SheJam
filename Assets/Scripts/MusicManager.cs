using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    void Awake()
    {
        // Ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep it between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }
}
