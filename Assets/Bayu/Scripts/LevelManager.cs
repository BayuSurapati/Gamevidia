using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        PlayerSpawnPoint spawnPoint = FindObjectOfType<PlayerSpawnPoint>();

        if (FindObjectOfType<PlayerScripts>() != null)
            return;

        if (spawnPoint != null)
        {
            Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Error");
        }
    }

    public void ResumeGame()
    {
        B_PauseMenuManager.Instance.pausePanel.SetActive(false);
        Time.timeScale = 1f;
        B_PauseMenuManager.Instance.isPaused = false;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
        Destroy(this.gameObject);
        Time.timeScale = 1f;
    }
}
