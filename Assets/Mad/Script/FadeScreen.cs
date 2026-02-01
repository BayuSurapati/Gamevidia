using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    public Image img;
    public float fadeDuration = 0.6f;

    void Awake()
    {
        if (img == null)
            img = GetComponentInChildren<Image>(true);

        if (img == null)
            Debug.LogError("FadeScreen: Image tidak ditemukan!");
    }

    public void SetAlpha(float a)
    {
        if (img == null) return;

        var c = img.color;
        c.a = a;
        img.color = c;
    }

    public IEnumerator FadeIn()
    {
        if (img == null) yield break;

        Debug.Log("FadeIn START");
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / fadeDuration);
            SetAlpha(Mathf.Lerp(1f, 0f, n));
            yield return null;
        }

        SetAlpha(0f);
        Debug.Log("FadeIn END");
        Debug.Log("alpha = " + img.color.a);
    }
}