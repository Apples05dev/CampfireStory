public class State_Idle : State
{
    public override void OnStart(StateMachine sm)
    {
        CanAct = true;
        _animName = "Idle";

        base.OnStart(sm);
    }
}
