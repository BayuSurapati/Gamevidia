using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public Animator fadeAnimator;
    public float cutsceneDuration = 5f;
    public string nextScene = "Menu";

    void Start()
    {
        StartCoroutine(CutsceneRoutine());
        B_AudioManager.Instance.PlaySFX(3);
    }

    IEnumerator CutsceneRoutine()
    {
        // ===== FADE IN =====
        fadeAnimator.Play("FadeIn", 0, 0f);
        yield return WaitAnimFinish("FadeIn");

        // ===== TAMPIL CUTSCENE =====
        yield return new WaitForSeconds(cutsceneDuration);

        // ===== FADE OUT =====
        fadeAnimator.Play("FadeOut", 0, 0f);
        yield return WaitAnimFinish("FadeOut");

        // ===== PINDAH SCENE (sudah FULL hitam) =====
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator WaitAnimFinish(string stateName)
    {
        // tunggu sampai state benar-benar aktif
        AnimatorStateInfo info;
        do
        {
            info = fadeAnimator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        while (!info.IsName(stateName));

        // tunggu anim selesai
        while (info.normalizedTime < 1f)
        {
            info = fadeAnimator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
    }
}
