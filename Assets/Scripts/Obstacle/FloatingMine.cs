using UnityEngine;

public class FloatingMine : MonoBehaviour
{
    public Transform pointA; // First point (left side)
    public Transform pointB; // Second point (right side)
    public float speed = 2f; // Speed of movement
    public float floatAmplitude = 0.5f; // Amplitude of the floating motion
    public float floatFrequency = 1f; // Frequency of the floating motion

    private Transform currentTarget; // Current target point
    private float originalY; // Original Y position for floating motion

    void Start()
    {
        // Set the initial target point to pointA
        currentTarget = pointA;

        // Store the initial Y position for floating motion
        originalY = transform.position.y;
    }

    void FixedUpdate()
    {
        // Calculate direction to the current target point (either pointA or pointB)
        Vector3 direction = (currentTarget.position - transform.position).normalized;

        // Move horizontally towards the target point
        transform.position += new Vector3(direction.x, 0, direction.z) * speed * Time.fixedDeltaTime;

        // Check if the mine is close to the target
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                             new Vector3(currentTarget.position.x, 0, currentTarget.position.z)) < 0.1f)
        {
            // Switch to the other point (loop back and forth between pointA and pointB)
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }

        // Add a floating effect using a sine wave (floating up and down)
        float floatOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, originalY + floatOffset, transform.position.z);
    }
}
