using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketAudioBehaviour : MonoBehaviour
{
     AudioSource thrustAudio;
    [SerializeField] AudioClip myaudio;
    // Start is called before the first frame update
    void Start()
    {
        thrustAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        DefaultAudio();
        if (Input.GetKey(KeyCode.Space))
        {
            PlayAudio(myaudio);
        }
    }

    private void DefaultAudio()
    {
        if (Input.GetKey(KeyCode.W))
        {
            // audio.PlayOneShot(myaudio);
            if (!thrustAudio.isPlaying)
            {
                thrustAudio.Play();
            }

        }
        else
        {
            thrustAudio.Pause();
        }
    }
    void PlayAudio(AudioClip audio)
    {
        thrustAudio.PlayOneShot(audio);
    }
}
