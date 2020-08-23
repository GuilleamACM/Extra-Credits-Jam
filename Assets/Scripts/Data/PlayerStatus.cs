﻿using TinyGecko.Pathfinding2D;
using UnityEngine;

class PlayerStatus : MonoBehaviour
{
    #region Fields
    [SerializeField] int _totalMemory = 4096;
    [SerializeField] float usageOfCPU = 0;
    [SerializeField] int initialBlockedMemory;

    [SerializeField]private int _usedMemory = 0;
    [SerializeField]private int _blockedMemory = 0;
    private int _score = 0;
    #endregion Field

    #region Singleton
    public static PlayerStatus Instance;
    void Awake() 
    {
        if (!Instance)
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
    #endregion Singleton

    #region Properties
    public int UsedMemory 
    {
        get => _usedMemory;
        set
        {
            _usedMemory = value;
            if (_usedMemory <= 0)
                _usedMemory = 0;

            if (LooseCondition())
            {
                GameManager.Instance.GameOver();
            }
        }
    }
    public int BlockedMemory
    {
        get => _blockedMemory;
        set
        {
            _blockedMemory = value;
            if (_blockedMemory <= 0)
                _blockedMemory = 0;

            if (LooseCondition())
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    public int Score { get => _score; set => _score = value; }
    #endregion Properties


    #region Methods
    private bool LooseCondition()
    {
        return _usedMemory + _blockedMemory > _totalMemory;
    }

    public bool WillLooseIfPlaced(Structure structure)
    {
        return _usedMemory + _blockedMemory + structure.memoryCost > _totalMemory;
    }
    #endregion Methods

}
