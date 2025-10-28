using UnityEngine;
using UnityEngine.UI;


public class TilePuzzle : MonoBehaviour
{
    public Button[] tiles; // Assign your 8 tile buttons in inspector
    public GameObject solvedText;
    public Button closeButton;
    public SC_FPSController playerController;
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip solvedSound;

    private Quaternion[] correctRotations;

    void Start()
    {
        // Store the correct rotation for each tile
        correctRotations = new Quaternion[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
        {
            correctRotations[i] = tiles[i].transform.rotation;
            // Randomize rotation by 0, 90, 180, or 270 degrees
            int randomAngle = 90 * Random.Range(0, 4);
            tiles[i].transform.rotation = Quaternion.Euler(0, 0, randomAngle);

            int index = i; // Local copy for the listener
            tiles[i].onClick.AddListener(() => RotateTile(index));
        }

        solvedText.SetActive(false);
        closeButton.onClick.AddListener(ClosePuzzle);
    }

    void RotateTile(int index)
    {
        tiles[index].transform.Rotate(0, 0, 90);
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound);
        CheckSolution();
    }


    void CheckSolution()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (Quaternion.Angle(tiles[i].transform.rotation, correctRotations[i]) > 1f)
                return;
        }
        solvedText.SetActive(true);
        if (audioSource && solvedSound) audioSource.PlayOneShot(solvedSound);
    }


    void ClosePuzzle()
    {
        gameObject.SetActive(false);
        playerController.SetControlsEnabled(true);
        solvedText.SetActive(false);
    }
}
