using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHintAnimator : MonoBehaviour
{
    public SpriteRenderer[] arrows;
    public float fadeDuration = 0.35f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public void ShowInstant()
    {
        gameObject.SetActive(true);
        foreach (var a in arrows)
        {
            if (a == null) continue;
            var c = a.color;
            c.a = 1f;
            a.color = c;
        }
    }

    public void PlayHideAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        gameObject.SetActive(true);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / fadeDuration);
            float e = ease.Evaluate(n);

            foreach (var a in arrows)
            {
                if (a == null) continue;
                var c = a.color;
                c.a = Mathf.Lerp(1f, 0f, e);
                a.color = c;
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}

