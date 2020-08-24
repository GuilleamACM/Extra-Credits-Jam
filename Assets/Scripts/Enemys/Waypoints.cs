using UnityEngine;
using TinyGecko.Pathfinding2D;
using TMPro;

public class Waypoints : MonoBehaviour
{
    [SerializeField] private Structure _coreStructure;

    public static Structure CoreSructure { get; set; }

    private void Awake()
    {
        CoreSructure = _coreStructure;
    }
}
