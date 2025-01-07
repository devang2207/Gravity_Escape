using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class OnFinished
{
    StateMachine SM_Ref;
    [SerializeField] internal GameObject FinishedParticleEffect;
    [SerializeField] internal AudioSource finishedAudio;

    Rigidbody PlayerRb;
    bool Done = false;
    internal void StartState(StateMachine stateMachine)
    {
        SM_Ref = stateMachine;
        PlayerRb = SM_Ref.GetComponent<Rigidbody>();
    }
    internal IEnumerator FinishedCoroutine()
    {
        //set the unlocked level index
        if (Done) { yield break; }
        Done = true;

        PlayerRb.isKinematic = true;
        SM_Ref.midParticleSys.Stop();                           //stopping particle systems
        SM_Ref.rightParticleSys.Stop();
        SM_Ref.leftParticleSys.Stop();

        if (SM_Ref.timerManager == null) { Debug.LogWarning("timer manager is null");yield break; }
        SM_Ref.timeRequiredTMP.text = "Time:- " + SM_Ref.timerManager.GetFormattedTime();

        // set the gem value for current level in player prefs
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (PlayerPrefs.GetInt($"Level_{currentLevelIndex}_GemCount", 0)<SM_Ref.gemCount)
        {
            PlayerPrefs.SetInt($"Level_{currentLevelIndex}_GemCount", SM_Ref.gemCount);
        }

        //play finished animation and audio
        if (!finishedAudio.isPlaying)//audio
        {
            finishedAudio.Play();
        }
        SM_Ref.Create(FinishedParticleEffect,SM_Ref.transform); //particle sys 



        //Finished Game Object set active true 
        if (SM_Ref.LevelTransition != null)
        {
            SM_Ref.LevelTransition.SetBool("DoTransition", true);
        }
        else { Debug.LogWarning("Level Transition is not assigned"); }

        yield return new WaitForSeconds(3.5f);// delay 
        SM_Ref.levelFinishGO.SetActive(true);


        if(SM_Ref.starAnimator == null)
        { Debug.LogWarning("gem count TMP is null"); yield break;}
        switch (SM_Ref.gemCount)
        {
            case 0:
                break;
                case 1:
                SM_Ref.starAnimator.SetTrigger("One");
                break;
            case 2:
                SM_Ref.starAnimator.SetTrigger("Two");
                break;
            case 3:
                SM_Ref.starAnimator.SetTrigger("Three");
                break;
                default:
                SM_Ref.starAnimator.SetTrigger("Three");
                break;
        }

        //Animate Gem count value
        if (SM_Ref.levelFinishedGemCountTMP == null)
        {
            Debug.LogWarning("Gem count TMP is null");
            yield break;
        }

        int currentCount = 0;
        float duration = 1.5f; // Total time for the animation
        float elapsedTime = 0;

        while (currentCount < SM_Ref.gemCount)
        {
            elapsedTime += Time.deltaTime;
            currentCount = Mathf.FloorToInt(Mathf.Lerp(0, SM_Ref.gemCount, elapsedTime / duration));
            SM_Ref.levelFinishedGemCountTMP.text = "Gems: " + currentCount;
            yield return null; // Wait for the next frame
        }

        // Ensure the final count is displayed at the end
        SM_Ref.levelFinishedGemCountTMP.text = "Gems: " + SM_Ref.gemCount;

    }
}
