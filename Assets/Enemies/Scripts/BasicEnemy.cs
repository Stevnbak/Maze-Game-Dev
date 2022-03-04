using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour, ICreature
{
    private NavMeshAgent agent;
    [Header("Movement")]
    public float movementSpeed;
    public float targetRange, attackRange, randomRange, trackRange;

    [Header("Stats")]
    public float setMaxHealth;
    public float maxHealth { get; set; }
    public float health { get; set; }

    [Header("Other")]
    public ParticleSystem deathVFX;
    public GameObject target;
    IAttack attack;

    bool randomDestSet = false;
    bool lineOfSight;
    bool targeting;

    void Start()
    {
        maxHealth = setMaxHealth;
        health = maxHealth;
        agent = GetComponentInChildren<NavMeshAgent>();
        if (target == null) target = GameObject.FindGameObjectWithTag("Player");
        attack = GetComponent<IAttack>();
    }

    void FixedUpdate()
    {
        maxHealth = setMaxHealth;
        //Update target
        if (target == null) target = GameObject.FindGameObjectWithTag("Player"); 

        //Line of sight:
        int layerMask = LayerMask.GetMask("World", "Creature", "Wall", "Player");
        Vector3 direction = (target.transform.position - transform.position);

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, Vector3.Distance(transform.position, target.transform.position), layerMask))
        {
            GameObject hitObject = hit.transform.gameObject;
            lineOfSight = (hitObject == target);
        }
        else
        {
            lineOfSight = false;
        }
        if(targeting && Vector3.Distance(transform.position, target.transform.position) > trackRange)
        {
                targeting = false;
        }

        //Movement
        if (Vector3.Distance(transform.position, target.transform.position) < targetRange || targeting) TargetPlayer();
        else RandomMovement();

        //Death
        if(health <= 0) Death();
    }

    void TargetPlayer()
    {
        targeting = true;
        //Debug.Log("Targeting player");
        agent.isStopped = false;
        randomDestSet = false;
        //Target position
        Vector3 targetPos = target.transform.position;

        //Set destination
        agent.SetDestination(targetPos);
        if (agent.remainingDistance < attackRange && lineOfSight)
        {
            Attack();
        } else attack.Stop();
    }

    void Attack()
    {
        //Rotation
        transform.LookAt(target.transform.position);

        //Stop ai movement
        randomDestSet = false;
        agent.isStopped = true;

        //Actual attack
        attack.Attack(target);
    }

    void RandomMovement()
    {
        attack.Stop();
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
        attack.Stop();
        PlayerPrefs.SetFloat("enemyKills", PlayerPrefs.GetFloat("enemyKills") + 1);
        GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<ObjectiveCounter>().countObjective("creature");
        GameObject vfxObj = Instantiate(deathVFX.gameObject);
        vfxObj.transform.position = transform.position;
        vfxObj.GetComponent<ParticleSystem>().Play();
        vfxObj.GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
