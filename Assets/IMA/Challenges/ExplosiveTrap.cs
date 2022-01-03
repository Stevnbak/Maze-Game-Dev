using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveTrap : MonoBehaviour
{
    public float damage;
    public float time;
    public float radius;
    public bool isTriggered;
    float t;

    private void Update()
    {
        if(isTriggered)
        {
            t += Time.deltaTime;
            if (t >= time) explosion();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isTriggered = true;
            t = 0;
        }
    }

    void explosion()
    {
        //Play explosion vfx
        Debug.Log("Explosion!");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.CompareTag("Player"))
            {
                hitCollider.GetComponent<PlayerInfo>().TakeDamage(damage / (Vector3.Distance(hitCollider.transform.position, transform.position) / radius));
            }
            if (hitCollider.CompareTag("Creature"))
            {
                hitCollider.GetComponent<ICreature>().TakeDamage(damage / (Vector3.Distance(hitCollider.transform.position, transform.position) / radius));
            }
        }
        Destroy(gameObject);
    }
}
