using UnityEngine;

public class State_Lose : State
{
    public override void OnStart(StateMachine sm)
    {
        // Constructor
        TimerLength = 0.65f;
        IsVulnerable = false;
        NextState = new State_Lost();
        _animName = "Lose";

        // Change opponent to win-state
        sm.Opponent.ChangeState(new State_Win());

        base.OnStart(sm);
    }
}
