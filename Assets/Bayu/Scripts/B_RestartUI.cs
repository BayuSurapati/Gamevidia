using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class B_RestartUI : MonoBehaviour
{
    public static B_RestartUI Instance;

    public GameObject deathPanel;

    private PlayerInputActions inputActions;
    private bool canRestart;

    private void Awake()
    {
        Instance = this;
        inputActions = new PlayerInputActions();
        deathPanel.SetActive(false);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Jump.performed += OnRestart;
    }

    private void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnRestart;
        inputActions.Player.Disable();
    }

    public void Show()
    {
        deathPanel.SetActive(true);
        Time.timeScale = 0f;
        canRestart = true;
    }

    void OnRestart(InputAction.CallbackContext ctx)
    {
        if (!canRestart) return;

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
