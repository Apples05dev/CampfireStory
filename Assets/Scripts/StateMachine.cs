using UnityEngine;

public delegate void OnStateChange(State newState);

[RequireComponent(typeof(AnimatedObject))]
public class StateMachine : MonoBehaviour
{
    public StateMachine Opponent;

    public HealthManager HealthManager { get; private set; }
    public AnimatedObject Animator { get; private set; }
    public State State { get; private set; }
    public event OnStateChange OnStateChange;

    private void Start()
    {
        Animator = GetComponent<AnimatedObject>();
        HealthManager = GetComponentInChildren<HealthManager>();

        State = new State_Idle(); // Default state.
        State.OnStart(this);
    }

    private void Update()
    {
        State?.OnUpdate(this);
    }

    public void SetAnimation(string animName)
    {
        Animator.SetAnimation(animName);
    }

    public void ChangeState(State newState)
    {
        State.OnExit(this);
        State = newState;

        newState.OnStart(this);
        OnStateChange?.Invoke(newState);
    }

    private bool TryAction(State newState)
    {
        // Check if the current state allows acts and return it.
        if (State.CanAct)
        {
            ChangeState(newState);
            return true;
        }
        return false;
    }

    public bool TryAttack()
    {
        return TryAction(new State_AttackStart());
    }
    public bool TryDefend()
    {
        return TryAction(new State_DefendStart());
    }

    public bool TryHurt()
    {
        // Create new bool in case the current state's OnHurt causes a change in states.
        bool success = State.IsVulnerable;
        State.OnHurt(this);

        return success;
    }
}