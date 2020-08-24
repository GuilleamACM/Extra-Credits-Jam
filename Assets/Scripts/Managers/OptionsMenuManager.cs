using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    public Toggle sfx;
    public Toggle bgm;
    private void Start()
    {
        sfx.isOn = AudioManager.Instance.IsSoundPlaying();
        bgm.isOn = AudioManager.Instance.IsMusicPlaying();
    }

    public void OnToggleBGM() 
    {
        bgm.isOn = AudioManager.Instance.ToggleMusic();
    }

    public void OnToggleSFX() 
    {
        sfx.isOn = AudioManager.Instance.ToggleSounds();
    }
}
