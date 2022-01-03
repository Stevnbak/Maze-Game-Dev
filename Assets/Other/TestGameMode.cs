using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameMode : MonoBehaviour, IGameController
{
    public float objectivesCompleted { get; set; }
    public float objectivesTotal { get; set; }
    public float time { get; set; }
    public bool isGameRunning { get; set; }

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        GetComponent<MazeGenerator>().GenerateMaze();
        //GetComponent<Spawning>().Spawn();
        objectivesTotal = 5;
        objectivesCompleted = 0;
        isGameRunning = true;
    }

    void Update()
    {

        if (!isGameRunning) return;
        time += Time.deltaTime;
        if (objectivesCompleted == objectivesTotal) EndGame();

    }

    public void EndGame()
    {
        isGameRunning = false;
        Debug.Log("Game completed in " + Mathf.Round(time) + " seconds!");
    }
}
