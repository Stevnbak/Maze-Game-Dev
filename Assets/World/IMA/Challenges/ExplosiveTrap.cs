using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveTrap : MonoBehaviour
{
    public float damage;
    public float time;
    public float radius;
    public bool isTriggered;
    public ParticleSystem vfx;
    float t = 0;

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
        }
    }

    void explosion()
    {
        Instantiate(vfx.gameObject, transform.position, transform.rotation, null).GetComponent<ParticleSystem>().Play();
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
