using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketAudioBehaviour : MonoBehaviour
{
    private AudioSource thrustAudioSource; 
    private AudioSource rotationAudioSource; 

    private void Start()
    {
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        if (audioSources.Length >= 2)
        {
            thrustAudioSource = audioSources[0];
            rotationAudioSource = audioSources[1];

            // Ensure AudioSources are set to loop
            thrustAudioSource.loop = true;
            rotationAudioSource.loop = true;
        }
        else
        {
            Debug.LogError("RocketAudioBehaviour: Not enough AudioSource components found in children.");
        }
    }

    private void Update()
    {
        if (thrustAudioSource != null && rotationAudioSource != null)
        {
            HandleThrustAudio();
            HandleRotationAudio();
        }
        else { Debug.LogWarning("thrust audio or rotation audio source are missing"); }
    }

    private void HandleThrustAudio()
    {
        if (InputHandler.Instance.Thrust)
        {
            if (!thrustAudioSource.isPlaying)
            {
                thrustAudioSource.Play();
            }
        }
        else
        {
            thrustAudioSource.Pause();
        }
    }

    private void HandleRotationAudio()
    {
        if (InputHandler.Instance.RotateLeft || InputHandler.Instance.RotateRight)
        {
            if (!rotationAudioSource.isPlaying)
            {
                rotationAudioSource.Play();
            }
        }
        else
        {
            rotationAudioSource.Pause();
        }
    }
}
