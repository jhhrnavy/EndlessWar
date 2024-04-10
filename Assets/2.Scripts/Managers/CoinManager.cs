using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : PersistentSingleton<CoinManager>
{
    [SerializeField] private int _coins = 0;

    public void AddCoins(int amount)
    {
        _coins += amount;
        Debug.Log($"Coins Added. Total Coins : {_coins}");
    }

    public int GetCoins()
    {
        return _coins;
    }
}
