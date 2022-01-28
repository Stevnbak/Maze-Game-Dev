using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public float health;
    public float maxHealth;
    
    void Update()
    {
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
