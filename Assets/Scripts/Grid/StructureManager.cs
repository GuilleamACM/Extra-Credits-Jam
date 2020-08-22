using System.Collections.Generic;
using UnityEngine;
using System;

namespace TinyGecko.Pathfinding2D
{
    class StructureManager : MonoBehaviour
    {
        #region Fields
        [SerializeField] private List<GameObject> _structuresPrefabs;
        [SerializeField] private Color _validPlace = new Color(1.0f, 1.0f, 1.0f, 0.7f);
        [SerializeField] private Color _invalidPlace = new Color(0.6f, 0.1f, 0.0f, 0.7f);
        private List<Structure> _placedStructures;
        private Structure _structureToPlace;
        #endregion Fields


        #region Properties
        public Structure StructureToPlace { get => _structureToPlace; set => _structureToPlace = value; }
        #endregion Properties


        #region MonoBehaviour Methods
        private void Update()
        {
            // Select Structures
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SelectStructure(0);
            }

            // Place Structures on Mouse Down
            if(StructureToPlace != null)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos = new Vector3(pos.x, pos.y, 0);
                StructureToPlace.transform.position = pos;

                var canPlace = CanPlaceStructure(StructureToPlace);
                if (canPlace.Item1)
                {
                    StructureToPlace.gameObject.GetComponent<SpriteRenderer>().color = _validPlace;
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                        PlaceStructure(StructureToPlace, canPlace.Item2);
                }
                else
                    StructureToPlace.GetComponent<SpriteRenderer>().color = _invalidPlace;
            }
        }

        private void Awake()
        {
            _placedStructures = new List<Structure>();
        }
        #endregion MonoBehaviour Methods


        #region Structure Placement Methods

        /// <summary>
        ///  Function to get the grids a structure is overlapping
        /// </summary>
        /// <param name="structure">The structure to check the overlaps</param>
        /// <returns>A list of GridNodes that are being overlapped</returns>
        private List<GridNode> OverlappingGrids(Structure structure) 
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

        /// <summary>
        /// Function to check if a given structure can be placed
        /// </summary>
        /// <param name="structure">The structure to be checked</param>
        /// <returns>
        /// A tuple containing a bool to know if it can be placed an the nodes 
        /// that the structure is/will occupy
        /// </returns>
        public Tuple<bool, List<GridNode>> CanPlaceStructure(Structure structure)
        {
            var nodes = OverlappingGrids(structure);
            foreach(var node in nodes)
            {
                if (node.occupied)
                    return Tuple.Create(false, nodes);
            }

            return Tuple.Create(nodes.Count == structure.EntitySize.x * structure.EntitySize.y, nodes);
        }

        /// <summary>
        /// Function to place a structure on the grid
        /// </summary>
        /// <param name="structure">The structure to be placed</param>
        /// <param name="nodes">The GridNodes to receive the structure</param>
        public void PlaceStructure(Structure structure, List<GridNode> nodes)
        {
            Vector3 center = Vector3.zero;
            foreach (var node in nodes)
            {
                node.occupied = true;
                center += node.worldPos;
            }
            center /= nodes.Count;
            structure.gameObject.transform.position = center;
            _placedStructures.Add(structure);

            structure.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            StructureToPlace = null;
        }
        #endregion Structure Placement Methods


        #region Structure Selection Methods

        /// <summary>
        /// Function to select a struction from the structure
        /// prefabs and prepare it to be placed
        /// </summary>
        /// <param name="index">index of the structure on the list</param>
        public void SelectStructure(int index)
        {
            if (index < 0 && index >= _structuresPrefabs.Count)
                return;
            if (_structuresPrefabs[index] == null)
                return;

            if (StructureToPlace != null)
                Destroy(StructureToPlace.gameObject);

            GameObject structure = Instantiate(_structuresPrefabs[index]);
            StructureToPlace = structure.GetComponent<Structure>();
        }
        #endregion Structure Selection Methods
    }
}
