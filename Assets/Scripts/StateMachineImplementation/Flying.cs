using UnityEngine;
[System.Serializable]
public class Flying 
{
    StateMachine SM_Ref;
    Rigidbody PlayerRb;
    internal float rotationSpeed = 100;
    private bool isGrounded;
    public void StartState(StateMachine state)
    {
        SM_Ref = state;
        PlayerRb = SM_Ref.GetComponent<Rigidbody>();
    }
    public  void UpdateState()
    {
        isGrounded = Physics.Raycast(SM_Ref.GroundCheckPos.position, Vector3.down, SM_Ref.groundCheckSize, SM_Ref.Safe);
        if (isGrounded)
        {
            SM_Ref.ChangeState(StateMachine.State.OnGround);
        }

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
            SM_Ref.midParticleSys.Stop();
        }
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
        PlayerRb.transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
    }
    internal void DisableVfx_ResetRotationSpeed()
    {
        rotationSpeed = 0;
        SM_Ref.midParticleSys.gameObject.SetActive(false);
        SM_Ref.rightParticleSys.gameObject.SetActive(false);
        SM_Ref.leftParticleSys.gameObject.SetActive(false);
    }

}
