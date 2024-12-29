using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulDrop : MonoBehaviour
{
    [Header("Soul Settings")]
    [SerializeField] private int _soulCount;
    [SerializeField] private GameObject _soulPrefab;

    public void DropSouls()
    {
        for (int i = 0; i < _soulCount; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.3f, 0.5f), 0);
            Vector3 spawnPoint = transform.position + randomOffset;

            Instantiate(_soulPrefab, spawnPoint, Quaternion.identity);
        }
    }
}