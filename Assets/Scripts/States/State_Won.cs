using UnityEngine;

public class State_Won : State
{
    public override void OnStart(StateMachine sm)
    {
        CanAct = false;
        _animName = "Won";
        TimerLength = 1f;

        base.OnStart(sm);
    }

    public override void OnTimerEnd(StateMachine sm)
    {
        // Ends the game as a victory if the winner is the Player object.
        GameManager.Instance.EndGame(sm.gameObject.name == "Player");
    }
}
