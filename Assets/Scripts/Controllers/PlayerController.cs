using System.Collections;
using System.Collections.Generic;
using TinyGecko.Pathfinding2D;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields
    private Structure _structureAtMouse;
    #endregion Fields

    #region MonoBehaviour Methods
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StructureManager.Instance.SelectStructure(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StructureManager.Instance.SelectStructure(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StructureManager.Instance.SelectStructure(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StructureManager.Instance.SelectStructure(3);
        }

        Structure lastStructureAtMouse = _structureAtMouse;
        _structureAtMouse = StructureManager.Instance.StructureAtPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        HighlightStructure(_structureAtMouse, lastStructureAtMouse);


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            bool result = StructureManager.Instance.PlaceCurrentStructure();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (StructureManager.Instance.StructureToPlace == null)
            {
                if (_structureAtMouse)
                    StructureManager.Instance.RemoveStructure(_structureAtMouse);
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

        curStructure.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }
    #endregion Methods
}
