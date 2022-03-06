using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class machineItem : MonoBehaviour, IInteractable
{
    public float time { get; set; }
    public float setTime;
    public bool hovering { get; set; }
    public GameObject inputPopup;
    Transform player;

    void Update()
    {
        inputPopup.transform.LookAt(player.position);

        if (hovering)
            inputPopup.SetActive(true);
        else inputPopup.SetActive(false);
    }
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        time = setTime;
    }

    public void interact()
    {
        GetComponent<Animator>().enabled = true;
        Debug.Log("Started machine");
        GetComponent<machineRunning>().enabled = true;
        inputPopup.SetActive(false);
        this.enabled = false;
    }
}
