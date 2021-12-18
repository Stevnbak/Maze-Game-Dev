using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public float health;

    
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
