using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    [Header("Spawning Prefabs")]
    public GameObject extractionObject;
    public GameObject pickupObjective;
    public GameObject locationObjective;
    public GameObject explosiveTrap;
    public GameObject closeTrap;
    public GameObject basicEnemy;

    [Header("Other")]
    public Transform parent;


    public void Spawn(int difficulty, int mazeSize)
    {
        //Objectives:
        for (int i = 0; i < 5 * difficulty; i++)
        {
            int r = Random.Range(1, 3);
            GameObject objSpawn;
            if (r == 1) objSpawn = pickupObjective;
            else objSpawn = locationObjective;

            if (r == 1) GetComponent<ObjectiveCounter>().addObjective("pickup");
            if (r == 2) GetComponent<ObjectiveCounter>().addObjective("location");


            r = Random.Range(1, 3);
            int x = Random.Range(0, mazeSize);
            if (x > mazeSize / 3 && x < mazeSize / 3 * 2) x = mazeSize / 3 * r;
            r = Random.Range(1, 3);
            int y = Random.Range(0, mazeSize);
            if (y > mazeSize / 3 && y < mazeSize / 3 * 2) y = mazeSize / 3 * r;

            Vector3 position = new Vector3((x * 5 - mazeSize * 2.5f) + 2.5f, 0f, (y * 5 - mazeSize * 2.5f) + 2.5f);

            Instantiate(objSpawn, position, Quaternion.identity, parent);
        }
        //Traps
        for (int i = 0; i < 7 * difficulty; i++)
        {
            int r = Random.Range(1, 3);
            GameObject objSpawn;
            if (r == 1) objSpawn = explosiveTrap;
            else objSpawn = closeTrap;

            r = Random.Range(1, 3);
            int x = Random.Range(0, mazeSize);
            if (x > mazeSize / 3 && x < mazeSize / 3 * 2) x = mazeSize / 3 * r;
            r = Random.Range(1, 3);
            int y = Random.Range(0, mazeSize);
            if (y > mazeSize / 3 && y < mazeSize / 3 * 2) y = mazeSize / 3 * r;

            Vector3 position = new Vector3((x * 5 - mazeSize * 2.5f) + 2.5f, 0f, (y * 5 - mazeSize * 2.5f) + 2.5f);

            GameObject trap = Instantiate(objSpawn, position, Quaternion.identity, parent);
            if (trap.GetComponent<CloseTrap>())
            {
                CloseTrap trapScript = trap.GetComponent<CloseTrap>();
                MazeCell trapObj = GameObject.Find("Maze Cell (" + x + ", " + y + ")").GetComponent<MazeCell>();
                trapScript.trapCell = trapObj;
                while (trapScript.wallNumber == 0)
                {
                    int w = Random.Range(1, 5);
                    if (w == 1 && !trapObj.Wall1) trapScript.wallNumber = 1;
                    if (w == 2 && !trapObj.Wall2) trapScript.wallNumber = 2;
                    if (w == 3 && !trapObj.Wall3) trapScript.wallNumber = 3;
                    if (w == 4 && !trapObj.Wall4) trapScript.wallNumber = 4;
                    if (trapObj.Wall1 && trapObj.Wall2 && trapObj.Wall3 && trapObj.Wall4) trapScript.wallNumber = 1;
                }
            }
        }
        //Creatures
        for (int i = 0; i < 3 * difficulty; i++)
        {
            spawnCreature(mazeSize);
        }

        //Extraction
        Vector3 extractPosition = new Vector3(0, 2, 0);
        Instantiate(extractionObject, extractPosition, Quaternion.identity, parent);
    }

    public void spawnCreature(int mazeSize)
    {
        int r = Random.Range(1, 3);
        int x = Random.Range(0, mazeSize);
        if (x > mazeSize / 3 && x < mazeSize / 3 * 2) x = mazeSize / 3 * r;
        r = Random.Range(1, 3);
        int y = Random.Range(0, mazeSize);
        if (y > mazeSize / 3 && y < mazeSize / 3 * 2) y = mazeSize / 3 * r;
        Vector3 position = new Vector3((x * 5 - mazeSize * 2.5f) + 2.5f, 1f, (y * 5 - mazeSize * 2.5f) + 2.5f);
        Instantiate(basicEnemy, position, Quaternion.identity, parent);
    }
}
