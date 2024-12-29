using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;   // Панель паузы
    public GameObject HUD;             // HUD объекта
    private bool isPaused = false;     // Состояние паузы

    public bool IsPaused => isPaused;

    void Update()
    {
        // Проверка нажатия ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Включаем или выключаем паузу
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Включаем паузу
    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        HUD.SetActive(false); // Выключаем HUD
        Time.timeScale = 0f;  // Ставим игру на паузу
        isPaused = true;
    }

    // Возобновляем игру
    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        HUD.SetActive(true); // Включаем HUD
        Time.timeScale = 1f; // Снимаем паузу
        isPaused = false;
    }

    // Выход в главное меню
    public void ExitToMainMenu()
    {
        HUD.SetActive(true); // Включаем HUD перед выходом
        Time.timeScale = 1f; // Снимаем паузу
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }
}

