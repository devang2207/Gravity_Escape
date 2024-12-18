using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Threading.Tasks;
public class SmallRocketHealthManager : MonoBehaviour
{
    //NAMING Sequence

    //PARAMETERS:-for tuning/mainly in editor  like serializefield
    //CACHE:- eg:- refereneces readability or speed
    //STATE:- private instance variables

    [SerializeField] Controls controls;
    [SerializeField] HealthManager healthManager;
    [SerializeField] Animator buttonAnim;
    [SerializeField] Animator DoorAnim;
    [SerializeField] Animator transitionAnim;
    [SerializeField] AudioSource deadAudioSource;
    [SerializeField] AudioSource finishedAudioSource;

    [SerializeField] ParticleSystem deadParticle;
    [SerializeField] ParticleSystem finishedParticle;

    Light myLight;
    Controls playerControls;


    
    bool isTransitioning = false;                          // is the state transitioning 

    const string start = "Start";                         //strings to use in compare tag
    const string button = "Button";

    private void Awake()
    {
        myLight = GetComponentInChildren<Light>();
        playerControls = GetComponent<Controls>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning) { return; }
        #region Conditions
        switch (collision.gameObject.tag)
        {
            case start:
                //do nothing
                break;
            case button:
                //if not transitioning|takeaway controls|play finished audio|change the level after delay|
                
                    buttonAnim.SetBool("ButtonPressed", true);
                    DoorAnim.SetBool("OpenDoor", true);
                    MainRocketSequel();
                break;
            default:
                //if not transitioning|takeaway controls|play dead audio|reset the level after delay|
                    DeadSequel();
                break;
                
        }
        #endregion
    }

    #region Sequels
    private void MainRocketSequel()
    {
        //Debug.Log("LevelFinishedSequel");
        // Playing the finished audio
        finishedAudioSource.Play();
        // Setting the state isTransitioning to true
        isTransitioning = true;
        // Disabling the movement script
        TakeAwayControls();
        // Instantiating finished particles at the player's position
        Instantiate(finishedParticle, this.transform.position, Quaternion.identity);
        // Giving Main Rocket Controls
        controls.enabled = true;
        healthManager.enabled = true;
        //Invoke("ChangeLevel", 2);
    }
    private void DeadSequel()
    {

        //taking away light
        myLight.enabled = false;
        // Playing the dead audio
        deadAudioSource.Play();
        // Setting the state isTransitioning to true
        isTransitioning = true;
        // Disabling the movement script
        TakeAwayControls();
        // Instantiating dead particles at the player's position
        Instantiate(deadParticle, this.transform.position, Quaternion.identity);
        // Calling ResetLevel after 2 seconds
        transitionAnim.SetBool("DoTransition", true);
        Invoke("ResetLevel", 3.5f);
    }
    #endregion

    #region CommonSequelFunctions
    void TakeAwayControls()                             //used in deadsequel and finished sequel
    {
        playerControls.enabled = false; 
    }
    #endregion

    #region FinishedSequelFunctions
    private void ChangeLevel()                          //used in  finished Sequel
    {
        //change level
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentBuildIndex + 1);
    }
    #endregion

    #region DeadSequelFunctions
    private void ResetLevel()                           //used in dead sequel
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentBuildIndex);
    }
    #endregion



}