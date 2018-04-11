using DigitalRuby.Tween;
using UnityEngine;

public class TweenAnimation : MonoBehaviour
{
    public float Offset = 0.1f;
    public float AnimationSpeed = 3f;

    public void Start()
    {
        TweenMove();
    }

    private void TweenMove()
    {
        Vector3 startPos = transform.localPosition;
        Vector3 endPos = startPos + new Vector3(0f, Offset, transform.localPosition.z);

        gameObject.Tween("MoveUpAndDown", startPos, endPos, 1f / AnimationSpeed, TweenScaleFunctions.CubicEaseOut, (t) =>
        {
            gameObject.transform.localPosition = t.CurrentValue;
        }, (t) =>
        {
            gameObject.Tween("MoveUpAndDown", endPos, startPos, 1f / AnimationSpeed, TweenScaleFunctions.CubicEaseIn, (t2) =>
            {
                gameObject.transform.localPosition = t2.CurrentValue;
            }, (t2) =>
            {
                TweenMove();
            });
        });
    }

    private void TweenRotate()
    {
        float startAngle = transform.rotation.eulerAngles.z;
        float endAngle = startAngle + 720.0f;
        gameObject.Tween("RotateCircle", startAngle, endAngle, 2.0f, TweenScaleFunctions.CubicEaseInOut, (t) =>
        {
            // progress
            transform.rotation = Quaternion.identity;
            transform.Rotate(Camera.main.transform.forward, t.CurrentValue);
        }, (t) =>
        {
            TweenRotate();
        });
    }
}