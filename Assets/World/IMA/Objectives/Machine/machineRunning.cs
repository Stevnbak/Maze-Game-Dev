using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class machineRunning : MonoBehaviour, ICreature
{
    [Header("Stats")]
    public float setMaxHealth;
    public float maxHealth { get; set; }
    public float health { get; set; }
    public float goalTime;

    [Header("Spawning")]
    public float spawnInterval;
    public int spawnRange;

    [Header("VFX/Animations")]
    public ParticleSystem deathVFX;
    public ParticleSystem spawnVFX;
    public Slider progressBar;

    [Header("Other")]
    float lastSpawn;
    float time;
    GameObject gameControllerObj;
    GameObject player;

    void Start()
    {
        maxHealth = setMaxHealth;
        health = maxHealth;
        gameControllerObj = GameObject.FindGameObjectWithTag("GameController");
        player = GameObject.FindGameObjectWithTag("Player");
        time = 0;
    }

    void FixedUpdate()
    {
        maxHealth = setMaxHealth;
        time += Time.fixedDeltaTime;

        //Progress bar
        progressBar.transform.parent.gameObject.SetActive(true);
        progressBar.transform.LookAt(player.transform.position);
        progressBar.value = (time / goalTime);

        //Spawn
        if(time >= lastSpawn + spawnInterval)
        {
            spawnEnemy();
            lastSpawn = time;
        }

        //Progress
        if(time >= goalTime)
        {
            gameControllerObj.GetComponentInChildren<ObjectiveCounter>().countObjective("machine");
            Destroy(gameObject);
        }

        //Death?
        if (health <= 0) Death();
    }

    void spawnEnemy()
    {
        int x = Random.Range(-spawnRange, spawnRange);
        int y = Random.Range(-spawnRange, spawnRange);
        Vector3 position = new Vector3(transform.position.x + x, 1, transform.position.y + y);
        int r = Random.Range(0, gameControllerObj.GetComponent<Spawning>().enemies.Length);
        GameObject newEnemy = Instantiate(gameControllerObj.GetComponent<Spawning>().enemies[r], position, Quaternion.identity, transform.parent);
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
