using System.Collections;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float _position1;
    [SerializeField] private float _position2;
    [SerializeField] AnimationCurve _animCurve;

    public void StartMove(int posNumber, float time)
    {
        // very patchwork but im running out of time here!!
        if (posNumber == 1)
            StartCoroutine(MoveLerp(_position1, time));
        else
            StartCoroutine(MoveLerp(_position2, time));
    }

    private IEnumerator MoveLerp(float targetPos, float time)
    {
        float startPos = transform.position.y;
        float t = 0;

        while (Mathf.Abs(transform.position.y - targetPos) > 0.01f) // while the difference between the current and target position is greater than 0.1
        {
            float posY = Mathf.Lerp(startPos, targetPos, _animCurve.Evaluate(t / time));
            transform.position = new(transform.position.x, posY, transform.position.z);

            t += Time.deltaTime;
            yield return null;
        }

        // set to account for minor faults in lerping
        transform.position = new(transform.position.x, targetPos, transform.position.z);

        GameManager.Instance.CameraHasMoved();
    }
}