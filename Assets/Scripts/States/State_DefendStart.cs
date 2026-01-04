using UnityEngine;

public class State_DefendStart : State
{
    public override void OnStart(StateMachine sm)
    {
        // Constructor
        TimerLength = 0.33f;
        IsVulnerable = false;
        NextState = new State_DefendFail(); // Default outcome, but is not guaranteed.
        _animName = "DefendStart";

        base.OnStart(sm);
    }

    public override void OnHurt(StateMachine sm)
    {
        // Getting attacked while in this State is a success. Such is the way of the defender.
        _stopTimer = true;
        sm.ChangeState(new State_DefendSuccess());
    }
}
