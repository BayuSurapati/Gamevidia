using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class B_PauseMenuManager : MonoBehaviour
{
    public static B_PauseMenuManager Instance;

    [Header("UI (Auto Assign per Scene)")]
    public GameObject pausePanel;
    public Animator resumeBtnAnim;
    public Animator restartBtnAnim;
    public Animator quitBtnAnim;

    [Header("Animation Settings")]
    public float buttonDelay = 0.1f;

    private PlayerInputActions inputActions;
    public bool isPaused;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Pause.performed += OnPause;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        inputActions.UI.Pause.performed -= OnPause;
        inputActions.UI.Disable();

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPauseUIInScene();
        ResumeImmediate();
    }

    void FindPauseUIInScene()
    {
        var ui = FindObjectOfType<B_UIReference>();

        if (ui == null)
        {
            Debug.LogWarning("Pause UI Reference NOT FOUND in scene!");
            return;
        }

        pausePanel = ui.pausePanel;
        resumeBtnAnim = ui.resumeBtnAnim;
        restartBtnAnim = ui.restartBtnAnim;
        quitBtnAnim = ui.quitBtnAnim;

        pausePanel.SetActive(false);
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
        if (pausePanel == null) return;

        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        StartCoroutine(ShowButtons());
    }

    public void ResumeGame()
    {
        StartCoroutine(HideButtonsAndResume());
        LevelManager.Instance.ResumeGame();
    }

    void ResumeImmediate()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    IEnumerator ShowButtons()
    {
        resumeBtnAnim.SetTrigger("Open");
        yield return new WaitForSecondsRealtime(buttonDelay);

        restartBtnAnim.SetTrigger("Open");
        yield return new WaitForSecondsRealtime(buttonDelay);

        quitBtnAnim.SetTrigger("Open");
    }

    IEnumerator HideButtonsAndResume()
    {
        quitBtnAnim.SetTrigger("Close");
        yield return new WaitForSecondsRealtime(buttonDelay);

        restartBtnAnim.SetTrigger("Close");
        yield return new WaitForSecondsRealtime(buttonDelay);

        resumeBtnAnim.SetTrigger("Close");
        yield return new WaitForSecondsRealtime(0.15f);

        pausePanel.SetActive(false);
        ResumeImmediate();
    }

    public void QuitGame()
    {
        LevelManager.Instance.QuitGame();
    }
}
