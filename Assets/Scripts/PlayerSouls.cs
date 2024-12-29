using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSouls : MonoBehaviour
{
    [Header("Souls Settings")]
    [SerializeField] private int _currentSouls;
    [SerializeField] private SoulsUI _soulsUI;

    public void AddSoul(int amount)
    {
        _currentSouls += amount;
        _soulsUI.UpdateSouls(_currentSouls);
    }

    public int GetSouls() { return _currentSouls; }

    public void SetSouls(int amount)
    {
        _currentSouls = amount;
        _soulsUI.UpdateSouls(_currentSouls);
    }

    public void SpendSouls(int amount)
    {
        _currentSouls -= amount;
        _soulsUI.UpdateSouls(_currentSouls);
    }
}