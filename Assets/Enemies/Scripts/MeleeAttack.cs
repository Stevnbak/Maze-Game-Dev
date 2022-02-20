using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttack
{
    public float attackTime;
    public float damage;
    float time;
    public AudioSource attackSound;

    public void Attack(GameObject target)
    {
        time += Time.deltaTime;
        if (time >= attackTime)
        {
            GetComponent<Animator>().Play("Attack");
            //attackSound.Play();
            target.GetComponent<ICreature>().TakeDamage(damage);
            time = 0;
        }
    }
    public void Stop()
    {
        attackSound.Stop();
    }
}