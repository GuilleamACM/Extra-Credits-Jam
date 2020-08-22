using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TinyGecko.Grid2D
{
    class StructureManager : MonoBehaviour
    {
        #region Fields
        [SerializeField] private List<GameObject> _structuresPrefabs;
        [SerializeField] private Structure _dummyStructure; // To test stuff
        #endregion Fields


        #region Properties
        public Structure StructureToPlace { get => _dummyStructure; set => _dummyStructure = value; }
        #endregion Properties
        List<GridNode> nodes;


        #region MonoBehaviour Methods
        private void Update()
        {
            if(_dummyStructure != null)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos = new Vector3(pos.x, pos.y, 0);
                _dummyStructure.transform.position = pos;

                bool canPlace = CanPlaceStructure(_dummyStructure);
                if (canPlace && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    PlaceStructure(_dummyStructure);
                    _dummyStructure = null;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if(nodes != null)
            {
                Gizmos.color = Color.red;
                foreach (var node in nodes)
                {
                    Gizmos.DrawCube(node.worldPos, new Vector3((float)WorldGrid.Instance.CelSize - 0.05f, (float)WorldGrid.Instance.CelSize - 0.05f, 0f));
                }
            }
        }
        #endregion MonoBehaviour Methods


        #region Methods
        public List<GridNode> OverlappingGrids(Structure structure) 
        {
            Vector2Int bounds = structure.EntitySize;
            Vector3 offset = WorldGrid.Instance.ToCenterOffset;
            Vector3 origin = structure.WorldPos + new Vector3(-bounds.x * WorldGrid.Instance.CelSize / 2.0f, bounds.y * WorldGrid.Instance.CelSize / 2.0f, 0);

            List<GridNode> occupyingNodes = new List<GridNode>();
            for (int y = 0; y < bounds.y; y++) 
            {
                for(int x = 0; x < bounds.x; x++)
                {
                    Vector3 pos = origin + offset + new Vector3(WorldGrid.Instance.CelSize * x, -WorldGrid.Instance.CelSize*y);
                    GridNode node = WorldGrid.Instance.LocalPosToGrid(WorldGrid.Instance.WorldToLocal(pos));
                    if (node != null && !node.occupied)
                        occupyingNodes.Add(node);
                }
            }

            return occupyingNodes;
        }

        public bool CanPlaceStructure(Structure structure)
        {
            nodes = OverlappingGrids(structure);
            foreach(var node in nodes)
            {
                if (node.occupied)
                    return false;
            }

            return nodes.Count == structure.EntitySize.x * structure.EntitySize.y;
        }

        public void PlaceStructure(Structure structure)
        {
            Vector3 center = Vector3.zero;
            foreach (var node in nodes)
            {
                node.occupied = true;
                center += node.worldPos;
            }
            center /= nodes.Count;
            structure.gameObject.transform.position = center;
        }
        #endregion Methods
    }
}
