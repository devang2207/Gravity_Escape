using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class OnDead 
{
    [Header("SpawnOnDeath")]
    [SerializeField] GameObject DestroyedRocket;
    [SerializeField] GameObject DeadParticle;

    //reference to state machine
    StateMachine SM_Ref;
    bool Dead = false;

    [Header("Audio")]
    [SerializeField] AudioSource DeadAudio;

    internal void StartState(StateMachine stateMachine)
    {
        SM_Ref = stateMachine;
    }
    internal IEnumerator DeadCoroutine()
    {
        //stop particle sys
        if (Dead) { yield break;}
        Dead = true;
        SM_Ref.midParticleSys.Stop();                           //stopping particle systems
        SM_Ref.rightParticleSys.Stop();
        SM_Ref.leftParticleSys.Stop();

        Transform firstChild = SM_Ref.transform.GetChild(0);     
        firstChild.gameObject.SetActive(false);                 //disabling PlayerBody

        SM_Ref.Create(DestroyedRocket,SM_Ref.transform);        //SPAWN Destroyed rocket
        SM_Ref.Create(DeadParticle,SM_Ref.transform);           //destroyed particle effect

        if (AudioManager.Instance != null)
        {
            if (!AudioManager.Instance.sfxSource.isPlaying)//audio
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.deathClip);
            }
        }

        yield return new WaitForSeconds(1);

        //play cloud animation 
        if(SM_Ref.LevelTransition != null)
        {
            SM_Ref.LevelTransition.SetBool("DoTransition",true);
        }else { Debug.LogWarning("Level Transition is not assigned"); }

        
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return null; 
    }
}
