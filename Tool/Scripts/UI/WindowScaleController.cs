using System.Collections;
using UnityEngine;

/// <summary>
/// 协程控制窗口弹窗动画
/// </summary>
public class WindowScaleController : MonoBehaviour
{
    [SerializeField] AnimationCurve showCurve;
    [SerializeField] AnimationCurve hideCurve;
    [SerializeField] float animationSpeed = 1f;

    protected virtual IEnumerator ShowPlane(Transform window)
    {
        float time = 0;
        if (window.localScale == Vector3.one) yield break;
        while (time <= 1f)
        {
            window.localScale = Vector3.one * showCurve.Evaluate(time);
            time += Time.deltaTime * animationSpeed;
            yield return null;
        }
        window.localScale = Vector3.one;
    }

    protected virtual IEnumerator HidePlane(Transform window)
    {
        float time = 0;
        if (window.localScale == Vector3.zero) yield break;
        while (time <= 1f)
        {
            window.localScale = Vector3.one * hideCurve.Evaluate(time);
            time += Time.deltaTime * animationSpeed;
            yield return null;
        }
        window.localScale = Vector3.zero;
    }
}
