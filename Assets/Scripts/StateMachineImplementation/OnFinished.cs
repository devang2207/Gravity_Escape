using System.Collections;
using UnityEngine;
[System.Serializable]
public class OnFinished
{
    StateMachine SM_Ref;
    [SerializeField] internal GameObject FinishedParticleEffect;
    [SerializeField] internal AudioSource finishedAudio;
    bool Done = false;
    internal void StartState(StateMachine stateMachine)
    {
        SM_Ref = stateMachine;
    }
    internal IEnumerator FinishedCoroutine()
    {
        //if (SM_Ref.LevelTransition == null) { Debug.LogWarning("LevelTransition is not assigned"); yield break; }
        if (Done) { yield break; }
        Done = true;

        if (!finishedAudio.isPlaying)//audio
        {
            finishedAudio.Play();
        }
        SM_Ref.Create(FinishedParticleEffect,SM_Ref.transform); //particle sys 

       // SM_Ref.LevelTransition.SetBool("DoTransition",true);//level transition anim
        yield return new WaitForSeconds(3.5f);// delay 
        SM_Ref.LevelChange(1);//level change
        yield return new WaitForSeconds(2);
    }
}
