using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Tower Defense/Wave")]
public class Wave : ScriptableObject
{
    public Enemy[] enemies;
    public float spawnRate;
}
