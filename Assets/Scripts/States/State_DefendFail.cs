using UnityEngine;

public class State_DefendFail : State
{
    public override void OnStart(StateMachine sm)
    {
        TimerLength = 0.75f;
        NextState = new State_Idle();
        _animName = "DefendFail";

        base.OnStart(sm);
    }
}
