using System.Collections.Generic;
using UnityEngine;

namespace TinyGecko.Pathfinding2D
{
    enum DistanceFunction
    {
        Manhttan,
        Euclidean
    }

    public class Pathfinder 
    {
        #region Fields
        private WorldGrid _grid;
        #endregion Fields

        #region Constructor
        public Pathfinder(WorldGrid grid)
        {
            _grid = grid;
        }
        #endregion Constructor

        #region Methods
        public Queue<GridNode> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            GridNode startNode = _grid.WorldPosToGrid(startPos);
            GridNode targetNode = _grid.WorldPosToGrid(targetPos);

            if (targetNode == null || startNode == null)
            {
                return null;
            }

            Heap<GridNode> openSet = new Heap<GridNode>(_grid.CelCount);
            HashSet<GridNode> closedSet = new HashSet<GridNode>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                GridNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    var path = RetracePath(startNode, targetNode);
                    return path;
                }

                foreach (GridNode neighbour in _grid.GetNeighbours(currentNode))
                {
                    if (neighbour.occupied || closedSet.Contains(neighbour))
                        continue;

                    int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }

            return null;
        }

        public Queue<GridNode> RetracePath(GridNode startNode, GridNode endNode)
        {
            List<GridNode> path = new List<GridNode>();
            GridNode currentNode = endNode;
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();
            Queue<GridNode> pathQueue = new Queue<GridNode>(path);
            return pathQueue;
        }

        int GetDistance(GridNode nodeA, GridNode nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            return (dstX + dstY) * 3;
            ////return Mathf.Sqrt(dstX * dstX + dstY * dstY);

            //if (dstX > dstY)
            //    return 14 * dstY + 10 * (dstX - dstY);
            //return 14 * dstX + 10 * (dstY - dstX);
        }
        #endregion Methods
    }
}
