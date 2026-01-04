using UnityEngine;

public class State_Win : State
{
    public override void OnStart(StateMachine sm)
    {
        // Constructor
        TimerLength = 0.65f;
        IsVulnerable = false;
        NextState = new State_Won();
        _animName = "Win";

        base.OnStart(sm);
    }
}
