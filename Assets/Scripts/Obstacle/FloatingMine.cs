using UnityEngine;

public class FloatingMine : MonoBehaviour
{
    public Transform pointA; // First point
    public Transform pointB; // Second point
    public float speed = 2f; // Speed of movement
    public float floatAmplitude = 0.5f; // Amplitude of the floating motion
    public float floatFrequency = 1f; // Frequency of the floating motion
    public Rigidbody rb; // Rigidbody attached to the mine

    private Transform currentTarget; // Current target point
    private Vector3 initialPosition; // Initial position for the bobbing effect

    void Start()
    {
        // Set the initial target point
        currentTarget = pointA;

        // Get the Rigidbody component
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        // Store the initial position for floating motion
        initialPosition = transform.position;

        // Add drag to make movement feel smooth
        rb.drag = 2f;
        rb.angularDrag = 5f;
    }

    void FixedUpdate()
    {
        // Calculate direction to the current target point
        Vector3 direction = (currentTarget.position - transform.position).normalized;

        // Apply velocity to move towards the target with a smooth damping effect
        Vector3 targetVelocity = direction * speed;
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.fixedDeltaTime * 2f);

        // Check if the mine is close to the target
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            // Switch to the other point
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }

        // Add a floating effect using sine wave
        float floatOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        rb.position = new Vector3(rb.position.x, initialPosition.y + floatOffset, rb.position.z);
    }
}
