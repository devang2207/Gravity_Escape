using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBehaviour : MonoBehaviour
{
    [SerializeField] float minRotationSpeed = 5f; // Minimum rotation speed
    [SerializeField] float maxRotationSpeed = 20f; // Maximum rotation speed
    [SerializeField] float changeInterval = 2f; // Time interval to change rotation
    [SerializeField] LayerMask player; // Layer mask for detecting the rocket
    [SerializeField] float moveSpeed = 3f; // Speed at which the gem moves towards the rocket
    [SerializeField] float attractionRange = 3f; // Range for detecting the rocket

    private float currentRotationSpeed;
    private Vector3 rotationAxis;
    private float changeTimer;

    private void Start()
    {
        // Initialize with a random rotation speed and axis
        currentRotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        rotationAxis = GetRandomAxis();
        changeTimer = changeInterval;
    }

    private void Update()
    {
        if (CheckRocket(out Transform rocketTransform))
        {
            // Move towards the rocket
            MoveTowardsRocket(rocketTransform);
        }

        // Rotate the gem
        transform.Rotate(rotationAxis * currentRotationSpeed * Time.deltaTime);

        // Update timer for changing rotation properties
        changeTimer -= Time.deltaTime;
        if (changeTimer <= 0)
        {
            // Randomize rotation speed and axis
            currentRotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
            rotationAxis = GetRandomAxis();
            changeTimer = changeInterval; // Reset timer
        }
    }

    // Helper function to get a random rotation axis
    private Vector3 GetRandomAxis()
    {
        // Randomize X, Y, and Z components between -1 and 1
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    // Check if the rocket is within range and get its transform
    private bool CheckRocket(out Transform rocketTransform)
    {
        rocketTransform = null;

        // Find all colliders within the attraction range
        Collider[] hits = Physics.OverlapSphere(transform.position, attractionRange, player);

        // If a rocket is found, set the transform and return true
        if (hits.Length > 0)
        {
            rocketTransform = hits[0].transform;
            return true;
        }

        return false;
    }

    // Move the gem towards the rocket
    private void MoveTowardsRocket(Transform rocketTransform)
    {
        // Calculate the direction to the rocket
        Vector3 direction = (rocketTransform.position - transform.position).normalized;

        // Move the gem towards the rocket
        transform.position = Vector3.MoveTowards(transform.position, rocketTransform.position, moveSpeed * Time.deltaTime);
    }
}
