using TinyGecko.Pathfinding2D;
using UnityEngine;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerController : MonoBehaviour
{
    #region Fields
    public HotbarManager hotbar;
    private Structure _structureAtMouse;
    private PlayerStatus _playerStatus;
    #endregion Fields

    #region MonoBehaviour Methods
    private void Awake()
    {
        _playerStatus = GetComponent<PlayerStatus>();    

        Debug.Log($"Player Status");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StructureManager.Instance.SelectStructure(0);
            hotbar.SelectSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StructureManager.Instance.SelectStructure(1);
            hotbar.SelectSlot(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StructureManager.Instance.SelectStructure(2);
            hotbar.SelectSlot(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StructureManager.Instance.SelectStructure(3);
            hotbar.SelectSlot(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StructureManager.Instance.SelectStructure(4);
            hotbar.SelectSlot(4);
        }

        Structure lastStructureAtMouse = _structureAtMouse;
        _structureAtMouse = StructureManager.Instance.StructureAtPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        HighlightStructure(_structureAtMouse, lastStructureAtMouse);


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StructureManager.Instance.PlaceCurrentStructure(_playerStatus);

        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (StructureManager.Instance.StructureToPlace == null)
            {
                if (_structureAtMouse)
                    StructureManager.Instance.RemoveStructure(_structureAtMouse, _playerStatus);
            }
            else
                StructureManager.Instance.CancelPlacement();
        }
    }
    #endregion MonoBehaviour Methods

    #region Methods
    private void HighlightStructure(Structure curStructure, Structure prevStructure)
    {

        if(prevStructure)
            prevStructure.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        if (!curStructure)
            return;

        curStructure.gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
    }
    #endregion Methods
}
