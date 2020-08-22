using System.Collections.Generic;
using UnityEngine;

namespace TinyGecko.Grid2D
{
    class Structure : MonoBehaviour, IGridEntity
    {
        #region Fields
        [SerializeField] private Vector2Int _entitySize;
        #endregion Fields


        #region Properties
        public Vector2Int EntitySize { get => _entitySize; set => _entitySize = value; }
        public Vector3 WorldPos { get => transform.position; set => transform.position = value; }
        #endregion Properties


        #region MonoBehaviour Methods
        private void Update()
        {
            
        }

        private void OnDrawGizmos()
        {
            //Gizmos.color = Color.red;
            //Gizmos.DrawWireCube(transform.position, new Vector3(_gridBounds.x * _celSize, _gridBounds.y * _celSize, 0));
            //if (_grid != null)
            //{
            //    foreach (GridNode n in _grid)
            //    {
            //        Gizmos.color = n.occupied ? new Color(0.7f, 0f, 0f, 0.2f) : Color.white;
            //        Gizmos.DrawCube(n.worldPos, new Vector3((float)_celSize - 0.2f, (float)_celSize - 0.2f, 0f));
            //    }
            //}
        }
        #endregion MonoBehaviour Methods


        #region Methods
        #endregion Methods
    }
}
