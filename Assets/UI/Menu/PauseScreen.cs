using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public GameObject screen;


    public void Trigger()
    {
        if (screen.activeSelf) Hide(); else Show();
    }
    public void Show()
    {
        Cursor.visible = true;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<IGameController>().isGameRunning = false;
        screen.SetActive(true);
    }

    public void Hide()
    {
        Cursor.visible = false;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<IGameController>().isGameRunning = true;
        screen.SetActive(false);
    }

    public void onSettings()
    {

    }

    public void onQuit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
