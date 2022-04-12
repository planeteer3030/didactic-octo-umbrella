using UnityEngine;

public abstract class AppState : ScriptableObject
{
    public virtual void OnEnterState() { }

    public virtual void OnExitState() { }

    public virtual void Update() { }
}
