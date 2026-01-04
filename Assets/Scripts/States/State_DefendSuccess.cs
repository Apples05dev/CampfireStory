using UnityEngine;

public class State_DefendSuccess : State
{
    public override void OnStart(StateMachine sm)
    {
        TimerLength = 0.25f;
        IsVulnerable = false;
        NextState = new State_Idle();
        _animName = "DefendSuccess";

        sm.Animator.FlashColor(Color.lightBlue);

        base.OnStart(sm);
    }

    public override void OnHurt(StateMachine sm)
    {
        // If the StateMachine object gets attacked *again*, simply repeat the current state.
        sm.ChangeState(this);
        
        base.OnHurt(sm);
    }
}
