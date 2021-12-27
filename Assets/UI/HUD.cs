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

    [Header("Info")]
    GameObject player;
    public float health;
    public float maxHealth;
    public float ammo;
    public float totalAmmo;

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
    }
}
