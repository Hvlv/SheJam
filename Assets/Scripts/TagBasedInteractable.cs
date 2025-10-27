// using UnityEngine;

// public class TagBasedInteractable : MonoBehaviour
// {
//     [Header("UI Elements")]
//     public GameObject pressEUI;          // "Press E" message
//     public GameObject dialogueUI;        // Dialogue UI to show when E is pressed

//     public string playerTag = "Player";
//     private bool playerInside = false;
//     private bool uiActive = false;
//     private SC_FPSController playerController;

//     private void Start()
//     {
//         if (pressEUI) pressEUI.SetActive(false);
//         if (dialogueUI) dialogueUI.SetActive(false);
//     }

//     private void Update()
//     {
//         if (playerInside)
//         {
//             // When player presses E and UI is not active
//             if (Input.GetKeyDown(KeyCode.E) && !uiActive)
//             {
//                 ShowDialogue();
//             }
//             // When player presses any key while dialogue is active → close dialogue
//             else if (uiActive && Input.anyKeyDown && !Input.GetKeyDown(KeyCode.E))
//             {
//                 CloseDialogue();
//             }
//         }
//     }

//     void ShowDialogue()
//     {
//         if (pressEUI) pressEUI.SetActive(false);
//         if (dialogueUI) dialogueUI.SetActive(true);
//         uiActive = true;

//         if (playerController != null)
//             playerController.canMove = false; // Freeze player movement
//     }

//     void CloseDialogue()
//     {
//         if (dialogueUI) dialogueUI.SetActive(false);
//         uiActive = false;

//         if (playerController != null)
//             playerController.canMove = true; // Unfreeze player movement
//     }

//     void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag(playerTag))
//         {
//             playerInside = true;
            
//             if (playerController == null)
//             {
//                 playerController = other.GetComponent<SC_FPSController>();
//                 if (playerController == null)
//                     playerController = other.GetComponentInParent<SC_FPSController>();
//                 if (playerController == null)
//                     playerController = FindObjectOfType<SC_FPSController>();
//             }
            
//             if (!uiActive && pressEUI) 
//                 pressEUI.SetActive(true);
//         }
//     }

//     void OnTriggerExit(Collider other)
//     {
//         if (other.CompareTag(playerTag))
//         {
//             playerInside = false;
//             if (pressEUI) pressEUI.SetActive(false);
//             if (uiActive)
//             {
//                 // Close dialogue if player exits while it's open
//                 CloseDialogue();
//             }
//         }
//     }
// }

using UnityEngine;

public class TagBasedInteractable : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject pressEUI;

    public string playerTag = "Player";
    private bool playerInside = false;
    private bool uiActive = false;
    private SC_FPSController playerController;

    private void Start()
    {
        if (pressEUI) pressEUI.SetActive(false);
    }

    private void Update()
    {
        if (playerInside)
        {
            if (Input.GetKeyDown(KeyCode.E) && !uiActive)
            {
                QuestionManager questionManager = GetComponent<QuestionManager>();

                if (questionManager != null)
                {
                    questionManager.ShowQuestion(playerController);
                    uiActive = true;

                    if (pressEUI) pressEUI.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("⚠️ No QuestionManager attached to this object!");
                }
            }
        }

        // إذا البانل مفتوح، خلي المؤشر ظاهر
        if (uiActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInside = true;

            if (playerController == null)
            {
                playerController = other.GetComponent<SC_FPSController>();
                if (playerController == null)
                    playerController = other.GetComponentInParent<SC_FPSController>();
                if (playerController == null)
                    playerController = FindFirstObjectByType<SC_FPSController>();
            }

            if (!uiActive && pressEUI)
                pressEUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInside = false;
            uiActive = false;

            if (pressEUI) pressEUI.SetActive(false);

            QuestionManager questionManager = GetComponent<QuestionManager>();
            if (questionManager != null)
            {
                questionManager.ForceCloseQuestion();
            }
        }
    }
}
