using UnityEngine;

public class State_Lost : State
{
    public override void OnStart(StateMachine sm)
    {
        CanAct = false;
        _animName = "Lost";

        base.OnStart(sm);
    }
}
