using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // ����� ��� ������ �� ����
    public void QuitGame()
    {
        Application.Quit(); // �������� ������ � ����� ����
    }

    // ����� ��� �������� �� ����� ������   
    public void OpenRulesMenu()
    {
        SceneManager.LoadScene("RulesMenuScene");
    }

    // ����� ��� �������� �� ����� ��������
    public void OpenSettingsMenu()
    {
        SceneManager.LoadScene("SettingsMenuScene");
    }

    // ����� ��� �������� �� �������� �����
    public void OpenSampleScene(int SampleScene )
    {
        SceneManager.LoadScene("FirstLevel");   
    }    
    
    // ����� ��� �������� �� ����� �������� ����
    public void OpenMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
}