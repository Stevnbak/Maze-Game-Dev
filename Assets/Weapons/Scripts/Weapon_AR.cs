using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon_AR : MonoBehaviour, IWeapon
{
    [Header("Stats")]
    public float damage;
    public float firerate, ammoMagTotal, ammoInMag, ammoTotal, reloadTime;
    public Transform lookingAt;

    public bool isFiring { get; set; }
    public bool isReloading { get; set; }
    public bool isADSing { get; set; }
    public float fireTime { get; set; }
    float reloadTimer;

    public void Initialize()
    {
        isFiring = false;
        isReloading = false;
        isADSing = false;
        fireTime = 0;
    }

    void Start()
    {
        
    }

    void Update()
    {
        //Aim gun
        transform.LookAt(lookingAt);

        //Fire
        if (isFiring) {
            fireTime += Time.deltaTime;
            if(fireTime >= firerate)
            {
                fireTime = 0;
                Fire();
            }
        }

        //Reload
        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            if(reloadTimer >= reloadTime) Reload();
        }
        else reloadTimer = 0;
    }

    public void Reload()
    {
        isReloading = false;
        if (ammoTotal == 0) return;
        ammoTotal -= ammoMagTotal - ammoInMag;
        ammoInMag = ammoMagTotal;
        if (ammoTotal < 0) { 
            ammoInMag += ammoTotal;
            ammoTotal = 0;
        }
    }

    public void Fire()
    {
        //Ammo
        if (ammoInMag == 0) return;
        ammoInMag -= 1;

        //RayCast Hit:
        int layerMask = 1 << 6;
        layerMask = ~layerMask;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.CompareTag("Wall") || hitObject.CompareTag("Wall"))
            {
                Debug.Log("Hit the maze");
            }

            if (hitObject.CompareTag("Creature"))
            {
                Debug.Log("Hit a creature");
                hitObject.GetComponent<ICreature>().TakeDamage(damage);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Hit nothing");
        }
    }
}
