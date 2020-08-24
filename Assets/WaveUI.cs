using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] WaveSpawner spawner;

    TextMeshProUGUI text;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        if(spawner)
            text.text = $"Current wave: {spawner.waveIndex+1}/{spawner.waves.Length}";
    }
}
