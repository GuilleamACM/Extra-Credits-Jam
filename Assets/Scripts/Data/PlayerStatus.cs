using TinyGecko.Pathfinding2D;
using UnityEngine;

class PlayerStatus : MonoBehaviour
{
    #region Fields
    [SerializeField] int _totalMemory = 4096;
    [SerializeField] float usageOfCPU = 0;

    private int _usedMemory = 0;
    private int _blockedMemory = 0;
    private int _score = 0;
    #endregion Field


    #region Properties
    public int UsedMemory 
    {
        get => _usedMemory;
        set
        {
            _usedMemory += value;
            if (_usedMemory <= 0)
                _usedMemory = 0;

            if (LooseCondition())
            {
                // TODO: Call GameManager.GameOver
            }
        }
    }
    public int BlockedMemory
    {
        get => _blockedMemory;
        set
        {
            _blockedMemory += value;
            if (_blockedMemory <= 0)
                _blockedMemory = 0;

            if (LooseCondition())
            {
                // TODO: Call GameManager.GameOver
            }
        }
    }

    public int Score { get => _score; set => _score = value; }
    #endregion Properties


    #region MonoBehaviour Methods
    private void Update()
    {
        
    }
    #endregion MonoBehaviour Method


    #region Methods
    private bool LooseCondition()
    {
        return _usedMemory + _blockedMemory >= _totalMemory;
    }

    public bool WillLooseIfPlaced(Structure structure)
    {
        return _usedMemory + _blockedMemory + structure.memoryCost >= _totalMemory;
    }
    #endregion Methods

}
