using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    public Image img;
    public float fadeDuration = 0.6f;
    [SerializeField, Range(0.01f, 1f)]
    private float flashDuration = 0.2f;

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

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / fadeDuration);
            float eased = Mathf.SmoothStep(0f, 1f, progress);
            SetAlpha(Mathf.Lerp(1f, 0f, eased));
            yield return null;
        }
        SetAlpha(0f);
    }

    public IEnumerator FadeOut(float duration)
    {
        if (img == null) yield break;
        float elapsed = 0f;
        float start = img.color.a;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            SetAlpha(Mathf.Lerp(start, 1f, t));
            yield return null;
        }
        SetAlpha(1f);
    }

    // ✅ Satu-satunya FlashQuick dengan callback opsional
    public IEnumerator FlashQuick(Action onBlack = null)
    {
        // fade to black
        yield return FadeOut(flashDuration);

        // callback saat layar full hitam
        onBlack?.Invoke();

        // fade kembali ke transparan
        yield return FadeIn();
    }

    // helper non-coroutine
    public void QuickFlash()
    {
        StartCoroutine(FlashQuick());
    }
}