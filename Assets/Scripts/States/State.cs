using UnityEngine;

public class State
{
    public float TimerLength { get; protected set; }
    public bool IsVulnerable { get; protected set; } = true;
    public bool CanAct { get; protected set; } = false;
    public State NextState { get; protected set; }

    protected float _timer;
    protected string _animName;
    protected bool _stopTimer;

    /// <summary>
    /// Runs when the StateMachine changes to this state. State variables should be declared here.
    /// "base.OnStart" should be ran last in most cases.
    /// </summary>
    public virtual void OnStart(StateMachine sm)
    {
        // You'll usually want to construct the state variables here.
        // More managable than doing it all in the StateMachine, imo.

        // Debug.Log($"{sm.gameObject.name}: state changed to {GetType().Name}");

        // Trigger the animation.
        sm.Animator.SetAnimation(_animName);

        // Set the timer.
        _timer = TimerLength;
    }

    /// <summary>
    /// Runs every frame while this state is active. 
    /// "base.OnUpdate" should be ran last in most cases.
    /// </summary>
    public virtual void OnUpdate(StateMachine sm)
    {        
        if (_timer > 0 && !_stopTimer)
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                OnTimerEnd(sm);
            }
        }
    }

    /// <summary>
    /// Runs after the timer in OnUpdate ends. 
    /// "base.OnTimerEnd" should be ran last in most cases.
    /// </summary>
    public virtual void OnTimerEnd(StateMachine sm)
    {
        if (NextState != null)
            sm.ChangeState(NextState);
    }

    /// <summary>
    /// Runs when the StateMachine changes to another state.
    /// </summary>
    public virtual void OnExit(StateMachine sm)
    {

    }

    /// <summary>
    /// Runs when the StateMachine object gets successfully attacked.
    /// </summary>
    public virtual void OnHurt(StateMachine sm)
    {
        if (IsVulnerable)
            sm.ChangeState(new State_Hurt());
    }
}