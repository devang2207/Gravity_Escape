using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    bool smokeGroundCheck;
 
    void Update()
    {
        if (InputHandler.Instance.Thrust&&smokeGroundCheck)
        {
            _particleSystem.Play();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            smokeGroundCheck = true;
        }
        else
        {
           smokeGroundCheck = false;
        }
    }
}
