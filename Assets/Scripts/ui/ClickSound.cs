using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSound : MonoBehaviour
{
    public void PlayClickSound()
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSFX();
        }
    }
}
