using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
    [SerializeField] bool upGravityPull = true;
    [SerializeField] float targetVelocity = 5f; // Target Y velocity
    [SerializeField] float lerpSpeed = 1f; // Speed of lerping

    private void OnTriggerStay(Collider other)
    {
        // Check if the player is present (by verifying the presence of StateMachine)
        if (other.gameObject.GetComponent<StateMachine>())
        {
            Rigidbody playerRB = other.gameObject.GetComponent<Rigidbody>();

            // Get the current velocity
            Vector3 currentVelocity = playerRB.velocity;

            // Set the target Y velocity based on the gravity direction
            float desiredYVelocity = upGravityPull ? targetVelocity : -targetVelocity;

            // Smoothly lerp the Y velocity to the desired value
            float newYVelocity = Mathf.Lerp(currentVelocity.y, desiredYVelocity, lerpSpeed * Time.deltaTime);

            // Update the Rigidbody velocity
            playerRB.velocity = new Vector3(currentVelocity.x, newYVelocity, currentVelocity.z);
        }
    }
}
