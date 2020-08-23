using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject SettingsOverlay;
    public void OnPlay()
    {
        LevelHolder.Instance.LoadCurrentLevel();
    }

    public void OnSettings() 
    {
        SettingsOverlay.SetActive(true);
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
