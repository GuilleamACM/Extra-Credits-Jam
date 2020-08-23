using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

namespace TinyGecko.Pathfinding2D
{
    class StructureManager : MonoBehaviour
    {
        #region Singleton
        private static StructureManager _instance;

        private void Awake()
        {
            if (!_instance)
            {
                Instance = this;
                _placedStructures = new List<Structure>();
            }
            else
            {
                Destroy(this);
                return;
            }

        }
        #endregion Singleton


        #region Fields
        [SerializeField] private List<GameObject> _structuresPrefabs;
        [SerializeField] private Color _validPlace = new Color(1.0f, 1.0f, 1.0f, 0.7f);
        [SerializeField] private Color _invalidPlace = new Color(0.6f, 0.1f, 0.0f, 0.7f);
        [SerializeField] List<GridCelState> _validPlacementStates = new List<GridCelState>();
        [SerializeField] private GameObject _placementFXPrefab;
        [SerializeField] private GameObject _destroyFXPrefab;
        private List<Structure> _placedStructures;
        private Structure _structureToPlace;
        private PlayerStatus status;
        #endregion Fields


        #region Properties
        public Structure StructureToPlace { get => _structureToPlace; set => _structureToPlace = value; }
        public List<Structure> PlacedStructures { get => _placedStructures; }
        public static StructureManager Instance { get => _instance; private set => _instance = value; }
        #endregion Properties


        #region MonoBehaviour Methods
        private void Start()
        {
            this.status = GameObject.FindObjectOfType<PlayerStatus>();
        }

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
                UpdateStructureToPlacePosition();

                var canPlace = CanPlaceStructure(StructureToPlace,status);
                if (canPlace.Item1)
                    StructureToPlace.gameObject.GetComponent<SpriteRenderer>().color = _validPlace;
                else
                    StructureToPlace.GetComponent<SpriteRenderer>().color = _invalidPlace;
            }
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
        private Tuple<bool, List<GridCel>> CanPlaceStructure(Structure structure, PlayerStatus status)
        {
            var cels = OverlappingGrids(structure);
            foreach(var cel in cels)
            {
                if (!IsCelValidForPlacement(cel))
                    return Tuple.Create(false, cels);
            }

            return Tuple.Create(cels.Count == structure.EntitySize.x * structure.EntitySize.y && !status.WillLooseIfPlaced(structure), cels);
        }

        /// <summary>
        /// Function to place a structure on the grid
        /// </summary>
        /// <param name="structure">The structure to be placed</param>
        /// <param name="cels">The GridCels to receive the structure</param>
        private void PlaceStructure(Structure structure, List<GridCel> cels)
        {
            Vector3 center = Vector3.zero;
            foreach (var cel in cels)
            {
                cel.celState = GridCelState.Occupied;
                center += cel.worldPos;
            }
            center /= cels.Count;
            structure.gameObject.transform.position = center;
            structure.OccupyingCels = cels;
            _placedStructures.Add(structure);

            Transform structureTransform = StructureToPlace.transform;
            structure.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            StructureToPlace = null;
            var go = Instantiate(_placementFXPrefab);
            go.transform.position = structureTransform.position;

            Sequence seq = DOTween.Sequence();
            seq.Append(structureTransform.DOScale(new Vector3(1.25f, 0.75f, 1.0f), 0.075f));
            seq.Append(structureTransform.DOScale(new Vector3(0.85f, 1.15f, 1.0f), 0.075f));
            seq.Append(structureTransform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.075f));
        }

        /// <summary>
        /// Function to try placing the current structure to place 
        /// on the grid
        /// </summary>
        /// <returns>True if the structure was placed, false otherwise</returns>
        public bool PlaceCurrentStructure(PlayerStatus status)
        {
            if (!StructureToPlace)
                return false;

            var canPlace = CanPlaceStructure(StructureToPlace, status);
            if (!canPlace.Item1)
                return false;

            status.UsedMemory += StructureToPlace.memoryCost;
            StructureToPlace.GetComponent<Tower>().stalled = false;
            PlaceStructure(StructureToPlace, canPlace.Item2);
            return true;
        }

        public void CancelPlacement()
        {
            if (StructureToPlace)
                Destroy(StructureToPlace.gameObject);

            StructureToPlace = null;
        }

        /// <summary>
        /// Function to remove a structure
        /// </summary>
        /// <param name="structure">The structure to be removed</param>
        /// <returns>True if structure was removed. False otherwise</returns>
        public bool RemoveStructure(Structure structure, PlayerStatus status)
        {
            if (_placedStructures.Contains(structure))
            {
                foreach(var cel in structure.OccupyingCels)
                    cel.celState = GridCelState.Free;

                var go = Instantiate(_destroyFXPrefab);
                go.transform.position = structure.transform.position;

                Sequence seq = DOTween.Sequence();
                seq.Append(structure.transform.DOScale(new Vector3(1.15f, 0.9f, 1.0f), 0.075f));
                seq.Append(structure.transform.DOScale(new Vector3(1.35f, 1.25f, 1.0f), 0.075f));
                seq.Append(structure.transform.DOScale(new Vector3(0.5f, 0.5f, 1.0f), 0.035f));
                seq.AppendCallback(() => {
                    Destroy(structure.gameObject);
                });

                status.UsedMemory -= structure.memoryCost;
                return _placedStructures.Remove(structure);
            }

            return false;
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
            if (index < 0 || index >= _structuresPrefabs.Count)
                return;
            if (_structuresPrefabs[index] == null)
                return;

            if (StructureToPlace != null)
                Destroy(StructureToPlace.gameObject);

            GameObject structure = Instantiate(_structuresPrefabs[index]);
            StructureToPlace = structure.GetComponent<Structure>();
            UpdateStructureToPlacePosition();
        }

        void UpdateStructureToPlacePosition() 
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos = new Vector3(pos.x, pos.y, 0);
            StructureToPlace.transform.position = pos;
        }

        /// <summary>
        /// Funciton to check if there's a structure at around a given position
        /// </summary>
        /// <returns>The corresponding structure. Null if there isn't any</returns>
        public Structure StructureAtPosition(Vector3 pos)
        {
            foreach(var structure in _placedStructures)
            {
                bool result = StructureInsideRegion(structure, pos);
                if (result)
                    return structure;
            }
            return null;
        }

        /// <summary>
        /// Helper function to verify if a structure is inside a region
        /// </summary>
        /// <param name="structure">Structure ref</param>
        /// <param name="pos">Position to check</param>
        /// <returns>True if pos is inside structure bounds. False otherwise</returns>
        private bool StructureInsideRegion(Structure structure, Vector3 pos)
        {
            float celSize = WorldGrid.Instance.CelSize;
            float halfSizeX = structure.EntitySize.x * celSize / 2.0f;
            float halfSizeY = structure.EntitySize.y * celSize / 2.0f;
            float minX = structure.WorldPos.x - halfSizeX;
            float maxX = structure.WorldPos.x + halfSizeX;
            float minY = structure.WorldPos.y - halfSizeY;
            float maxY = structure.WorldPos.y + halfSizeY;

            return pos.x >= minX && pos.x <= maxX && pos.y >= minY && pos.y <= maxY;
        }

        #endregion Structure Selection Methods
    }
}
