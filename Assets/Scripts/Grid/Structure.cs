using System.Collections.Generic;
using UnityEngine;

namespace TinyGecko.Pathfinding2D
{
    public class Structure : MonoBehaviour, IGridEntity
    {
        #region Fields
        [SerializeField] private Vector2Int _entitySize;
        #endregion Fields


        #region Properties
        public Vector2Int EntitySize { get => _entitySize; set => _entitySize = value; }
        public Vector3 WorldPos { get => transform.position; set => transform.position = value; }
        public List<GridCel> OccupyingCels { get; set; } // list of cels the structure is occupying
        public int memoryCost = 16;
        public bool isMine = false;
        #endregion Properties


        #region MonoBehaviour Methods
        #endregion MonoBehaviour Methods
    }
}
