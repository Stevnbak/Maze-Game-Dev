using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject barVisuals;
    public float health, maxHealth, showTime;
    float oldHealth = 0;
    float time = 0;
    bool showing;
    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        health = GetComponent<ICreature>().health;
        maxHealth = GetComponent<ICreature>().maxHealth;
        barVisuals.GetComponentInChildren<Slider>().value = (health / maxHealth);
        if (showing)
        {
            barVisuals.transform.LookAt(player.position);
            time += Time.deltaTime;
            if (time >= showTime) HideHealthBar();
        }
    }
    void FixedUpdate()
    {
        if(oldHealth != health)
        {
            ShowHealthBar();
        }
        oldHealth = health;
    }

    void ShowHealthBar()
    {
        time = 0;
        showing = true;
        barVisuals.SetActive(true);
    }

    void HideHealthBar()
    {
        showing = false;
        barVisuals.SetActive(false);
    }
}
