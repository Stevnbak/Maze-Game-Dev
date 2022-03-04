using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour, ICreature
{
    public float health { get; set; }
    public float setMaxHealth;
    public float maxHealth { get; set; }
    public float ammo;
    
    void Start()
    {
        maxHealth = setMaxHealth;
        health = maxHealth;
        GetComponentInChildren<IWeapon>().ammoTotal = ammo;
        GetComponentInChildren<IWeapon>().Initialize();
    }

    void Update()
    {
        maxHealth = setMaxHealth;
        ammo = GetComponentInChildren<IWeapon>().ammoTotal;
        health = Mathf.Clamp(health, 0, maxHealth);
        if(health <= 0)
        {
            PlayerPrefs.SetString("state", "lose");
            GameObject.FindGameObjectWithTag("GameController").GetComponent<IGameController>().EndGame();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
