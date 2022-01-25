using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Screens")]
    public GameObject startScreen;
    public GameObject quitPopup;
    public GameObject settings;
    [Header("StartGamePopup")]
    public TextMeshProUGUI difficultyDisplay;
    public Slider difficulty;

    private void Start()
    {
        difficulty.value = PlayerPrefs.GetInt("difficulty");
        PlayerPrefs.SetFloat("objectivesCount", 0);
        PlayerPrefs.SetFloat("objectivesTotal", 0);
        PlayerPrefs.SetFloat("enemyKills", 0);
        PlayerPrefs.SetFloat("mapTime", 0);
        PlayerPrefs.SetFloat("shotsFired", 0);
        PlayerPrefs.SetFloat("difficulty", 0);
    }
    void Update()
    {
        difficultyDisplay.text = difficulty.value.ToString();
    }

    public void onStartGame()
    {
        if (!quitPopup.activeSelf)
        {
            startScreen.SetActive(true);
            settings.SetActive(false);
        }
    }

    public void onPlay()
    {
        PlayerPrefs.SetInt("difficulty", (int) difficulty.value);
        SceneManager.LoadScene("GameScene");
    }

    public void onSettings()
    {
        if (!quitPopup.activeSelf)
        {
            startScreen.SetActive(false);
            settings.SetActive(true);
        }
    }

    public void OnQuit()
    {
        if (!quitPopup.activeSelf)
            quitPopup.SetActive(true);
    }

    public void onYes()
    {
        Application.Quit();
    }

    public void onNo()
    {
        quitPopup.SetActive(false);
    }
}