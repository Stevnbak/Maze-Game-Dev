using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        if (PlayerPrefs.GetString("state") == "win") winState.SetActive(true); else loseState.SetActive(false);
        //Time
        float time = PlayerPrefs.GetFloat("time");
        float bestTime = PlayerPrefs.GetFloat("bestTime", time);
        Debug.Log(bestTime);
        if (time < bestTime) bestTime = time;
        PlayerPrefs.SetFloat("bestTime", bestTime);
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

    public void OnPlay()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnExit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
