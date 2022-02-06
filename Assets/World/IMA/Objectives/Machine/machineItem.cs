using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class machineItem : MonoBehaviour, IInteractable
{
    public float time { get; set; }
    public float setTime;
    public void Start()
    {
        time = setTime;
    }

    public void interact()
    {
        GetComponent<Animator>().enabled = true;
        Debug.Log("Started machine");
        GetComponent<machineRunning>().enabled = true;
        this.enabled = false;
    }
}
