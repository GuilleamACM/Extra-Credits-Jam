﻿using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Tower Defense/Wave")]
public class Wave : ScriptableObject
{
    public GameObject[] enemies;
    public float spawnRate;
}
