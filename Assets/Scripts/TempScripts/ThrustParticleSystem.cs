using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustParticleSystem : MonoBehaviour
{
    [SerializeField] int maxParticles = 256;
    ParticleSystem[] thrustParticles;


    private void Awake()
    {
        thrustParticles = GetComponentsInChildren<ParticleSystem>();
    }
    void Update()
    {
        ParticleConditions();
    }

    private void ParticleConditions()
    {
        if (Input.GetKey(KeyCode.W))
        {

            foreach (ParticleSystem ps in thrustParticles)
            {
                ParticleSystem.MainModule mainModule = ps.main;

                mainModule.maxParticles = maxParticles;
            }
        }
        else
        {
            foreach (ParticleSystem ps in thrustParticles)
            {
                ParticleSystem.MainModule mainModule = ps.main;
                mainModule.maxParticles = 0;
            }
        }
    }
}
