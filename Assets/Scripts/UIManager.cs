using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private GameObject _deathScreen;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _deathScreen.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        _deathScreen.SetActive(true);
    }

    public void HideDeathScreen()
    {
        _deathScreen.SetActive(false);
    }

    public void OnRestartButtonPressed()
    {
        RestartLevel();
    }

    public void RestartLevel()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayerHealthSystem player = FindObjectOfType<PlayerHealthSystem>();
        if (player != null)
        {
            player.Respawn();
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}