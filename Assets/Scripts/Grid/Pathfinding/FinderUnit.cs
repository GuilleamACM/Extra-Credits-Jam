
using UnityEngine;
using System.Collections.Generic;

namespace TinyGecko.Pathfinding2D
{
    /// <summary>
    /// Unit that finds a path to a target in the world grid
    /// </summary>
    class FinderUnit : MonoBehaviour
    {
        #region Fields
        Queue<GridCel> grid;
        #endregion Fields

        #region MonoBehaviour Methods
        private void Update()
        {
            if (_tempRef && _tempRef.PlacedStructures.Count > 0)
            {
                grid = WorldGrid.Instance.Pathfinder.FindPath(transform.position, _tempRef.PlacedStructures[0]);
            }
        }

        [SerializeField] StructureManager _tempRef;

        private void OnDrawGizmos()
        {
            if (!WorldGrid.Instance)
                return;

            Gizmos.color = Color.gray;
            float celSize = WorldGrid.Instance.CelSize;
            if (grid != null)
            {
                foreach (GridCel n in grid)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawCube(n.worldPos + new Vector3(0,0,-1), new Vector3(celSize - 0.05f, celSize - 0.05f, 2f));
                }
            }
        }
        #endregion MonoBehaviour Methods
    }
}
