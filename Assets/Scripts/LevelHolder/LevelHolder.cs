using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelHolder : Singleton<LevelHolder>
{
    public string [] levelSceneNames;
    int levelIndex = 0;

    public bool SetCurrentLevelAsNextLevel() 
    {
        if (levelIndex + 1 < levelSceneNames.Length) 
        {
            levelIndex++;
            return true;
        }
        return false;
    }

    public void LoadCurrentLevel() 
    {
        SceneManager.LoadScene(levelSceneNames[levelIndex]);
    }

    public void ResetLevel()
    {
        levelIndex = 0;
    }
}
