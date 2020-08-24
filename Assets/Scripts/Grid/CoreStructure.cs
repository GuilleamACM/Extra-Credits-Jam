using System.Collections.Generic;
using TinyGecko.Pathfinding2D;
using UnityEngine;

[RequireComponent(typeof(Structure))]
public class CoreStructure : MonoBehaviour
{
    private void Start()
    {
        var structure = GetComponent<Structure>();
        Vector2Int bounds = structure.EntitySize;
        Vector3 offset = WorldGrid.Instance.ToCenterOffset;
        Vector3 origin = structure.WorldPos + new Vector3(-bounds.x * WorldGrid.Instance.CelSize / 2.0f, bounds.y * WorldGrid.Instance.CelSize / 2.0f, 0);

        List<GridCel> occupyingCels = new List<GridCel>();
        for (int y = 0; y < bounds.y; y++)
        {
            for (int x = 0; x < bounds.x; x++)
            {
                Vector3 pos = origin + offset + new Vector3(WorldGrid.Instance.CelSize * x, -WorldGrid.Instance.CelSize * y);
                GridCel cel = WorldGrid.Instance.LocalPosToGrid(WorldGrid.Instance.WorldToLocal(pos));
                if (cel != null)
                    occupyingCels.Add(cel);
            }
        }

        structure.OccupyingCels = occupyingCels;

        foreach(var cel in occupyingCels)
        {
            cel.celState = GridCelState.NotWalkable;
        }
    }
}
