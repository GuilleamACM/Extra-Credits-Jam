
using UnityEngine;
using System.Collections.Generic;

namespace TinyGecko.Pathfinding2D
{
    class FinderUnit : MonoBehaviour
    {
        #region Fields
        Queue<GridNode> grid;
        #endregion Fields

        #region MonoBehaviour Methods
        private void Update()
        {
            grid = WorldGrid.Instance.Pathfinder.FindPath(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            float celSize = WorldGrid.Instance.CelSize;
            if (grid != null)
            {
                foreach (GridNode n in grid)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawCube(n.worldPos + new Vector3(0,0,-1), new Vector3(celSize - 0.05f, celSize - 0.05f, 2f));
                }
            }
        }
        #endregion MonoBehaviour Methods
    }
}
