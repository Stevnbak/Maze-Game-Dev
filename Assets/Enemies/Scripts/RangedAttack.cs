using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour, IAttack
{
    public float attackTime;
    public float damage;
    float time;
    public ParticleSystem VFX;
    public AudioSource attackSound;

    void Update()
    {
        
    }

    public void Attack(GameObject target)
    {
        time += Time.deltaTime;
        VFX.Play();
        if (!attackSound.isPlaying) attackSound.Play();
        if (time >= attackTime)
        {
            target.GetComponent<ICreature>().TakeDamage(damage);
            time = 0;
        }
        
    }

    public void Stop()
    {
        VFX.Stop();
        attackSound.Stop();
    }
}
