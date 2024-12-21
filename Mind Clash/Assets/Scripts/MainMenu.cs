using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // Метод для запуска игры
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene"); // Загружаем основную сцену игры
    }

    // Метод для выхода из игры
    public void QuitGame()
    {
        Debug.Log("Игра завершена");
        Application.Quit(); // Закрываем игру
    }
}
