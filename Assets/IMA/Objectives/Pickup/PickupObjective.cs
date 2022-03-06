using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjective : MonoBehaviour, IInteractable
{
    public float setTime;
    public float time { get; set; }
    public string objectiveName;
    public bool hovering { get; set; }
    public GameObject inputPopup;
    Transform player;

    void Update()
    {
        time = setTime;

        inputPopup.transform.LookAt(player.position);

        if (hovering)
            inputPopup.SetActive(true);
        else inputPopup.SetActive(false);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

        public void interact()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<ObjectiveCounter>().countObjective(objectiveName);
        AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, transform.position);
        Destroy(gameObject);
    }
}
