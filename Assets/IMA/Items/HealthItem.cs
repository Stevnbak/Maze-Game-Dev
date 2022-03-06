using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour, IInteractable
{

    public float time { get; set; }
    public float setTime;
    public float healthBonus;
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerInfo>().health += healthBonus;
        AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, transform.position);
        Destroy(gameObject);
    }
}
