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
        B_AudioManager.Instance.PlayBGM(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
        B_AudioManager.Instance.StopBGM();
    }
}
