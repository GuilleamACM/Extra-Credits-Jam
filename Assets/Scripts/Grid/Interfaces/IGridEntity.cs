using System.Collections.Generic;
using UnityEngine;

namespace TinyGecko.Pathfinding2D
{
    public interface IGridEntity // Entities that can occupy a WorldGrid
    {
        Vector2Int EntitySize { get; set; }
        Vector3 WorldPos { get; set; }
    }
}
