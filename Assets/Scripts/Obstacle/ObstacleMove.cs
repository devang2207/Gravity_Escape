using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    Vector3 StartLocation; // Starting position of the obstacle
    [SerializeField] Vector3 movementVector; // Direction and distance of movement
    [SerializeField][Range(0, 1)] float movementFactor; // Current movement progress
    [SerializeField] float period = 2f; // Time for a complete cycle

    private void Awake()
    {
        
    }
    private void Start()
    {
        StartLocation = transform.position; // Store the initial position
    }

    private void Update()
    {
        if (period <= Mathf.Epsilon) { return; } // Avoid divide by zero errors

        // Calculate the sine wave based movement
        float cycles = Time.time % period / period; // Restart the wave each period
        const float tau = Mathf.PI * 2; // Full circle (2 * pi)
        float rawSinWave = Mathf.Sin(cycles * tau); // Generate sine wave (-1 to +1)

        movementFactor = (rawSinWave + 1f) / 2f; // Normalize to 0 to 1
        Vector3 Offset = movementVector * movementFactor; // Calculate movement
        transform.position = StartLocation + Offset; // Apply the movement
    }

}
