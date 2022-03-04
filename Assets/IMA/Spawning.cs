using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    [Header("Spawn prefabs")]
    public GameObject extractionObject;
    public GameObject pickupObjective;
    public GameObject machine;
    public GameObject[] traps;
    public GameObject[] enemies;
    public GameObject[] weapons;
    public GameObject[] items;

    [Header("Other")]
    public Transform parent;
    int mazeSize;

    public void Spawn(int difficulty, int mazeSize)
    {
        this.mazeSize = mazeSize;
        //Pickup Objectives:
        int pickupCount;
        pickupCount = difficulty == 1 ? 5 : difficulty == 2 ? 10 : difficulty == 3 ? 20 : difficulty == 4 ? 35 : 0;
        for (int i = 0; i < pickupCount; i++)
        {
            GameObject objSpawn = pickupObjective;

            GetComponentInChildren<ObjectiveCounter>().addObjective("pickup");

            int r = Random.Range(1, 3);
            int x = Random.Range(0, mazeSize);
            if (x > mazeSize / 3 && x < mazeSize / 3 * 2) x = mazeSize / 3 * r;
            r = Random.Range(1, 3);
            int y = Random.Range(0, mazeSize);
            if (y > mazeSize / 3 && y < mazeSize / 3 * 2) y = mazeSize / 3 * r;

            Vector3 position = new Vector3((x * 5 - mazeSize * 2.5f) + 2.5f, 0f, (y * 5 - mazeSize * 2.5f) + 2.5f);

            Instantiate(objSpawn, position, Quaternion.identity, parent);
        }
        //Machine objectives
        int machineCount;
        machineCount = difficulty == 1 ? 0 : difficulty == 2 ? 1 : difficulty == 3 ? 3 : difficulty == 4 ? 5 : 6;
        for (int i = 0; i < machineCount; i++)
        {
            GetComponentInChildren<ObjectiveCounter>().addObjective("machine");
            spawnMachine();
        }

        //Traps
        int trapCount;
        trapCount = difficulty == 1 ? 7 : difficulty == 2 ? 15 : difficulty == 3 ? 25 : difficulty == 4 ? 50 : 0;
        for (int i = 0; i < trapCount; i++)
        {
            int r = Random.Range(0, traps.Length);
            GameObject objSpawn = traps[r];

            r = Random.Range(1, 3);
            int x = Random.Range(0, mazeSize);
            if (x > mazeSize / 3 && x < mazeSize / 3 * 2) x = mazeSize / 3 * r;
            r = Random.Range(1, 3);
            int y = Random.Range(0, mazeSize);
            if (y > mazeSize / 3 && y < mazeSize / 3 * 2) y = mazeSize / 3 * r;

            Vector3 position = new Vector3((x * 5 - mazeSize * 2.5f) + 2.5f, 0f, (y * 5 - mazeSize * 2.5f) + 2.5f);

            GameObject trap = Instantiate(objSpawn, position, Quaternion.identity, parent);

            //Rotate to open wall
            float wallNumber = 0;
            MazeCell trapObj = GameObject.Find("Maze Cell (" + x + ", " + y + ")").GetComponent<MazeCell>();
            //Select wall
            while (wallNumber == 0)
            {
                int w = Random.Range(1, 5);
                if (w == 1 && !trapObj.Wall1) wallNumber = 1;
                if (w == 2 && !trapObj.Wall2) wallNumber = 2;
                if (w == 3 && !trapObj.Wall3) wallNumber = 3;
                if (w == 4 && !trapObj.Wall4) wallNumber = 4;
                if (trapObj.Wall1 && trapObj.Wall2 && trapObj.Wall3 && trapObj.Wall4) wallNumber = 1;
            }
            //Actual rotation
            float rotation = 0;
            if (wallNumber == 1) rotation = 0;
            if (wallNumber == 2) rotation = 180;
            if (wallNumber == 3) rotation = 270;
            if (wallNumber == 4) rotation = 90;
            trap.transform.Rotate(0, rotation, 0);

            //Close trap set wall
            if (trap.GetComponent<CloseTrap>())
            {
                CloseTrap trapScript = trap.GetComponent<CloseTrap>();
                trapScript.trapCell = trapObj;
                trapScript.wallNumber = wallNumber;
            }
        }
        //Creatures
        int enemyCount;
        enemyCount = difficulty == 1 ? 4 : difficulty == 2 ? 10 : difficulty == 3 ? 20 : difficulty == 4 ? 30 : 0;
        for (int i = 0; i < enemyCount; i++)
        {
            spawnCreature();
        }
        //Item
        int itemCount;
        itemCount = difficulty == 1 ? 2 : difficulty == 2 ? 5 : difficulty == 3 ? 10 : difficulty == 4 ? 20 : 0;
        for (int i = 0; i < itemCount; i++)
        {
            spawnItem();
        }
        //Weapon
        int weaponCount;
        weaponCount = difficulty == 1 ? 1 : difficulty == 2 ? 3 : difficulty == 3 ? 5 : difficulty == 4 ? 10 : 0;
        for (int i = 0; i < weaponCount; i++)
        {
            spawnWeapon();
        }

        //Extraction
        Vector3 extractPosition = new Vector3(0, 2, 0);
        Instantiate(extractionObject, extractPosition, Quaternion.identity, parent);
    }

    public void spawnCreature()
    {
        int r = Random.Range(1, 3);
        int x = Random.Range(0, mazeSize);
        if (x > mazeSize / 3 && x < mazeSize / 3 * 2) x = mazeSize / 3 * r;
        r = Random.Range(1, 3);
        int y = Random.Range(0, mazeSize);
        if (y > mazeSize / 3 && y < mazeSize / 3 * 2) y = mazeSize / 3 * r;
        Vector3 position = new Vector3((x * 5 - mazeSize * 2.5f) + 2.5f, 1f, (y * 5 - mazeSize * 2.5f) + 2.5f);
        r = Random.Range(0, enemies.Length);
        Instantiate(enemies[r], position, Quaternion.identity, parent);
    }

    public void spawnWeapon()
    {
        int r = Random.Range(0, weapons.Length);
        GameObject weaponPrefab = weapons[r];
        int x = Random.Range(0, mazeSize);
        int y = Random.Range(0, mazeSize);
        Vector3 position = new Vector3((x * 5 - mazeSize * 2.5f) + 2.5f, 0f, (y * 5 - mazeSize * 2.5f) + 2.5f);
        Instantiate(weaponPrefab, position, Quaternion.identity, parent);
    }

    public void spawnItem()
    {
        int r = Random.Range(0, items.Length);
        GameObject itemPrefab = items[r];
        int x = Random.Range(0, mazeSize);
        int y = Random.Range(0, mazeSize);
        Vector3 position = new Vector3((x * 5 - mazeSize * 2.5f) + 2.5f, 0f, (y * 5 - mazeSize * 2.5f) + 2.5f);
        Instantiate(itemPrefab, position, Quaternion.identity, parent);
    }

    public void spawnMachine()
    {
        int r = Random.Range(1, 3);
        int x = Random.Range(0, mazeSize);
        if (x > mazeSize / 3 && x < mazeSize / 3 * 2) x = mazeSize / 3 * r;
        r = Random.Range(1, 3);
        int y = Random.Range(0, mazeSize);
        if (y > mazeSize / 3 && y < mazeSize / 3 * 2) y = mazeSize / 3 * r;
        Vector3 position = new Vector3((x * 5 - mazeSize * 2.5f) + 2.5f, -0.3f, (y * 5 - mazeSize * 2.5f) + 2.5f);
        Instantiate(machine, position, Quaternion.identity, parent);
    }
}
