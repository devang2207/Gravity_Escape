using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StateMachine : MonoBehaviour
{
    
    #region CommonVariables_Flying&OnGround
    [Header("Flying and OnGround")]
    [SerializeField]internal int RocketThrust = 700;
    [SerializeField]internal LayerMask Safe;
    [SerializeField][Range(0,1)]internal float groundCheckSize = 0.75f;

    [Header("ParticleSys")]
    [SerializeField] internal ParticleSystem midParticleSys;
    [SerializeField] internal ParticleSystem rightParticleSys;
    [SerializeField] internal ParticleSystem leftParticleSys;
    [SerializeField] ParticleSystem GemCollectionParticle;
    internal Transform GroundCheckPos;


    #endregion

    #region  CommonVariables_OnDead&OnFinished
    [Header("Dead || Finished")]
    [SerializeField] bool checkVelocity;
    [SerializeField] internal float allowedVelocityY = 5;
    [SerializeField] internal float allowedVelocityX = 5;

    [SerializeField] internal GameObject levelFinishGO;
    [SerializeField] internal Animator starAnimator;
    [SerializeField] internal TMPro.TextMeshProUGUI levelFinishedGemCountTMP;
    [SerializeField] internal TMPro.TextMeshProUGUI timeRequiredTMP;
    internal TimeManager timerManager;

    //[SerializeField] internal GameObject gameOverGO;

    [SerializeField] internal Animator LevelTransition;
    #endregion


    #region State Scripts
    [Header("State Scripts")]
    [SerializeField] internal Flying flying = new Flying();
    [SerializeField] OnGround onGround = new OnGround();
    [SerializeField] OnFinished onFinished = new OnFinished();
    [SerializeField] OnDead onDead = new OnDead();
    [SerializeField] FuelCalculator fuelCalculator = new FuelCalculator();
    #endregion

    [Header("Collision Handling")]
    [SerializeField] float requiredCollisionTime = 2f; // Time the player must stay collided to change the state
    private bool isCollidingWithFinish = false;
    private float collisionTimer = 0f;
    internal bool canChangeState = true;

    //current level gem count
    public int gemCount;
    //current state
    internal State currentState;
    public enum State
    {
        Flying,OnGround,OnFinished,OnDead
    }
    void Start()
    {
        timerManager = FindObjectOfType<TimeManager>();
        if (timerManager == null) { Debug.LogWarning("time manager is null"); }

        if (levelFinishGO != null )
        {
            levelFinishGO.SetActive(false);
        }
        else { Debug.LogWarning("level finish or game Over GameObj is not assigned"); }

        currentState = State.OnGround;                  
        GroundCheckPos = this.gameObject.transform;  
        
        flying.StartState(this);
        onDead.StartState(this);
        onGround.StartState(this);
        onFinished.StartState(this);
        fuelCalculator.StartState(this);
    }
    void Update()
    {
        //Debug.Log(gemCount);
        SafeToLand();
        fuelCalculator.UpdateState();
        switch (currentState)
        {
            case State.Flying:
            case State.OnGround:
            break;
            case State.OnFinished:
                StartCoroutine(onFinished.FinishedCoroutine());
            break;
            case State.OnDead:
                StartCoroutine(onDead.DeadCoroutine());
            break;
        }
    }
    private void FixedUpdate()
    {
        switch (currentState)
        {
            case State.OnGround:
                onGround.UpdateState();
                break;
            case State.Flying:
                flying.UpdateState();
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canChangeState) { return; }

        if (collision.collider.CompareTag("Finish"))
        {
            isCollidingWithFinish = true;
            collisionTimer = 0f; // Reset the timer when collision starts
        }
        else if (!collision.collider.CompareTag("Start") && !collision.collider.CompareTag("Finish"))
        {
            currentState = State.OnDead;
            canChangeState = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        flying.rotationSpeed = 0f;
        if (!canChangeState || !isCollidingWithFinish) { return; }

        if (collision.collider.CompareTag("Finish"))
        {
            collisionTimer += Time.deltaTime; // Increment the timer based on time elapsed

            if (collisionTimer >= requiredCollisionTime)
            {
                // Change state and reset collision-related variables
                if (checkVelocity)
                {
                    currentState = SafeToLand() ? State.OnFinished : State.OnDead;
                    Debug.Log(this.GetComponent<Rigidbody>().velocity.x + "X" + this.GetComponent<Rigidbody>().velocity.y + "Y");
                }
                else
                {
                    currentState = State.OnFinished;
                }

                canChangeState = false;
                isCollidingWithFinish = false; // Reset collision state
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        flying.rotationSpeed = 100f;
        if (collision.collider.CompareTag("Finish"))
        {
            isCollidingWithFinish = false;
            collisionTimer = 0f; // Reset the timer when collision ends
        }
    }


    // Coroutine to handle delayed state change
    private IEnumerator ChangeStateWithDelay(State newState)
    {
        yield return new WaitForSeconds(2f); // Adjust the delay time as needed
        currentState = newState;
        canChangeState = false;
    }

    internal void ChangeState(State newState)
    {
        currentState = newState; 
    }
    internal void Create(GameObject spawnObject,Transform pos)
    {
        Instantiate(spawnObject, pos.position,Quaternion.identity);
    }
    internal void LevelChange(int buildIndex)
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + buildIndex;

        int maxUnlockedLevels = PlayerPrefs.GetInt("UnlockedLevel");
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings && nextSceneIndex > maxUnlockedLevels)
        {
                PlayerPrefs.SetInt("UnlockedLevel", nextSceneIndex);
        }
        // Check if the next scene index is within the valid range
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next scene if it exists
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // If no next scene exists (it's the last scene), reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    internal bool SafeToLand()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        
        return rb.velocity.y < allowedVelocityY && rb.velocity.x < allowedVelocityX;
    }
    private void OnTriggerEnter(Collider other)
    {
        //gems collected in level
        if (other.gameObject.GetComponent<GemBehaviour>())
        {
            gemCount++;
            if (AudioManager.Instance != null)
            {
                if (!AudioManager.Instance.sfxSource.isPlaying)//audio
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.gemAudioClip);
                }
            }
            Instantiate(GemCollectionParticle,transform.position,Quaternion.identity);
            Destroy(other.gameObject);
        }
    }
}