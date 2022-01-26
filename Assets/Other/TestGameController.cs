using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestGameController : MonoBehaviour, IGameController
{
    public float objectivesCompleted { get; set; }
    public float objectivesTotal { get; set; }
    public float time { get; set; }
    public bool isGameRunning { get; set; }
    public int difficulty;
    public bool extractionReady { get; set; }
    float lastSpawn;
    public bool win;
    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        objectivesCompleted = 0;
        isGameRunning = true;
    }

    void Update()
    {
        if (!isGameRunning) return;
        time += Time.deltaTime;
        if(win) EndGame();
    }

    public void EndGame()
    {
        PlayerPrefs.SetFloat("mapTime", Mathf.Round(GetComponent<HUD>().mapTime));
        PlayerPrefs.SetFloat("time", Mathf.Round(time));
        PlayerPrefs.SetFloat("objectivesCount", objectivesCompleted);
        PlayerPrefs.SetFloat("objectivesTotal", objectivesTotal);
        isGameRunning = false;
        Debug.Log("Game completed in " + time + " seconds!");
        SceneManager.LoadScene("EndScreen");
    }
}