using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI AmmoCount;
    public TextMeshProUGUI TotalAmmo;
    public Image WeaponIcon;
    public Image Crosshair;
    public Slider HealthBar;
    public Image Interact;

    [Header("Info")]
    GameObject player;
    public float health;
    public float maxHealth;
    public float ammo;
    public float totalAmmo;
    public bool interactInProgress;
    float t, time;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //Update info
        health = player.GetComponent<PlayerInfo>().health;
        maxHealth = player.GetComponent<PlayerInfo>().maxHealth;
        ammo = player.GetComponentInChildren<IWeapon>().ammoInMag;
        totalAmmo = player.GetComponentInChildren<IWeapon>().ammoTotal;

        //Calculate health percent
        float healthPercent = health / maxHealth;

        //Update HUD
        AmmoCount.text = ammo.ToString();
        TotalAmmo.text = totalAmmo.ToString();
        HealthBar.value = healthPercent;

        //Interact update
        if (interactInProgress)
        {;
            t += Time.deltaTime;
            float fill = t / time;
            Interact.fillAmount = fill;
            if (t >= time)
            {
                interactInProgress = false;
                Interact.fillAmount = 0;
            }
        }
    }

    public void startInteractTimer(float time)
    {
        interactInProgress = true;
        t = 0;
        this.time = time;
    }

    public void stopInteractTimer()
    {
        interactInProgress = false;
        Interact.fillAmount = 0;
    }
}
