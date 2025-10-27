using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject puzzlePanel;
    public Text hintText;
    public Button[] switchButtons;
    public Button closeButton;

    [Header("Visuals - 3D or UI neon lights")]
    public GameObject[] signPieces;

    [Header("Puzzle Settings")]
    public int[] correctSequence;
    [TextArea] public string hint = "Neon glows; the answer hides in light.";

    [Header("Audio")]
    public AudioClip buttonClickSound;
    public AudioClip wrongSequenceSound;
    public AudioClip correctSequenceSound;
    public AudioSource audioSource;

    private List<int> currentSequence = new List<int>();
    private bool solved = false;
    private SC_FPSController playerController;

    void Start()
    {
        // Wire button clicks
        for (int i = 0; i < switchButtons.Length; i++)
        {
            int idx = i;
            if (switchButtons[i] != null)
            {
                switchButtons[i].onClick.RemoveAllListeners();
                switchButtons[i].onClick.AddListener(() => RegisterSwitch(idx));
            }
        }

        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePuzzle);

        // Deactivate all neon lights at start
        foreach (var sp in signPieces)
            if (sp != null) sp.SetActive(false);

        if (hintText != null) hintText.gameObject.SetActive(false);

        // Find player
        playerController = FindObjectOfType<SC_FPSController>();
    }

    void Update()
    {
        if (!puzzlePanel.activeSelf || solved) return;

        // Optional: keyboard input
        if (Input.GetKeyDown(KeyCode.Alpha1)) RegisterSwitch(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) RegisterSwitch(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) RegisterSwitch(2);
    }

    public void OpenPuzzle()
    {
        puzzlePanel.SetActive(true);

        if (!solved && playerController != null)
            playerController.SetControlsEnabled(false);
    }

    public void ClosePuzzle()
    {
        puzzlePanel.SetActive(false);

        if (playerController != null)
            playerController.SetControlsEnabled(true);
    }

    public void RegisterSwitch(int index)
    {
        if (solved) return;

        // Play button click sound
        if (audioSource != null && buttonClickSound != null)
            audioSource.PlayOneShot(buttonClickSound);

        // Add to current input sequence
        currentSequence.Add(index);

        // Activate neon sign corresponding to button pressed
        if (index >= 0 && index < signPieces.Length && signPieces[index] != null)
            signPieces[index].SetActive(true);

        int pos = currentSequence.Count - 1;

        // Check if wrong
        if (pos >= correctSequence.Length || currentSequence[pos] != correctSequence[pos])
        {
            // Play wrong sequence sound
            if (audioSource != null && wrongSequenceSound != null)
                audioSource.PlayOneShot(wrongSequenceSound);

            StartCoroutine(WrongSequenceReset());
            return;
        }

        // Check if solved
        if (currentSequence.Count == correctSequence.Length)
        {
            // Play correct sequence sound
            if (audioSource != null && correctSequenceSound != null)
                audioSource.PlayOneShot(correctSequenceSound);

            StartCoroutine(SolveRoutine());
        }
    }

    IEnumerator WrongSequenceReset()
    {
        yield return new WaitForSecondsRealtime(0.3f);

        currentSequence.Clear();
        foreach (var sp in signPieces)
            if (sp != null) sp.SetActive(false);
    }

    IEnumerator SolveRoutine()
    {
        solved = true;

        for (int i = 0; i < signPieces.Length; i++)
        {
            if (signPieces[i] != null)
                signPieces[i].SetActive(true);
            yield return new WaitForSecondsRealtime(0.15f);
        }

        if (hintText != null)
        {
            hintText.text = hint;
            hintText.gameObject.SetActive(true);
        }
    }
}
