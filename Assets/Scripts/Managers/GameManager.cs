using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool started { get;private set; }
    public UnityEvent StartGameEvent;
    #region Singleton
    public static GameManager Instance;
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void OnDestroy()
    {
        Instance = null;
    }
    #endregion Singleton


    [Header("Overlays")]
    public GameObject pauseOverlay;
    public GameObject levelCompletedOverlay;
    public GameObject gameOverOverlay;

    [Header("Music / Sounds")]
    public string gameOverTrack;
    public string levelCompleteTrack;

    private void Start()
    {
        WaveSpawner.Instance.enabled = false;
    }

    public void StartGame()
    {
        started = true;
        StartGameEvent.Invoke();
        WaveSpawner.Instance.enabled = true;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseOverlay.SetActive(true);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseOverlay.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LevelComplete() 
    {
        levelCompletedOverlay.SetActive(true);
        AudioManager.Instance.PlayAudio(levelCompleteTrack);
    }

    public void GameOver() 
    {
        gameOverOverlay.SetActive(true);
        AudioManager.Instance.PlayAudio(gameOverTrack);
    }
}
