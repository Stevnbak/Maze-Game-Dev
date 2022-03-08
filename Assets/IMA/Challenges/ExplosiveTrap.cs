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
            if (t == 0) GetComponent<AudioSource>().Play();
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
        GameObject vfxObj = Instantiate(vfx.gameObject, transform.position, transform.rotation, null);
        vfxObj.GetComponent<ParticleSystem>().Play();
        vfxObj.GetComponent<AudioSource>().Play();

        Debug.Log("Explosion!");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(hitCollider.transform.position, transform.position);
            float damageTaken = damage * (radius / distance);
            if (distance > radius) damageTaken = 0;

            if (hitCollider.CompareTag("Player"))
            {
                hitCollider.GetComponent<PlayerInfo>().TakeDamage(damageTaken);
            }
            if (hitCollider.CompareTag("Creature"))
            {
                hitCollider.GetComponent<ICreature>().TakeDamage(damageTaken);
            }
        }
        Destroy(gameObject);
    }
}
