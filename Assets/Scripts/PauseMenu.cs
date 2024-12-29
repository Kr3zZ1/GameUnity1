using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;   // ������ �����
    public GameObject HUD;             // HUD �������
    private bool isPaused = false;     // ��������� �����

    public bool IsPaused => isPaused;

    void Update()
    {
        // �������� ������� ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �������� ��� ��������� �����
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

    // �������� �����
    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        HUD.SetActive(false); // ��������� HUD
        Time.timeScale = 0f;  // ������ ���� �� �����
        isPaused = true;
    }

    // ������������ ����
    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        HUD.SetActive(true); // �������� HUD
        Time.timeScale = 1f; // ������� �����
        isPaused = false;
    }

    // ����� � ������� ����
    public void ExitToMainMenu()
    {
        HUD.SetActive(true); // �������� HUD ����� �������
        Time.timeScale = 1f; // ������� �����
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }
}

