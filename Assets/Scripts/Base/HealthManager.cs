using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class HealthManager : MonoBehaviour
{
    //NAMING Sequence

    //PARAMETERS:-for tuning/mainly in editor  like serializefield
    //CACHE:- eg:- refereneces readability or speed
    //STATE:- private instance variables

    [SerializeField]AudioSource deadAudioSource;
    [SerializeField]AudioSource finishedAudioSource;
    [SerializeField] AudioSource TransitionAudioSource;

    [SerializeField] ParticleSystem deadParticle;
    [SerializeField] ParticleSystem finishedParticle;

    [SerializeField] Animator LevelTransitionAnim;
    [SerializeField] string TransitionName = "DoTransition";
    [SerializeField] GameObject DestroyableRocket;

    [SerializeField] float fuelCapacity = 100;
    [SerializeField] float currentFuel;
    [SerializeField] float burningRate = 10;
    [SerializeField] Image fuelIndicator;
    [SerializeField] Image SafeToLand;
    [SerializeField] Image DangerToLand;

    InputHandler inputHandler;
    Light myLight;
    Controls playerControls;
    Rigidbody rocketRb;
    float yVelocity;
    [SerializeField]float allowedVelocity=5.0f;
    float xVelocity;


    //bool isFuelAvailable = true;
    bool isTransitioning = false;                          // is the state transitioning 

    const string start = "Start";                         //strings to use in compare tag
    const string finished = "Finish";  

    
    
    private void Awake()
    {
        rocketRb = GetComponent<Rigidbody>();
        myLight = GetComponentInChildren<Light>();
        playerControls = GetComponent<Controls>();
    }
    private void Start()
    {
        inputHandler = InputHandler.Instance;
        currentFuel = fuelCapacity;
        SafeToLand.gameObject.SetActive(true);
        DangerToLand.gameObject.SetActive(true);
    }

    private void Update()
    {
        yVelocity = rocketRb.velocity.y;
        xVelocity = rocketRb.velocity.x;

        FuelCalculator();
        SafeOrNotIndicatingImages();
    }

    //temp
    private void SafeOrNotIndicatingImages()
    {
        if (CannotLand())
        {
            SafeToLand.enabled = false;
            SafeToLand.gameObject.SetActive(false);
            DangerToLand.enabled = true;
            DangerToLand.gameObject.SetActive(true);
        }
        else
        {
            SafeToLand.enabled = true;
            SafeToLand.gameObject.SetActive(true);
            DangerToLand.enabled = false;
            DangerToLand.gameObject.SetActive(false);
        }
    }

    private void FuelCalculator()
    {
        if (inputHandler.Thrust)
        {
            currentFuel -= burningRate * Time.deltaTime;
            if (currentFuel < 0)
            {
                //isTransitioning = true;
                playerControls.enabled = false;
                Invoke("ResetLevel",5);

                //DeadSequel();
            }
        }
        if (inputHandler.RotateLeft || inputHandler.RotateRight)
        {
            currentFuel -= burningRate/2 *Time.deltaTime;
        }
        fuelIndicator.fillAmount = currentFuel / fuelCapacity;      //set the fill amount to current fuel available.
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning) { return; }//if its transitioning dont go to conditions. 
        #region Conditions
        switch (collision.gameObject.tag)
        {
            case start:
                //do nothing
                if (CannotLand()) // check velocity
                {
                    //do nothing
                    return;
                }
                else
                {

                }
                break;
            case finished:
                //if not transitioning|takeaway controls|play finished audio|change the level after delay|
                if (!CannotLand()) // check velocity
                {
                    //Debug.Log("xVel" + xVelocity);
                    //Debug.Log("yVel" + yVelocity);
                    LevelFinishedSequel();
                }
                else
                {
                    //Debug.Log("xVel" + xVelocity);
                    //Debug.Log("yVel" + yVelocity);
                    DeadSequel();
                }

                break;
            default:
                //if not transitioning|takeaway controls|play dead audio|reset the level after delay|
                DeadSequel();
                break;

        }
        #endregion
    }

    private bool CannotLand()
    {
        return (xVelocity > allowedVelocity || xVelocity < -allowedVelocity) || (yVelocity > allowedVelocity || yVelocity < -allowedVelocity);
    }

    #region Sequels
    private void LevelFinishedSequel()
    {
        TransitionAudioSource.Play();
        Debug.Log("LevelFinishedSequel");
        // Playing the finished audio
        finishedAudioSource.Play();
        // Setting the state isTransitioning to true
        isTransitioning = true;
        // Disabling the movement script
        TakeAwayControls();
        // Instantiating finished particles at the player's position
        Instantiate(finishedParticle, this.transform.position, Quaternion.identity);
        //Level Transition Animation
        Invoke(nameof(StartTransition),1);
        // Calling ChangeLevel after 2 seconds
        //Invoke("ChangeLevel", 5);
        //unlock new level in player prefs
        UnlockNewLevel();
    }

    private void StartTransition()
    {
        LevelTransitionAnim.gameObject.SetActive(true);
        LevelTransitionAnim.SetBool(TransitionName, true);
        Invoke(nameof(ChangeLevel),3.5f);
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
        DeathEffect();
        // Calling ResetLevel after 2 seconds
        LevelTransitionAnim.SetBool(TransitionName, true);
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
        SceneManager.LoadScene(currentBuildIndex+ 1);
    }
    private void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("UnlockedLevel", currentScene + 1);
            PlayerPrefs.Save();
        }

    }
    #endregion

    #region DeadSequelFunctions
    private void ResetLevel()                           //used in dead sequel
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentBuildIndex);
    }
    private void DeathEffect()
    {
        // Instantiate death particle effect at the current position of the rocket
        Instantiate(deadParticle, transform.position, Quaternion.identity);

        // Get the position and rotation of the rocket
        Vector3 rocketPosition = transform.position;
        Quaternion rocketRotation = transform.rotation;

        // Instantiate the DestroyableRocket at the same position and rotation as the rocket
        Instantiate(DestroyableRocket, rocketPosition, rocketRotation);

        // Disable the rocket gameObject
        gameObject.SetActive(false);
    }

    #endregion


    
}