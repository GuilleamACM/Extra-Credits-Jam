using System.Collections.Generic;
using UnityEngine;

namespace TinyGecko.Pathfinding2D
{
    class Structure : MonoBehaviour, IGridEntity
    {
        #region Fields
        [SerializeField] private Vector2Int _entitySize;
        [SerializeField] private Sprite structureSprite;
        #endregion Fields


        #region Properties
        public Vector2Int EntitySize { get => _entitySize; set => _entitySize = value; }
        public Vector3 WorldPos { get => transform.position; set => transform.position = value; }
        #endregion Properties


        #region MonoBehaviour Methods
        private void OnValidate()
        {
            GetComponent<SpriteRenderer>().sprite = structureSprite;
        }
        #endregion MonoBehaviour Methods
    }
}
