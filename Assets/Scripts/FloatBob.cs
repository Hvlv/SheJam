using UnityEngine;

public class FloatBob : MonoBehaviour
{
	[Header("Bob Settings")]
	[SerializeField] private float amplitude = 0.25f; // Peak height in meters
	[SerializeField] private float frequency = 1.0f;  // Cycles per second
	[SerializeField] private float phaseOffset = 0f;   // Start phase in radians
	[SerializeField] private bool randomizePhase = true; // Desync multiple instances

	// Tracks how much vertical offset was applied last frame so we don't fight other motion
	private float lastOffsetY = 0f;

	void Start()
	{
		if (randomizePhase)
		{
			// Randomize phase so many objects don't bob in unison
			phaseOffset = Random.value * Mathf.PI * 2f;
		}
	}

	void Update()
	{
		// Compute new vertical offset via sine wave
		float t = Time.time;
		float newOffsetY = Mathf.Sin(phaseOffset + t * Mathf.PI * 2f * Mathf.Max(0f, frequency)) * Mathf.Max(0f, amplitude);

		// Apply only the delta so external translations/physics are preserved
		float deltaY = newOffsetY - lastOffsetY;
		if (Mathf.Abs(deltaY) > Mathf.Epsilon)
		{
			transform.position += Vector3.up * deltaY;
		}

		lastOffsetY = newOffsetY;
	}
}


