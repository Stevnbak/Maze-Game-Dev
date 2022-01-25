using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationObjective : MonoBehaviour
{
    public string objectiveName;
    public float distance;
    Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if(Vector3.Distance(player.position, transform.position) <= distance) complete();
    }

    public void complete()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectiveCounter>().countObjective(objectiveName);
        Destroy(gameObject);
    }
}
