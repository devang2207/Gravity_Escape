using UnityEngine;

public class MonoClass : MonoBehaviour
{
    State currentState;
    // Start is called before the first frame update
    void Start()
    {
        currentState.StartState();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }
    public void ChangeState(State newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.StartState();
    }
}

