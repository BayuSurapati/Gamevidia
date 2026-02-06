using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TileLineAnimator : MonoBehaviour
{
    [Header("Lines (isi 1 atau 2)")]
    public GameObject[] lines;

    [Header("Animator")]
    public Animator[] animator;

    void Awake()
    {
        if (animator == null || animator.Length == 0)
            animator = GetComponentsInChildren<Animator>();
    }

    void SetLinesActive(bool active)
    {
        if (lines == null) return;

        foreach (var l in lines)
        {
            if (l != null) l.SetActive(active);
        }
    }

    public void ShowInstant()
    {
        SetLinesActive(true);

        foreach (var anim in animator)
        {
            if (anim == null) continue;
            anim.SetBool("InFrame", false);
            anim.SetBool("OutFrame", false);
            anim.Play("idle", 0, 0f);
        }
    }

    public void HideInstant()
    {
        SetLinesActive(false);

        foreach (var anim in animator)
        {
            if (anim == null) continue;
            anim.SetBool("InFrame", false);
            anim.SetBool("OutFrame", false);
        }
    }

    public void PlayIn()
    {
        SetLinesActive(true);

        foreach (var anim in animator)
        {
            if (anim == null) continue;
            anim.SetBool("OutFrame", false);
            anim.SetBool("InFrame", true);
        }
    }

    public void PlayOut()
    {
        SetLinesActive(true);

        foreach (var anim in animator)
        {
            if (anim == null) continue;
            anim.SetBool("InFrame", false);
            anim.SetBool("OutFrame", true);
        }
    }

    // Animation Event: taruh di akhir clip In
    public void OnInFinished()
    {
        foreach (var anim in animator)
        {
            if (anim == null) continue;
            anim.SetBool("InFrame", false);
        }
    }

    // Animation Event: taruh di akhir clip Out
    public void OnOutFinished()
    {
        foreach (var anim in animator)
        {
            if (anim == null) continue;
            anim.SetBool("OutFrame", false);
        }
        SetLinesActive(false);
    }
}