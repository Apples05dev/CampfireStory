using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionReference _attackInput;
    [SerializeField] private InputActionReference _defendInput;

    public StateMachine StateMachine;
    private float _timer;
    [SerializeField] private float _inputGraceTime = 0.2f;
    private bool _attemptingAttack;
    private bool _attemptingDefend;

    private void OnEnable()
    {
        _attackInput.action.performed += OnAttack;
        _defendInput.action.performed += OnDefend;
    }
    private void OnDisable()
    {
        _attackInput.action.performed -= OnAttack;
        _defendInput.action.performed -= OnDefend;
    }

    private void Update()
    {
        // Simple grace timer system.
        if (_timer > 0)
        {
            if (_attemptingAttack && StateMachine.TryAttack())
            {
                _timer = 0;
            }
            else if (_attemptingDefend && StateMachine.TryDefend())
            {
                _timer = 0;
            }

            _timer -= Time.deltaTime;
        }
        else if (_attemptingAttack || _attemptingDefend)
        {
            _attemptingAttack = false;
            _attemptingDefend = false;
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.Started)
        {
            GameManager.Instance.StartIntro();
            return;
        }
        else if (!GameManager.Instance.Active)
            return;

        _attemptingAttack = true;
        _timer = _inputGraceTime;
    }
    private void OnDefend(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.Started || !GameManager.Instance.Active)
            return;

        _attemptingDefend = true;
        _timer = _inputGraceTime;
    }
}
