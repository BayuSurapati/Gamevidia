using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class B_PauseMenuManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;

    [Header("Button Animators")]
    public Animator resumeBtnAnim;
    public Animator restartBtnAnim;
    public Animator quitBtnAnim;

    [Header("Animation Settings")]
    public float buttonDelay = 0.1f;

    private PlayerInputActions inputActions;
    private bool isPaused;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Pause.performed += OnPause;
    }

    void OnDisable()
    {
        inputActions.UI.Pause.performed -= OnPause;
        inputActions.UI.Disable();
    }

    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void OnPause(InputAction.CallbackContext context)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        StartCoroutine(ShowButtons());
    }

    IEnumerator ShowButtons()
    {
        resumeBtnAnim.SetTrigger("Open");
        yield return new WaitForSecondsRealtime(buttonDelay);

        restartBtnAnim.SetTrigger("Open");
        yield return new WaitForSecondsRealtime(buttonDelay);

        quitBtnAnim.SetTrigger("Open");
    }

    public void ResumeGame()
    {
        StartCoroutine(HideButtonsAndResume());
    }

    IEnumerator HideButtonsAndResume()
    {
        quitBtnAnim.SetTrigger("Close");
        yield return new WaitForSecondsRealtime(buttonDelay);

        restartBtnAnim.SetTrigger("Close");
        yield return new WaitForSecondsRealtime(buttonDelay);

        resumeBtnAnim.SetTrigger("Close");
        yield return new WaitForSecondsRealtime(0.25f);

        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
