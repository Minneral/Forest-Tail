using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Укажите имя или индекс игровой сцены
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
