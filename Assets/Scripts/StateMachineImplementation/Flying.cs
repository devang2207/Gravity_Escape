using UnityEngine;
[System.Serializable]
public class Flying 
{
    StateMachine SM_Ref;
    Rigidbody PlayerRb;
    [SerializeField] float rotationSpeed = 100;
    [SerializeField] float fallMultiplier = 5;
    private bool isGrounded;
    public void StartState(StateMachine state)
    {
        SM_Ref = state;
        PlayerRb = SM_Ref.GetComponent<Rigidbody>();
    }
    public  void UpdateState()
    {
        //check if grounded
        GroundCheck();
        //GravityMultiplier();
        //movement

        Thrust();
        LeftRotation();
        RightRotation();
    }

    void GravityMultiplier()
    {
        if(PlayerRb.velocity.y < 0)
        {
            PlayerRb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }
    void GroundCheck()
    {
        isGrounded = Physics.Raycast(SM_Ref.GroundCheckPos.position, Vector3.down, SM_Ref.groundCheckSize, SM_Ref.Safe);
        if (isGrounded)
        {
            SM_Ref.ChangeState(StateMachine.State.OnGround);
        }
    }
    void Thrust()
    {
        if (InputHandler.Instance.Thrust)
        {
            PlayerRb.AddRelativeForce(Vector3.up * SM_Ref.RocketThrust * Time.deltaTime);
            if (!SM_Ref.midParticleSys.isPlaying)
            {
                SM_Ref.midParticleSys.Play();
            }
        }
        else
        {
            GravityMultiplier();
            PlayerRb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            SM_Ref.midParticleSys.Stop();
        }
    }
    void RightRotation()
    {
        if (InputHandler.Instance.RotateRight)
        {
            if (!SM_Ref.leftParticleSys.isPlaying)
            {
                SM_Ref.leftParticleSys.Play();
            }
            DoRotation(rotationSpeed);
        }
        else
        {
            SM_Ref.leftParticleSys.Stop();
        }
    }
    void LeftRotation()
    {
        if (InputHandler.Instance.RotateLeft)
        {
            if (!SM_Ref.rightParticleSys.isPlaying)
            {
                SM_Ref.rightParticleSys.Play();
            }
            DoRotation(-rotationSpeed);
        }
        else
        {
            SM_Ref.rightParticleSys.Stop();
        }
    }

    internal void DoRotation(float rotationSpeed)
    {
        PlayerRb.transform.Rotate(Vector3.back * rotationSpeed * Time.fixedDeltaTime);
    }
    internal void DisableVfx_ResetRotationSpeed()
    {
        rotationSpeed = 0;
        SM_Ref.midParticleSys.gameObject.SetActive(false);
        SM_Ref.rightParticleSys.gameObject.SetActive(false);
        SM_Ref.leftParticleSys.gameObject.SetActive(false);
    }

}
