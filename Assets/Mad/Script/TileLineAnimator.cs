using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLineAnimator : MonoBehaviour
{
    [Header("Lines")]
    public Transform lineLeft;
    public Transform lineRight;

    [Header("Shrink")]
    public float duration = 0.35f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    Vector3 leftStartScale;
    Vector3 rightStartScale;

    void Awake()
    {
        if (lineLeft != null) leftStartScale = lineLeft.localScale;
        if (lineRight != null) rightStartScale = lineRight.localScale;
    }

    public void ShowInstant()
    {
        if (lineLeft != null)
        {
            lineLeft.gameObject.SetActive(true);
            lineLeft.localScale = leftStartScale;
        }

        if (lineRight != null)
        {
            lineRight.gameObject.SetActive(true);
            lineRight.localScale = rightStartScale;
        }
    }

    public void PlayHideAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(ShrinkRoutine());
    }

    IEnumerator ShrinkRoutine()
    {
        if (lineLeft != null) lineLeft.gameObject.SetActive(true);
        if (lineRight != null) lineRight.gameObject.SetActive(true);

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / duration);
            float e = ease.Evaluate(n);

            // shrink Y only (pivot center)
            if (lineLeft != null)
            {
                Vector3 s = leftStartScale;
                s.y = Mathf.Lerp(leftStartScale.y, 0f, e);
                lineLeft.localScale = s;
            }

            if (lineRight != null)
            {
                Vector3 s = rightStartScale;
                s.y = Mathf.Lerp(rightStartScale.y, 0f, e);
                lineRight.localScale = s;
            }

            yield return null;
        }

        if (lineLeft != null) lineLeft.gameObject.SetActive(false);
        if (lineRight != null) lineRight.gameObject.SetActive(false);
    }
}
