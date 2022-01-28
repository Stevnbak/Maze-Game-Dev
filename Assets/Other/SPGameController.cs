using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SPGameController : MonoBehaviour, IGameController
{
    public float objectivesCompleted { get; set; }
    public float objectivesTotal { get; set; }
    public float time { get; set; }
    public bool isGameRunning { get; set; }
    public int difficulty;
    public bool extractionReady { get; set; }

    [Header("Spawning")]
    public float enemySpawningInterval;
    public float itemSpawningInterval;
    float enemyLastSpawn;
    float itemLastSpawn;
    void Start()
    {
        difficulty = PlayerPrefs.GetInt("difficulty");
        StartGame();
    }

    public void StartGame()
    {
        GetComponent<MazeGenerator>().GenerateMaze(difficulty);
        GetComponent<Spawning>().Spawn(difficulty, GetComponent<MazeGenerator>().size);
        for(int i = 0; i < 5 * difficulty; i++) GetComponent<ObjectiveCounter>().addObjective("creature");
        objectivesCompleted = 0;
        isGameRunning = true;
    }

    void Update()
    {
        if (!isGameRunning) return;
        time += Time.deltaTime;
        if (objectivesCompleted == objectivesTotal) extractionReady = true; else extractionReady = false;
        if(time > enemyLastSpawn + enemySpawningInterval / difficulty)
        {
            enemyLastSpawn = time;
            GetComponent<Spawning>().spawnCreature(GetComponent<MazeGenerator>().size);
        }
        if (time > enemyLastSpawn + itemSpawningInterval / difficulty)
        {
            itemLastSpawn = time;
            GetComponent<Spawning>().spawnItem(GetComponent<MazeGenerator>().size);
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
