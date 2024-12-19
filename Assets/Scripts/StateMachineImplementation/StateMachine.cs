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
    internal Transform GroundCheckPos;
    #endregion

    #region  CommonVariables_OnDead&OnFinished
    [Header("OnDead&OnFinished")]
    [SerializeField]internal float allowedVelocityY = 5;
    [SerializeField]internal float allowedVelocityX = 5;
    [SerializeField]internal Animator LevelTransition;
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
    internal bool canChangeState = true;

    //current state
    internal State currentState;
    public enum State
    {
        Flying,OnGround,OnFinished,OnDead
    }
    void Start()
    {
        currentState = State.OnGround;                  //start state
        GroundCheckPos = this.gameObject.transform;     
        onGround.StartState(this);
        flying.StartState(this);
        onDead.StartState(this);
        onFinished.StartState(this);
        fuelCalculator.StartState(this);
    }
    void Update()
    {
        SafeToLand();
        fuelCalculator.UpdateState();
        switch (currentState)
        {
            case State.OnGround:
                onGround.UpdateState();
            break;
            case State.Flying:
                flying.UpdateState();
            break;
            case State.OnFinished:
                StartCoroutine(onFinished.FinishedCoroutine());
            break;
            case State.OnDead:
                StartCoroutine(onDead.DeadCoroutine());
            break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!canChangeState){return;}

        if (collision.collider.CompareTag("Finish"))
        {
            Debug.Log(this.GetComponent<Rigidbody>().velocity.x+"X" + this.GetComponent<Rigidbody>().velocity.y+"Y"); 
            currentState = SafeToLand()? State.OnFinished : State.OnDead;
            canChangeState = false;
        }
        else if (!collision.collider.CompareTag("Start")&&!collision.collider.CompareTag("Finish"))
        {
            currentState = State.OnDead;
            canChangeState = false;
        }
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
        if(nextSceneIndex > maxUnlockedLevels)
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
}