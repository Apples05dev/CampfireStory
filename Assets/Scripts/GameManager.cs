using System.Collections;
using UnityEngine;

[System.Serializable]
public class OpeningAnimations
{
    public string Name;
    public float Duration;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private InputManager _player;
    [SerializeField] private EnemyBehavior _enemy;
    [SerializeField] private AnimatedObject _cinematicAnimator;
    private CameraMover _cameraMover;

    public bool Active {  get; private set; }
    public bool Started { get; private set; }
    private bool _isStarting;

    [Space(10)]
    [SerializeField] private GameObject _startImage;
    [SerializeField] private GameObject _restartImage;

    [Space(10)]
    [SerializeField] private OpeningAnimations[] _openingAnims;
    [SerializeField] private float _cameraTime = 2f;
    [SerializeField] private string _winAnimName = "Win";
    [SerializeField] private string _loseAnimName = "Lose";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _cameraMover = Camera.main.GetComponent<CameraMover>();
    }

    public void StartIntro()
    {
        Started = true;

        _startImage.SetActive(false);
        _restartImage.SetActive(false);

        StartCoroutine(OpeningCinematic());
    }

    public void StartGame()
    {
        Active = true;
        _enemy.ToggleActive(true);
    }

    public void StopEnemyAI()
    {
        _enemy.ToggleActive(false);
    }

    public void EndGame(bool hasWon)
    {
        Active = false;
        _enemy.ToggleActive(false);

        _cinematicAnimator.SetAnimation(hasWon ? _winAnimName : _loseAnimName);
        _cameraMover.StartMove(1, _cameraTime);
    }

    public void CameraHasMoved()
    {
        if (!_isStarting)
        {
            _isStarting = true;
            StartGame();
        }
        else
        {
            _isStarting = false;
            Started = false;

            _startImage.SetActive(true);
            _restartImage.SetActive(true);
        }
    }

    public IEnumerator OpeningCinematic()
    {
        yield return Wait(1);

        foreach (OpeningAnimations anim in _openingAnims)
        {
            _cinematicAnimator.SetAnimation(anim.Name);
            yield return Wait(anim.Duration);
        }

        _player.StateMachine.ChangeState(new State_Idle());
        _player.StateMachine.HealthManager.ResetHealth();
        _enemy.StateMachine.ChangeState(new State_Idle());
        _enemy.StateMachine.HealthManager.ResetHealth();

        _cameraMover.StartMove(2, _cameraTime);

        yield return null;
    }

    private IEnumerator Wait(float seconds)
    {
        float t = 0;
        while (t < seconds)
        {
            t += Time.deltaTime;
            yield return null;
        }
    }
}
