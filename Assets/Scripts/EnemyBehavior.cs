using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class EnemyBehavior : MonoBehaviour
{
    public StateMachine StateMachine { get; private set; }

    private bool _active = false;
    
    [SerializeField] private float _randomizedActTime = 1f;
    private Coroutine _randomizedActCoroutine;

    [Space(5)]
    // The following variables should all be decimals, with the remainder between the sum and 1 being "do nothing." For example, if you want an equal chance of attacking, defending and idling, you'd put these two as 0.333.
    [SerializeField] private float _actWeight_Attack = 0.4f;
    [SerializeField] private float _actWeight_Defend = 0.35f;

    [Space(5)]
    [Tooltip("How long to wait extra when waiting by a state's length.")]
    [SerializeField] private float _waitMargin = 0.1f;
    [Tooltip("How long the max waiting time is to react to an attack with a block.")]
    [SerializeField] private float _reactionTimeRange = 0.7f;
    [Tooltip("Chance of following up a successful attack with another act.")]
    [SerializeField] private float _followUpChance = 0.33f;
    [Tooltip("Multiplier added to _followUpChance when the previous act failed.")]
    [SerializeField] private float _failedActChanceMultiplier = 1.5f;

    // Suddenly switching to an integer-based randomizer here, oops. Don't have enough time to consider making more consistent.
    [Tooltip("Initial chance of defending right after being attacked.")]
    [SerializeField] private int _damagedDefendChance = 4;
    private int _defendChanceCount = 0;

    private void Start()
    {
        StateMachine = GetComponent<StateMachine>();
        StateMachine.OnStateChange += OnStateChange;
        StateMachine.Opponent.OnStateChange += OnOpponentStateChange;
    }

    public void ToggleActive(bool active)
    {
        _active = active;
        ToggleRandomizedActLoop(active);
    }

    private void ToggleRandomizedActLoop(bool active)
    {
        if (_randomizedActCoroutine != null)
            StopCoroutine(_randomizedActCoroutine);

        if (active)
            _randomizedActCoroutine = StartCoroutine(RandomActTimer());
    }

    private void OnStateChange(State newState)
    {
        if (!_active)
            return;
        
        Type stateType = newState.GetType();

        // If I successfully defend an attack, immediately follow with my own attack.
        if (stateType == typeof(State_DefendSuccess))
            StartCoroutine(WaitToAct(1, newState.TimerLength + _waitMargin));

        // After an attack, decide whether to follow up with a random act or not based on r. Increase chance when the attack was unsuccessful.
        else if (stateType == typeof(State_AttackSuccess) || stateType == typeof(State_AttackFail))
        {
            float r = UnityEngine.Random.Range(0f, 1f);
            float chance = stateType == typeof(State_AttackFail) ? _followUpChance * _failedActChanceMultiplier : _followUpChance;
            if (r < chance)
                StartCoroutine(WaitToAct(0, newState.TimerLength + _waitMargin));
        }

        // After being successfully attacked, roll a chance to defend. increase this chance every time it fails, and reset the counter otherwise.
        else if (stateType == typeof(State_Hurt))
        {
            float r = UnityEngine.Random.Range(1, _damagedDefendChance - _defendChanceCount);
            if (r == 1)
            {
                StartCoroutine(WaitToAct(2, newState.TimerLength + _waitMargin * 2)); // Waits a bit longer so the defend is more likely to be successful.
                _defendChanceCount = 0;
            }
            else
                _defendChanceCount++;
        }
    }
    private void OnOpponentStateChange(State newState)
    {
        if (!_active)
            return;

        Type stateType = newState.GetType();

        // If my opponent attacks, defend after r seconds based on _reactionTimeRange.
        if (stateType == typeof(State_AttackStart))
        {
            float r = UnityEngine.Random.Range(0f, _reactionTimeRange);
            StartCoroutine(WaitToAct(2, r));
        }

        // If my opponent defends while I am idle, try to attack immediately.
        else if (stateType == typeof(State_DefendStart) || stateType == typeof(State_DefendFail))
        {
            if (StateMachine.State.GetType() == typeof(State_Idle))
                StateMachine.TryAttack();
        }
    }

    private void RandomAct()
    {
        float r = UnityEngine.Random.Range(0f, 1f);

        // Attack
        float w = _actWeight_Attack;
        if (r < w)
        {
            StateMachine.TryAttack();
            return;
        }

        // Defend
        w += _actWeight_Defend;
        if (r < w)
        {
            StateMachine.TryDefend();
            return;
        }

        // If r is still higher than w, do nothing.
        return;
    }
    private IEnumerator RandomActTimer()
    {
        WaitForSeconds wait = new(_randomizedActTime);

        while (true)
        {
            // wait for '_randomizedActTime' seconds at the start of the loop.
            yield return wait;

            // Skip to the next loop in case the enemy shouldn't be active. Shouldn't happen since deactivating disables the timer, but this is here just in case.
            if (!_active)
                continue;

            // Skip to next loop (AKA start waiting again) if the current state isn't idle.
            if (StateMachine.State.GetType() != typeof(State_Idle))
                continue;

            RandomAct();
        }
    }

    /// <summary>
    /// Wait for 'delay' seconds for an act based on 'setting'.
    /// </summary>
    /// <param name="setting">What type of act to wait for. 0 = Random, 1 = Attack, 2 = Defend</param>
    /// <param name="delay">How long to wait for.</param>
    /// <returns></returns>
    private IEnumerator WaitToAct(int setting, float delay)
    {
        yield return new WaitForSeconds(delay);

        switch (setting)
        {
            case 0: RandomAct();
                break;
            case 1: StateMachine.TryAttack();
                break;
            case 2: StateMachine.TryDefend();
                break;
        }
    }
}
