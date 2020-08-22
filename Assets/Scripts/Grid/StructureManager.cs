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
        [SerializeField] List<GridCelState> _validPlacementStates = new List<GridCelState>();
        private List<Structure> _placedStructures;
        private Structure _structureToPlace;
        #endregion Fields


        #region Properties
        public Structure StructureToPlace { get => _structureToPlace; set => _structureToPlace = value; }
        public List<Structure> PlacedStructures { get => _placedStructures; }
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
        /// <returns>A list of GridCels that are being overlapped</returns>
        private List<GridCel> OverlappingGrids(Structure structure) 
        {
            Vector2Int bounds = structure.EntitySize;
            Vector3 offset = WorldGrid.Instance.ToCenterOffset;
            Vector3 origin = structure.WorldPos + new Vector3(-bounds.x * WorldGrid.Instance.CelSize / 2.0f, bounds.y * WorldGrid.Instance.CelSize / 2.0f, 0);

            List<GridCel> occupyingCels = new List<GridCel>();
            for (int y = 0; y < bounds.y; y++) 
            {
                for(int x = 0; x < bounds.x; x++)
                {
                    Vector3 pos = origin + offset + new Vector3(WorldGrid.Instance.CelSize * x, -WorldGrid.Instance.CelSize*y);
                    GridCel cel = WorldGrid.Instance.LocalPosToGrid(WorldGrid.Instance.WorldToLocal(pos));
                    if (cel != null && IsCelValidForPlacement(cel))
                        occupyingCels.Add(cel);
                }
            }

            return occupyingCels;
        }

        /// <summary>
        /// Function to check if a given structure can be placed
        /// </summary>
        /// <param name="structure">The structure to be checked</param>
        /// <returns>
        /// A tuple containing a bool to know if it can be placed an the cels 
        /// that the structure is/will occupy
        /// </returns>
        public Tuple<bool, List<GridCel>> CanPlaceStructure(Structure structure)
        {
            var cels = OverlappingGrids(structure);
            foreach(var cel in cels)
            {
                if (!IsCelValidForPlacement(cel))
                    return Tuple.Create(false, cels);
            }

            return Tuple.Create(cels.Count == structure.EntitySize.x * structure.EntitySize.y, cels);
        }

        /// <summary>
        /// Function to place a structure on the grid
        /// </summary>
        /// <param name="structure">The structure to be placed</param>
        /// <param name="cels">The GridCels to receive the structure</param>
        public void PlaceStructure(Structure structure, List<GridCel> cels)
        {
            Vector3 center = Vector3.zero;
            foreach (var cel in cels)
            {
                cel.celState = GridCelState.Occupied;
                center += cel.worldPos;
            }
            center /= cels.Count;
            structure.gameObject.transform.position = center;
            structure.occupyingCels = cels;
            _placedStructures.Add(structure);

            structure.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            StructureToPlace = null;
        }
        #endregion Structure Placement Methods


        #region Placement Utiliy Functions
        public bool IsCelValidForPlacement(GridCel cel)
        {
            return _validPlacementStates.Contains(cel.celState);
        }
        #endregion Placement Utility Functions


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
