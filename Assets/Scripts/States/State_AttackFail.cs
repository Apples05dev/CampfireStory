using UnityEngine;

public class State_AttackFail: State
{
    public override void OnStart(StateMachine sm)
    {
        TimerLength = 0.5f;
        NextState = new State_Idle();
        _animName = "AttackFail";

        base.OnStart(sm);
    }
}
