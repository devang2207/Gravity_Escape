using UnityEngine;

public class FloatingMine : MonoBehaviour
{
    public Transform pointA; // First point (left side)
    public Transform pointB; // Second point (right side)
    public float speed = 2f; // Speed of movement
    public float floatAmplitude = 0.5f; // Amplitude of the floating motion
    public float floatFrequency = 1f; // Frequency of the floating motion

    private Transform currentTarget; // Current target point
    private Vector3 initialPosition; // Initial position for the bobbing effect

    void Start()
    {
        // Set the initial target point to pointA
        currentTarget = pointA;

        // Store the initial position for floating motion
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Calculate direction to the current target point (either pointA or pointB)
        Vector3 direction = (currentTarget.position - transform.position).normalized;

        // Move towards the target point
        transform.position += direction * speed * Time.fixedDeltaTime;

        // Check if the mine is close to the target
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            // Switch to the other point (loop back and forth between pointA and pointB)
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }

        // Add a floating effect using sine wave (floating up and down)
        float floatOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, initialPosition.y + floatOffset, transform.position.z);
    }
}
