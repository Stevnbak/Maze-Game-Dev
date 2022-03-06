using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : MonoBehaviour, IInteractable
{
    public float time { get; set; }
    public float setTime;
    public float speedBonus;
    public float duration;
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
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        time = setTime;
    }

    public void interact()
    {
        MovementController player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementController>();
        player.speedBoostTime = duration;
        player.speedBoostEffect = speedBonus;
        AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, transform.position);
        Destroy(gameObject);
    }
}
