using UnityEngine;

public abstract class BaseState : ScriptableObject
{
    public virtual void OnEnter(BaseStateMachine sm) { }

    public abstract void OnUpdate(BaseStateMachine sm);

    public virtual void OnFixedUpdate(BaseStateMachine sm) { }

    public virtual void OnExit(BaseStateMachine sm) { }
}
