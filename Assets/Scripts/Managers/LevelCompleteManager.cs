using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteManager : MonoBehaviour
{
    public void OnNextLevel()
    {
        LevelHolder.Instance.SetCurrentLevelAsNextLevel();
        LevelHolder.Instance.LoadCurrentLevel();
    }
    public void OnPlayAgain()
    {
        LevelHolder.Instance.LoadCurrentLevel();
    }
    public void OnMainMenu()
    {
        GameManager.Instance.BackToMainMenu();
    }
}
