using UnityEngine;

namespace TinyGecko.Pathfinding2D
{
    [ExecuteInEditMode]
    public class WorldGrid : MonoBehaviour
    {
        #region Singleton
        private static WorldGrid _instance;
        public static WorldGrid Instance { get => _instance; private set => _instance = value; }
        #endregion Singleton


        #region Fields
        [SerializeField] float _celSize = 1;
        [SerializeField] Vector2Int _gridBounds = new Vector2Int(128, 128);

        private GridNode[,] _grid;
        #endregion Fields


        #region Properties

        /// <summary>
        /// Property to get the Grid Origin position, which is the top-left corner
        /// </summary>
        public Vector3 GridOrigin
        {
            get => transform.position + new Vector3(-_gridBounds.x * _celSize/2.0f, _gridBounds.y * _celSize/2.0f, 0);
        }

        public Vector2 GridBounds { get => new Vector2(_celSize*_gridBounds.x, _celSize*_gridBounds.y); }

        /// <summary>
        /// Function to offset the topleft position of a cell to the center
        /// </summary>
        public Vector3 ToCenterOffset { get => new Vector3((float)_celSize / 2.0f, -(float)_celSize / 2.0f, 0); }

        public float CelSize { get => _celSize; }
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
        }

        private void OnValidate()
        {
            _grid = new GridNode[_gridBounds.x, _gridBounds.y];

            for (int y = 0; y < _gridBounds.y; y++)
            {
                for (int x = 0; x < _gridBounds.x; x++)
                {
                    Vector3 nodePos = GridOrigin + ToCenterOffset + new Vector3(_celSize * x, -_celSize * y);
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
            }
        }
        #endregion MonoBehaviour Methods


        #region Methods

        /// <summary>
        /// Function to verify if a grid position is valid(nothing is occupying it)
        /// durin grid construction
        /// </summary>
        /// <param name="nodePos">The grid node position</param>
        /// <returns>True if there's no collider at this position. False otherwise</returns>
        private bool VerifyPosition(Vector3 nodePos)
        {
            return false;
        }

        /// <summary>
        /// Function to convert a local grid position to a grid node
        /// </summary>
        /// <param name="localPos">Local position coordinates</param>
        /// <returns>The corresponding grid or null if it wasn't a valid position</returns>
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

        /// <summary>
        /// Convert a world coordinates to  grid coordinates
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Vector3 WorldToLocal(Vector3 pos)
        {
            Vector3 localPos = pos - GridOrigin;
            localPos = new Vector3(localPos.x, -localPos.y, localPos.z);
            return localPos;
        }

        /// <summary>
        /// Function to convert the mouse position on screen to local grid position
        /// </summary>
        /// <returns>The mouse position in grid coordinates</returns>
        public Vector3 MouseToLocal()
        {
            return WorldToLocal(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        #endregion Methods
    }
}
