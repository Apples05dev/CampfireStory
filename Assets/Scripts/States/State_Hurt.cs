using UnityEngine;

public class State_Hurt : State
{
    public override void OnStart(StateMachine sm)
    {
        // Constructor
        TimerLength = 0.5f;
        IsVulnerable = true;
        NextState = new State_Idle();
        _animName = "Hurt";

        // Since being in the hurt state always means the object has taken damage, might as well run the logic for it here.
        if (sm.HealthManager != null)
        {
            // If the object's HP drops to 0
            if (!sm.HealthManager.Damage(1))
            {
                IsVulnerable = false;
                NextState = new State_Lose();
                GameManager.Instance.StopEnemyAI();
            }
        }

        sm.Animator.FlashColor(Color.red);

        base.OnStart(sm);
    }
}
