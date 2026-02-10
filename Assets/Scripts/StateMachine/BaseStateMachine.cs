using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    [SerializeField]
    private BaseState currentState;

    private void Start()
    {
        if (currentState != null) currentState.OnEnter(this);
    }

    private void Update()
    {
        if (currentState != null) currentState.OnUpdate(this);
    }

    private void FixedUpdate()
    {
        if (currentState != null) currentState.OnFixedUpdate(this);
    }

    private void ChangeState(BaseState nextState)
    {
        if (currentState != null) currentState.OnExit(this);
        currentState = nextState;
        if (currentState != null) currentState.OnEnter(this);
    }
}
