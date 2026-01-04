using UnityEngine;

public class State_AttackStart : State
{
    public override void OnStart(StateMachine sm)
    {
        // Constructor
        TimerLength = 0.4f;
        _animName = "AttackStart";

        base.OnStart(sm);
    }

    public override void OnTimerEnd(StateMachine sm)
    {
        // Try to hurt opponent to know which state to go to next.
        // Auto-fails if there is no opponent.
        if (sm.Opponent == null || !sm.Opponent.TryHurt())
        {
            sm.ChangeState(new State_AttackFail());
        }
        else
        {
            sm.ChangeState(new State_AttackSuccess());
        }
    }
}
