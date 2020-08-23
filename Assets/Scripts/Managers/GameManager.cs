using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    public GameObject gameOverOverlay;
    public string gameOverTrack;

    public void GameOver() 
    {
        gameOverOverlay.SetActive(true);
        AudioManager.Instance.PlayAudio(gameOverTrack);
    }

    public void LevelComplete() 
    {
    }
}
