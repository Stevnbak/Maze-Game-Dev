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
        GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<ObjectiveCounter>().countObjective(objectiveName);
        AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, transform.position);
        Destroy(gameObject);
    }
}
