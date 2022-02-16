using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [Header("States")]
    public GameObject winState;
    public GameObject loseState;

    [Header("Times")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI bestTimeText;

    [Header("Stats")]
    public TextMeshProUGUI objectives;
    public TextMeshProUGUI enemies;
    public TextMeshProUGUI mapTime;
    public TextMeshProUGUI shots;
    public TextMeshProUGUI difficulty;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadSceneAsync("MenuBackground", LoadSceneMode.Additive);
        if (PlayerPrefs.GetString("state") == "win") winState.SetActive(true); else loseState.SetActive(true);
        //Time
        float time = PlayerPrefs.GetFloat("time");
        float bestTime = PlayerPrefs.GetFloat("bestTime" + PlayerPrefs.GetInt("difficulty"), 0);
        if (PlayerPrefs.GetString("state") == "win")
        {
            if (time < bestTime || bestTime == 0)
            {
                bestTime = time;
                PlayerPrefs.SetFloat("bestTime" + PlayerPrefs.GetInt("difficulty"), bestTime);
            }
        }
        float minutes = Mathf.Floor(time / 60);
        float seconds = time - (minutes * 60);
        timeText.text = minutes + " minutes and " + seconds + " seconds";
        minutes = Mathf.Floor(bestTime / 60);
        seconds = bestTime - (minutes * 60);
        bestTimeText.text = minutes + " minutes and " + seconds + " seconds";
        //Stats
        objectives.text = PlayerPrefs.GetFloat("objectivesCount") + "/" + PlayerPrefs.GetFloat("objectivesTotal");
        enemies.text = PlayerPrefs.GetFloat("enemyKills").ToString();
        mapTime.text = PlayerPrefs.GetFloat("mapTime") + " sec";
        shots.text = PlayerPrefs.GetFloat("shotsFired").ToString();
        difficulty.text = PlayerPrefs.GetInt("difficulty").ToString();
    }

    private void Update()
    {
        InputSystem.Update();
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnExit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
