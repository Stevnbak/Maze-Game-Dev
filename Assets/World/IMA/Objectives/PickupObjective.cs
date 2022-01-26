using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjective : MonoBehaviour, IInteractable
{
    public float setTime;
    public float time { get; set; }
    public string objectiveName;
    
    void Update()
    {
        time = setTime;
    }

    public void interact()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectiveCounter>().countObjective(objectiveName);
        Destroy(gameObject);
    }
}
