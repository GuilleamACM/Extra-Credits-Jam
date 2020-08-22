using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace TinyGecko.Grid2D
{
    [ExecuteInEditMode]
    public class WorldGrid : MonoBehaviour
    {
        #region Singleton
        private static WorldGrid _instance;
        public static WorldGrid Instance { get => _instance; private set => _instance = value; }
        #endregion Singleton


        #region Fields
        [SerializeField] int _celSize = 32;
        [SerializeField] Vector2Int _gridBounds = new Vector2Int(128, 128);

        private GridNode[,] _grid;
        #endregion Fields


        #region Properties
        public Vector3 GridOrigin
        {
            get => transform.position + new Vector3(-_gridBounds.x * _celSize/2.0f, _gridBounds.y * _celSize/2.0f, 0);
        }

        public Vector2 GridBounds { get => new Vector2(_celSize*_gridBounds.x, _celSize*_gridBounds.y); }

        /// <summary>
        /// Function to offset the topleft position of a cell to the center
        /// </summary>
        public Vector3 ToCenterOffset { get => new Vector3((float)_celSize / 2.0f, -(float)_celSize / 2.0f, 0); }

        public int CelSize { get => _celSize; }
        #endregion Properties


        #region MonoBehaviour Methods
        private void Awake()
        {
            if (!_instance)
                Instance = this;
            else {
                Destroy(this);
                return;
            }


            _grid = new GridNode[_gridBounds.x, _gridBounds.y];

            for(int y = 0; y < _gridBounds.y; y++)
            {
                for(int x = 0; x < _gridBounds.x; x++)
                {
                    Vector3 nodePos = GridOrigin + ToCenterOffset + new Vector3(_celSize*x, -_celSize*y);
                    _grid[x, y] = new GridNode(VerifyPosition(nodePos), nodePos, x, y);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireCube(transform.position, new Vector3(_gridBounds.x * _celSize, _gridBounds.y * _celSize, 0));
            if (_grid != null)
            {
                foreach (GridNode n in _grid)
                {
                    Gizmos.color = n.occupied ? new Color(0.7f, 0f, 0f, 0.2f) : Color.white;
                    Gizmos.DrawCube(n.worldPos, new Vector3((float)_celSize - 0.05f, (float)_celSize - 0.05f, 0f));
                }

                GridNode g = LocalPosToGrid(MouseToLocal());
                Gizmos.color = Color.cyan;
                if(g != null)
                    Gizmos.DrawCube(g.worldPos, new Vector3((float)_celSize - 0.05f, (float)_celSize - 0.05f, 0f));

            }
        }
        #endregion MonoBehaviour Methods


        #region Methods
        public bool VerifyPosition(Vector3 pos)
        {
            return false;
        }

        public GridNode LocalPosToGrid(Vector3 localPos)
        {
            if (localPos.x > 0 && localPos.y > 0 && localPos.x <= GridBounds.x && localPos.y <= GridBounds.y)
            {
                Vector3 pos = localPos / _celSize;
                int x = (int)pos.x;// % _gridSize;
                int y = (int)pos.y;// % _gridSize;
                return _grid[x, y];
            }

            return null;
        }

        public Vector3 WorldToLocal(Vector3 pos)
        {
            Vector3 localPos = pos - GridOrigin;
            localPos = new Vector3(localPos.x, -localPos.y, localPos.z);
            return localPos;
        }

        public Vector3 MouseToLocal()
        {
            return WorldToLocal(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        #endregion Methods
    }
}
