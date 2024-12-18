using UnityEngine;
[System.Serializable]
public class OnGround 
{
    StateMachine SM_Ref;
    Rigidbody PlayerRb;
    bool grounded = true; 
    internal void StartState(StateMachine stateMachine)
    {
        SM_Ref = stateMachine;
        PlayerRb = SM_Ref.GetComponent<Rigidbody>();
    }
    internal void UpdateState()
    {
        grounded = Physics.Raycast(SM_Ref.GroundCheckPos.position, Vector3.down, SM_Ref.groundCheckSize,SM_Ref.Safe);
        if (InputHandler.Instance.Thrust)
        {
           PlayerRb.AddRelativeForce(Vector3.up*SM_Ref.RocketThrust*Time.deltaTime);
        }
        if (!grounded)
        {
            SM_Ref.ChangeState(StateMachine.State.Flying);
        }
    }
    
}
