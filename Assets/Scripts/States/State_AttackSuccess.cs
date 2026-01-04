using UnityEngine;

public class State_AttackSuccess: State
{
    public override void OnStart(StateMachine sm)
    {
        TimerLength = 0.3f;
        IsVulnerable = false;
        NextState = new State_Idle();
        _animName = "AttackSuccess";

        base.OnStart(sm);
    }
}
