using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector3 _lastCheckpointPosition;
    private float _playerMaxHealth;
    private float _playerHealth;
    private int _playerSoulsCount;
    private Inventory _inventory;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void SetCheckpoint(Vector3 position)
    {
        _lastCheckpointPosition = position;

        PlayerHealthSystem playerHealthSystem = FindObjectOfType<PlayerHealthSystem>();
        PlayerSouls playerSouls = FindObjectOfType<PlayerSouls>();

        _playerMaxHealth = playerHealthSystem.MaxHealthPoints;
        _playerHealth = playerHealthSystem.CurrentHealthPoints;
        _playerSoulsCount = playerSouls.GetSouls();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayerHealthSystem player = FindObjectOfType<PlayerHealthSystem>();
        player.Respawn();
    }

    public Vector3 GetCheckpointPosition() => _lastCheckpointPosition;
    public float GetSavedMaxHealth() => _playerMaxHealth;
    public float GetSavedCurrentHealth() => _playerHealth;
    public int GetSavedSoulsCount() => _playerSoulsCount;
}