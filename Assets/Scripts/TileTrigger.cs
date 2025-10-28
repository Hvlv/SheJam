using UnityEngine;

public class TileTrigger : MonoBehaviour
{
    public GameObject interactText; // "Press E to play" UI
    public GameObject puzzleCanvas; // The puzzle UI canvas

    private bool playerInRange = false;
    private SC_FPSController playerController;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            puzzleCanvas.SetActive(true);
            playerController.SetControlsEnabled(false);
            interactText.SetActive(false); // hides "Press E"
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerController = other.GetComponent<SC_FPSController>();
            interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactText.SetActive(false);
        }
    }
}
