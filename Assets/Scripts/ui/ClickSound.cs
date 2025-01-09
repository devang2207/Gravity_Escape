using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSound : MonoBehaviour
{
    public void TransitionSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.transitionClip);
        }
    }
    public void PlayClickSound()
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSFX();
        }
    }
}
