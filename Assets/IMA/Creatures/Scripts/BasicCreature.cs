using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicCreature : MonoBehaviour, ICreature
{
    public float health { get; set; }
    private NavMeshAgent agent;
    Vector3 targetPos;
    GameObject player;
    [Header("Stats")]
    public float maxHealth;
    public float damage, movementSpeed, range;

    void Start()
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //Movement
        Move();

        //Death
        if(health <= 0) Death();
    }

    void Move()
    {
        //Target position
        if(Vector3.Distance(transform.position, player.transform.position) < range)
        {
            targetPos = player.transform.position;
        }

        //Path
        NavMeshPath path = agent.path;
        agent.CalculatePath(targetPos, path);
        agent.path = path;
    }

    void Death()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
