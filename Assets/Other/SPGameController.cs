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
    public float weaponSpawningInterval;
    float enemyLastSpawn;
    float itemLastSpawn;
    float weaponLastSpawn;
    void Start()
    {
        difficulty = PlayerPrefs.GetInt("difficulty");
        StartGame();
    }

    public void StartGame()
    {
        GetComponent<MazeGenerator>().GenerateMaze(difficulty);
        GetComponent<Spawning>().Spawn(difficulty, GetComponent<MazeGenerator>().size);
        int enemyCount;
        enemyCount = difficulty == 1 ? 5 : difficulty == 2 ? 10 : difficulty == 3 ? 20 : difficulty == 4 ? 50 : 0;
        for (int i = 0; i < enemyCount; i++) GetComponentInChildren<ObjectiveCounter>().addObjective("creature");
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
            GetComponent<Spawning>().spawnCreature();
        }
        if (time > itemLastSpawn + itemSpawningInterval / difficulty)
        {
            itemLastSpawn = time;
            GetComponent<Spawning>().spawnItem();
        }
        if (time > weaponLastSpawn + weaponSpawningInterval / difficulty)
        {
            weaponLastSpawn = time;
            GetComponent<Spawning>().spawnWeapon();
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
