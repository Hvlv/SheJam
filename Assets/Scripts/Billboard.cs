using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform player;  // Reference to the player or camera
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    [SerializeField] private float maxRotationAngle = 20f; // Maximum degrees to rotate

    void Start()
    {
        // Store the original position and rotation when the script starts
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // FREEZE POSITION - always keep original position
        transform.position = originalPosition;

        // Find player if not assigned
        if (player == null)
        {
            if (Camera.main != null)
                player = Camera.main.transform;
            else
                return;
        }

        // Get direction from player to object (so quad faces the player)
        Vector3 direction = transform.position - player.position;

        // Zero out Y-axis to only rotate horizontally
        direction.y = 0f;

        // If direction is not zero, face the player
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            // Calculate the angle between original and target rotation
            float angle = Quaternion.Angle(originalRotation, targetRotation);
            
            // If the angle is greater than maxRotationAngle, limit it
            if (angle > maxRotationAngle)
            {
                float t = maxRotationAngle / angle;
                targetRotation = Quaternion.Slerp(originalRotation, targetRotation, t);
            }
            
            transform.rotation = targetRotation;
        }
    }
}