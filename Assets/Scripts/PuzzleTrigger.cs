using UnityEngine;
using UnityEngine.UI;  // Needed for UI

public class PuzzleTrigger : MonoBehaviour
{
    public PuzzleManager puzzleManager;    // Reference to PuzzleManager
    public Text interactText;              // UI Text: "Press E to play puzzle"

    private bool playerInside = false;

    void Start()
    {
        // Hide the text at the start
        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            // Show the text
            if (interactText != null)
                interactText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            // Hide the text
            if (interactText != null)
                interactText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            // Open the puzzle panel
            puzzleManager.OpenPuzzle();

            // hide the text while puzzle is open
            if (interactText != null)
                interactText.gameObject.SetActive(false);
        }
    }
}
