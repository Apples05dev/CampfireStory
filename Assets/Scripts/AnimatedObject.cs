using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Animation
{
    public string Name;
    public Sprite[] Frames;
}

public class AnimatedObject : MonoBehaviour
{
    [SerializeField] private Animation[] _animations;
    private int _currentAnimationIndex = 0;
    private int _currentFrame = 0;

    private SpriteRenderer _spriteRenderer;
    private Image _img;
    bool _isImage;

    private void Awake()
    {
        if (TryGetComponent(out SpriteRenderer sr))
        {
            _spriteRenderer = sr;
        }
        else if (TryGetComponent(out Image img))
        {
            _img = img;
            _isImage = true;
        }
    }

    private void Start()
    {
        AnimationManager.Instance.OnAnimation += OnAnimationEvent;
    }

    private void OnAnimationEvent()
    {
        // Return if there is nothing to animate.
        if (_animations.Length <= 0)
        {
            Debug.LogWarning($"Object of name \"{gameObject.name} is trying to animate, but has nothing to do so! If this is intentional, please remove the AnimatedObject script from the object.\"");
            return;
        }

        IncrementAndSetAnimation();
    }

    private void IncrementAndSetAnimation()
    {
        if (_animations[_currentAnimationIndex].Frames.Length > 1)
        {
            // Increment the current frame index. If the current frame index is the same or higher than the amount of frames, reset it back to zero.
            _currentFrame = _currentFrame >= (_animations[_currentAnimationIndex].Frames.Length - 1) ? 0 : _currentFrame + 1;
        }
        else // Don't touch the frame index if there's only one frame.
        {
            _currentFrame = 0;
        }

        // Animate the object.
        if (_isImage)
            _img.sprite = _animations[_currentAnimationIndex].Frames[_currentFrame];
        else
            _spriteRenderer.sprite = _animations[_currentAnimationIndex].Frames[_currentFrame];
    }

    public void SetAnimation(string animName)
    {
        // Loop through all animations and set the current animation index to the one that matches the given name.
        for (int i = 0; i < _animations.Length; i++)
        {
            if (_animations[i].Name == animName)
            {
                _currentAnimationIndex = i;
                IncrementAndSetAnimation();
                return;
            }
        }

        Debug.LogWarning($"Couldn't find animation of name \"{animName}\"");
    }

    public void FlashColor(Color color, float flashTime = 0.2f)
    {
        if (_isImage)
            _img.material.SetColor("OverlayColor", color);
        else
            _spriteRenderer.material.SetColor("_OverlayColor", color);

        StartCoroutine(LerpOverlayColor(flashTime));
    }
    private IEnumerator LerpOverlayColor(float time)
    {
        float t = 0f;
        while (t < time)
        {
            float value;
            float halfTime = time / 2;

            // Get lerped value. Once t is over half, it mirrors back.
            if (t / time <= 0.5f)
                value = Mathf.Lerp(0, 1, t / halfTime);
            else
                value = Mathf.Lerp(0, 1, t - halfTime / halfTime);

            // Apply
            if (_isImage)
                _img.material.SetFloat("_OverlayAlpha", value);
            else
                _spriteRenderer.material.SetFloat("_OverlayAlpha", value);

            t += Time.deltaTime;
            yield return null;
        }
    }
}
