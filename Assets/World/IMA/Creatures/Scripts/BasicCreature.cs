using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicCreature : MonoBehaviour, ICreature
{
    public float health { get; set; }
    private NavMeshAgent agent;
    GameObject player;
    [Header("Movement")]
    public float movementSpeed;
    public float targetRange, attackRange, randomRange;
    [Header("Stats")]
    public float damage;
    public float maxHealth, attackTime;

    [Header("Other")]
    bool randomDestSet = false;
    float time;
    IGameController gameController;
    public ParticleSystem deathVFX;

    void Start()
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<IGameController>();
    }

    void FixedUpdate()
    {
        if(!gameController.isGameRunning)
        {
            agent.isStopped = true;
            return;
        }
        agent.speed = movementSpeed;
        //Movement
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange) Attack();
        else if (Vector3.Distance(transform.position, player.transform.position) < targetRange) TargetPlayer();
        else RandomMovement();

        //Death
        if(health <= 0) Death();
    }

    void TargetPlayer()
    {
        //Debug.Log("Targeting player");
        agent.isStopped = false;
        randomDestSet = false;
        //Target position
        Vector3 targetPos = player.transform.position;

        //Set destination
        agent.SetDestination(targetPos);
    }

    void Attack()
    {
        //Debug.Log("Attacking player");
        randomDestSet = false;
        agent.isStopped = true;
        time += Time.deltaTime;
        if (time >= attackTime)
        {
            player.GetComponent<PlayerInfo>().TakeDamage(damage);
            time = 0;
        }
    }

    void RandomMovement()
    {
        //Debug.Log("Moving randomly");
        agent.isStopped = false;
        if (!randomDestSet)
        {
            float randomX = Random.Range(-randomRange, randomRange) + transform.position.x;
            float randomZ = Random.Range(-randomRange, randomRange) + transform.position.z;
            Vector3 randomPos = new Vector3(randomX, 1, randomZ);
            agent.SetDestination(randomPos);
            randomDestSet = true;
            //Debug.Log("Changed target pos to: " + randomPos);
        } else if(agent.remainingDistance < 0.5)
        {
            randomDestSet = false;
        }
        //Debug.Log("Target position: " + agent.destination);
    }

    void Death()
    {
        PlayerPrefs.SetFloat("enemyKills", PlayerPrefs.GetFloat("enemyKills") + 1);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectiveCounter>().countObjective("creature");
        GameObject vfxObj = Instantiate(deathVFX.gameObject);
        vfxObj.transform.position = transform.position;
        vfxObj.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
