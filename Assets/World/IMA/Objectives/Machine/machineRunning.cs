using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class machineRunning : MonoBehaviour, ICreature
{
    [Header("Stats")]
    public float maxHealth;
    public float health { get; set; }
    public float goalTime;

    [Header("Spawning")]
    public float spawnInterval;
    public int spawnRange;
    public GameObject enemyPrefab;

    [Header("VFX/Animations")]
    public ParticleSystem deathVFX;
    public ParticleSystem spawnVFX;
    [Header("Other")]
    float lastSpawn;
    float time;
    GameObject player;
    GameObject gameControllerObj;

    void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        gameControllerObj = GameObject.FindGameObjectWithTag("GameController");
        time = 0;
    }

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;

        if(time >= lastSpawn + spawnInterval)
        {
            spawnEnemy();
            lastSpawn = time;
        }

        if(time >= goalTime)
        {
            gameControllerObj.GetComponentInChildren<ObjectiveCounter>().countObjective("machine");
            Destroy(gameObject);
        }

        if (health <= 0) Death();
    }

    void spawnEnemy()
    {
        int x = Random.Range(-spawnRange, spawnRange);
        int y = Random.Range(-spawnRange, spawnRange);
        Vector3 position = new Vector3(transform.position.x + x, 1, transform.position.y + y);
        GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity, transform.parent);
        Instantiate(spawnVFX.gameObject, position, Quaternion.identity, null);
        newEnemy.GetComponent<BasicEnemy>().target = this.gameObject;
    }

    void Death()
    {
        Instantiate(deathVFX.gameObject, transform.position, Quaternion.identity, null);
        gameControllerObj.GetComponent<Spawning>().spawnMachine();
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
