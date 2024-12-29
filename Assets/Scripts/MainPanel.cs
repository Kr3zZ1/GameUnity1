using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Метод для выхода из игры
    public void QuitGame()
    {
        Application.Quit(); // Работает только в билде игры
    }

    // Метод для перехода на сцену правил   
    public void OpenRulesMenu()
    {
        SceneManager.LoadScene("RulesMenuScene");
    }

    // Метод для перехода на сцену настроек
    public void OpenSettingsMenu()
    {
        SceneManager.LoadScene("SettingsMenuScene");
    }

    // Метод для перехода на основную сцену
    public void OpenSampleScene(int SampleScene )
    {
        SceneManager.LoadScene("FirstLevel");   
    }    
    
    // Метод для перехода на сцену главного меню
    public void OpenMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
}