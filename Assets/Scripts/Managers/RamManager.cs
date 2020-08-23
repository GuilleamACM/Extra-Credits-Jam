using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class RamManager : MonoBehaviour
{
    #region Singleton
    public static RamManager Instance;
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

    [Header("Bars")]
    public Image blockedRamBar;
    public Image usedRamBar;

    [Header("Texts")]
    public TextMeshProUGUI totalMemoryText;
    public TextMeshProUGUI freeRamText;
    public TextMeshProUGUI usedRamText;
    public TextMeshProUGUI blockedRamText;

    public void UpdateBars(int totalMemory, int usedMemory, int blockedMemory)
    {
        float usedMemoryFill = (float)usedMemory / totalMemory;
        float blockedMemoryFill = (float)blockedMemory / totalMemory;
        usedRamBar.DOFillAmount(usedMemoryFill, 0.25f).Play();
        blockedRamBar.DOFillAmount(blockedMemoryFill, 0.25f).Play();

        UpdatePercentages(usedMemoryFill, blockedMemoryFill);
    }

    public void UpdatePercentages(float usedMemoryPercentage, float blockedMemoryPercentage)
    {
        int usedPercentage = (int)(usedMemoryPercentage * 100);
        int blockedPercentage = (int)(blockedMemoryPercentage * 100);
        int totalMemoryPercentage = usedPercentage + blockedPercentage;
        int freeMemoryPercentage = 100 - totalMemoryPercentage;
        totalMemoryText.text = "Memory Usage: " + totalMemoryPercentage  + "%";
        freeRamText.text = "Free Ram: " + freeMemoryPercentage + "%";
        usedRamText.text = "Used Ram: " + usedPercentage + "%";
        blockedRamText.text = "Blocked Ram: " + blockedPercentage + "%";
    }
}
