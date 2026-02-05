using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class B_MainMenu : MonoBehaviour
{
    [Header("GameScene")]
    public string gameSceneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
