using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestGameMode : MonoBehaviour, IGameController
{
    public float objectivesCompleted { get; set; }
    public float objectivesTotal { get; set; }
    public float time { get; set; }
    public bool isGameRunning { get; set; }
    public int difficulty;
    public bool extractionReady { get; set; }
    public float spawningInterval;
    float lastSpawn;
    void Start()
    {
        difficulty = PlayerPrefs.GetInt("difficulty");
        StartGame();
    }

    public void StartGame()
    {
        GetComponent<MazeGenerator>().GenerateMaze(difficulty);
        GetComponent<Spawning>().Spawn(difficulty, GetComponent<MazeGenerator>().size);
        objectivesCompleted = 0;
        isGameRunning = true;
    }

    void Update()
    {
        if (!isGameRunning) return;
        time += Time.deltaTime;
        if (objectivesCompleted == objectivesTotal) extractionReady = true; else extractionReady = false;
        if(time > lastSpawn + spawningInterval / difficulty)
        {
            lastSpawn = time;
            GetComponent<Spawning>().spawnCreature(GetComponent<MazeGenerator>().size);
        }
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
