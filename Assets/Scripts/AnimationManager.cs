using UnityEngine;
using System.Collections;

public delegate void OnAnimationEvent();

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; }
    public event OnAnimationEvent OnAnimation;

    [SerializeField] private float _frameTime;

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
        StartAnimator();
    }

    public void StartAnimator()
    {
        StartCoroutine(Animator());
    }
    public void StopAnimator()
    {
        StopAllCoroutines();
    }

    private IEnumerator Animator()
    {
        WaitForSeconds wait = new(_frameTime);
        float currentTime = _frameTime;

        while (true)
        {
            // Lets me tweak the animation timing without having to restart or create a new WaitForSeconds every loop.
            if (_frameTime != currentTime)
            {
                wait = new(_frameTime);
                currentTime = _frameTime;
            }

            OnAnimation?.Invoke();
            // Debug.Log("Animation Invoked!");

            yield return wait;
        }
    }
}
