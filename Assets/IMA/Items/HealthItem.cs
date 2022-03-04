using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour, IInteractable
{

    public float time { get; set; }
    public float setTime;
    public float healthBonus;
    void Start()
    {
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
