using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ArrowHintAnimator : MonoBehaviour
{
    public Animator[] arrowAnims;

    [Header("Bool Names")]
    public string boolIn = "isIn";
    public string boolOut = "isOut";

    [Header("Anim Length")]
    public float inAnimLength = 0.25f;
    public float outAnimLength = 0.25f;

    SpriteRenderer[] srs;

    void Awake()
    {
        srs = GetComponentsInChildren<SpriteRenderer>(true);
    }

    void SetVisible(bool visible)
    {
        if (srs == null) return;
        foreach (var sr in srs)
        {
            if (sr == null) continue;
            sr.enabled = visible;

            // tameng kalau ada yg ubah alpha
            var c = sr.color;
            c.a = 1f;
            sr.color = c;
        }
    }

    public void HideInstant()
    {
        StopAllCoroutines();
        SetVisible(false);

        foreach (var anim in arrowAnims)
        {
            if (anim == null) continue;
            anim.SetBool(boolIn, false);
            anim.SetBool(boolOut, false);
        }
    }

    public void ShowInstant()
    {
        StopAllCoroutines();
        SetVisible(true);

        foreach (var anim in arrowAnims)
        {
            if (anim == null) continue;
            anim.SetBool(boolIn, false);
            anim.SetBool(boolOut, false);
        }
    }

    public void PlayIn()
    {
        StopAllCoroutines();
        SetVisible(true);

        foreach (var anim in arrowAnims)
        {
            if (anim == null) continue;
            anim.SetBool(boolOut, false);
            anim.SetBool(boolIn, true);
        }

        StartCoroutine(ResetInBool());
    }

    IEnumerator ResetInBool()
    {
        yield return new WaitForSeconds(inAnimLength);

        foreach (var anim in arrowAnims)
        {
            if (anim == null) continue;
            anim.SetBool(boolIn, false);
        }
    }

    public IEnumerator PlayOutRoutine()
    {
        StopAllCoroutines();
        SetVisible(true);

        foreach (var anim in arrowAnims)
        {
            if (anim == null) continue;
            anim.SetBool(boolIn, false);
            anim.SetBool(boolOut, true);
        }

        yield return new WaitForSeconds(outAnimLength);

        foreach (var anim in arrowAnims)
        {
            if (anim == null) continue;
            anim.SetBool(boolOut, false);
        }

        // HIDE tapi object tetap hidup
        SetVisible(false);
    }

    public void ForceStopAll()
    {
        StopAllCoroutines();

        foreach (var anim in arrowAnims)
        {
            if (anim == null) continue;
            anim.SetBool(boolIn, false);
            anim.SetBool(boolOut, false);
        }

        SetVisible(true);
    }
}