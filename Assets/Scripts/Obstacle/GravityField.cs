using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
    [SerializeField]bool upGravityPull = true;
    [SerializeField] float force = 10;
    private void OnTriggerStay(Collider other)
    {
        //check if the player is there not other random object
        if (other.gameObject.GetComponent<StateMachine>())
        {
            Rigidbody playerRB = other.gameObject.GetComponent<Rigidbody>();
            if (upGravityPull)
            {
                playerRB.AddForce(Vector3.up * force);
            }
            else if (!upGravityPull)
            {
                playerRB.AddForce(Vector3.up * -force);
            }
        }
    }
}
