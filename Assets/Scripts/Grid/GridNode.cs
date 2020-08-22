using UnityEngine;
using TinyGecko.Grid2D.Pathfinding;

namespace TinyGecko.Grid2D
{
    public class GridNode : IHeapItem<GridNode>
    { 
        #region Fields
        public bool occupied = false;
        public Vector3 worldPos; // Center of grid node
        public int gridX, gridY;

        public int gCost;
        public int hCost;
        public int fCost { get => gCost + hCost; }
        public int HeapIndex { get; set; }
        #endregion Fields


        #region Constructors
        public GridNode(bool occupied, Vector3 wPos, int x, int y)
        {
            this.occupied = occupied;
            worldPos = wPos;
            gridX = x;
            gridY = y;

            gCost = hCost = 0;
        }
        #endregion Constructors


        #region Methods
        public int CompareTo(GridNode other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(other.hCost);
            }

            return -compare;
        }
        #endregion Methods
    }
}
