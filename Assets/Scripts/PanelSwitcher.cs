using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    [Header("Assign your panels in order here")]
    public GameObject[] panels;  // e.g., Panel1, Panel2, Panel3, Panel4

    private int currentPanel = 0;

    void Start()
    {
        // Start with the first panel visible, others hidden
        ShowPanel(0);
    }

    void Update()
    {
        // Press Space or Enter to go to the next panel
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            NextPanel();
        }
    }

    public void NextPanel()
    {
        if (currentPanel < panels.Length - 1)
        {
            ShowPanel(currentPanel + 1);
        }
    }

    private void ShowPanel(int index)
    {
        // Disable all panels except the one we want to show
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }

        // Play the audio clip attached to this panel (if it has one)
        AudioSource voice = panels[index].GetComponent<AudioSource>();
        if (voice != null && voice.clip != null)
        {
            voice.Stop(); // stop previous if somehow still playing
            voice.Play();
        }

        currentPanel = index;
    }
}
