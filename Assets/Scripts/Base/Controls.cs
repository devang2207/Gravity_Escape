using UnityEngine;
public class Controls : MonoBehaviour
{
    //NAMING Sequence

    //PARAMETERS:-for tuning/mainly in editor  like serializefield
    //CACHE:- eg:- refereneces readability or speed
    //STATE:- private instance variables


    //taking transform of every groundcheck

    [SerializeField] Transform GroundCheck1;              //groundCheck

    [SerializeField] ParticleSystem LeftParticle;         //Particle Systems
    [SerializeField] ParticleSystem RightParticle;
    [SerializeField] ParticleSystem MidParticle;

    [SerializeField] float groundCheckSize = 1f;
    [SerializeField] int rocketTrust;                     //speed for rocket
    [SerializeField] int rotationSpeed;                   // rotation Speed for rocket
    [SerializeField] LayerMask safeMask;
    [SerializeField] float maxVelocity;
    [SerializeField] AudioSource sideThrustAudio;

    AudioSource defaultAudioSource;
    Rigidbody rocketRb;

    bool isGrounded;                              //state for grounded or not
    bool isFuelAvailable;                         //fuel availability check
    InputHandler inputHandler;
    private State state = State.onGround;
    
    //public static NasaUdneWala instance; //this means this thing is globally accessible

    //awake is called before start
    private void Awake()
    {
        
        rocketRb = GetComponent<Rigidbody>();             //taking rb component added to player
    }
    private void Start()
    {
        inputHandler = InputHandler.Instance;
    }
    void Update()
    {
        defaultAudioSource = GetComponent<AudioSource>();
        isGrounded = GroundCheck();
        RocketMovement();

        state = GroundCheck() ? State.onGround : State.Flying;
        if (isGrounded)
        {
            state = State.onGround;
            DisableParticleSystemAndAudio();
        }
        else
        {
            state = State.Flying;

        };
        //Debug.Log(state.ToString());
      
    }
    void RocketMovement()
    {
        if (isFuelAvailable == false) {  }
        if (inputHandler.Thrust)
        {
            ProcessThrust();

        }
        else
        {
            defaultAudioSource.Stop();
            MidParticle.Stop();
        }

        //using parameters for roatating right and left on input
        if (!isGrounded)
        {
            if (inputHandler.RotateLeft)
            {
                RotateLeft();
                rocketRb.AddForce(Vector3.up * rocketTrust/3 * Time.deltaTime);
                if (!sideThrustAudio.isPlaying)
                {
                    sideThrustAudio.Play();
                }
            }
            else if (inputHandler.RotateRight)
            {
                RotateRight();
                rocketRb.AddForce(Vector3.up * rocketTrust / 3 *Time.deltaTime);
                if (!sideThrustAudio.isPlaying)
                {
                    sideThrustAudio.Play();
                }
            }
            else
            {
                //set particles to 0
                LeftParticle.Stop();
                RightParticle.Stop();
                //stop audio 
                sideThrustAudio.Stop();
            }
        }
    }

    private void RotateRight()
    {
        //Rotate right
        DoRotation(-rotationSpeed);
        if (LeftParticle.isPlaying == false)
        {
            LeftParticle.Play();
        }
    }

    private void RotateLeft()
    {
        //Rotate left
        DoRotation(rotationSpeed);
        if (RightParticle.isPlaying == false)
        {
            RightParticle.Play();
        }
    }

    private void ProcessThrust()
    {
        //calculating upwards force in an vector
        Vector3 upwardForce = Vector3.up * rocketTrust * Time.deltaTime;
        rocketRb.AddRelativeForce(upwardForce, ForceMode.Acceleration);
        if (!defaultAudioSource.isPlaying)
        {
            defaultAudioSource.Play();
        }
        if (!MidParticle.isPlaying)
        {
            MidParticle.Play();
        }
    }

    void DoRotation(float RotationValue)
    {
        transform.Rotate(Vector3.forward * RotationValue * Time.deltaTime);
    }

    //Checking if Grounded
    bool GroundCheck()
    {
        bool groundCheck = Physics.CheckSphere(GroundCheck1.position,groundCheckSize,safeMask);
        return groundCheck;
    }

    //to draw groundchecks
    private void OnDrawGizmos()
    {
       // Gizmos.DrawSphere(GroundCheck1.position, groundCheckSize);
    }


    private void DisableParticleSystemAndAudio()
    {
        //disabling particle system
        RightParticle.Stop();
        LeftParticle.Stop();
        //disabling audio
        sideThrustAudio.Stop();
    }


    //onDisable Stop everything
    private void OnDisable()
    {
        MidParticle.Stop();
        LeftParticle.Stop();
        RightParticle.Stop();
        if (defaultAudioSource != null && defaultAudioSource.isPlaying)
        {
            defaultAudioSource.Stop();
        }
        
        sideThrustAudio.Stop();
    }
    public enum State
    {
        Flying,
        onGround,
        onFinished
    }

}
