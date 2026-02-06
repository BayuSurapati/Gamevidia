using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class B_ComicCutsceneController : MonoBehaviour
{
    [Header("Comic Strips (Order Matters)")]
    public GameObject[] comicStrips;

    [Header("Timing")]
    public float stripDelay = 0.8f;

    [Header("UI")]
    public GameObject pressSpaceText;

    [Header("Next Scene")]
    public string nextSceneName = "MainScene";

    private PlayerInputActions inputActions;
    private bool canContinue;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Confirm.performed += OnConfirm;
    }

    void OnDisable()
    {
        inputActions.UI.Confirm.performed -= OnConfirm;
        inputActions.UI.Disable();
    }

    void Start()
    {
        pressSpaceText.SetActive(false);

        foreach (var strip in comicStrips)
            strip.SetActive(false);

        StartCoroutine(PlayComicSequence());
    }

    IEnumerator PlayComicSequence()
    {
        for (int i = 0; i < comicStrips.Length; i++)
        {
            comicStrips[i].SetActive(true);

            Animator anim = comicStrips[i].GetComponent<Animator>();
            if (anim != null)
                anim.SetTrigger("Open");

            yield return new WaitForSeconds(stripDelay);
        }

        pressSpaceText.SetActive(true);
        canContinue = true;
    }

    void OnConfirm(InputAction.CallbackContext context)
    {
        if (!canContinue) return;

        SceneManager.LoadScene(nextSceneName);
    }
}
