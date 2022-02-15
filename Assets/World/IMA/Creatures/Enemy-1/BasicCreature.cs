using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicCreature : MonoBehaviour, ICreature
{
    public float health { get; set; }
    private NavMeshAgent agent;
    [Header("Movement")]
    public float movementSpeed;
    public float targetRange, attackRange, randomRange;

    [Header("Stats")]
    public float damage;
    public float maxHealth, attackTime;

    [Header("Other")]
    public ParticleSystem deathVFX;
    public ParticleSystem attackVFX;
    public GameObject target;
    public string targetTag = "Player";
    bool randomDestSet = false;
    bool lineOfSight;
    bool targetingPlayer;
    float time;
    IGameController gameController;

    void Start()
    {
        health = maxHealth;
        agent = GetComponentInChildren<NavMeshAgent>();
        if (target == null) target = GameObject.FindGameObjectWithTag("Player");
        if (targetTag == "") targetTag = "Player";
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<IGameController>();
    }

    void FixedUpdate()
    {
        //Update target
        if (target == null) { 
            target = GameObject.FindGameObjectWithTag("Player"); 
            targetTag = "Player"; 
        }

        //Line of sight:
        int layerMask = LayerMask.GetMask("World", "Creature", "Wall", "Player");
        Vector3 direction = (target.transform.position - transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(transform.position, target.transform.position), layerMask))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.CompareTag(targetTag))
            {
                lineOfSight = true;
                //Debug.Log("Looking at player");
            } else
            {
                lineOfSight = false;
                //Debug.Log("Not looking at player");
            }
        }
        else
        {
            lineOfSight = false;
            //Debug.Log("Seeing nothing");
        }
        if(targetingPlayer)
        {
            if(Vector3.Distance(transform.position, target.transform.position) > targetRange * 2.5f)
            {
                targetingPlayer = false;
            }
        }

        //Movement
        if (Vector3.Distance(transform.position, target.transform.position) < targetRange || targetingPlayer) TargetPlayer();
        else RandomMovement();

        //Death
        if(health <= 0) Death();
    }

    void TargetPlayer()
    {
        targetingPlayer = true;
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
        }
        else
        {
            GetComponent<AudioSource>().Stop();
            attackVFX.Stop();
        }
    }

    void Attack()
    {
        attackVFX.Play();
        if(!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
        transform.LookAt(target.transform.position);
        //Debug.Log("Attacking player");
        randomDestSet = false;
        agent.isStopped = true;
        time += Time.deltaTime;
        if (time >= attackTime)
        {
            target.GetComponent<ICreature>().TakeDamage(damage);
            time = 0;
        }
    }

    void RandomMovement()
    {
        attackVFX.Stop();
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
