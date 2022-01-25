using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon_AR : MonoBehaviour, IWeapon
{
    [Header("Stats")]
    public float damage;
    public float firerate, setAmmoTotal, ammoMagTotal, reloadTime;
    public float ammoInMag {get; set;}
    public float ammoTotal { get; set; }
    public Transform lookingAt;

    [Header("Shooting")]
    public bool addBulletSpread = true;
    public Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    public ParticleSystem shootingSytem;
    public Transform bulletPoint;
    public TrailRenderer bulletTrail;


    //Bools
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
        //Update total ammo from editor
        ammoTotal = setAmmoTotal;
        Reload();
    }

    void Update()
    {
        //Update editor total ammo
        setAmmoTotal = ammoTotal;

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
            if(reloadTimer == 0) GameObject.FindGameObjectWithTag("GameController").GetComponent<HUD>().startInteractTimer(reloadTime);
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
        //Visuals
        //shootingSystem.Play();

        //Ammo
        if (ammoInMag == 0) return;
        ammoInMag -= 1;

        PlayerPrefs.SetFloat("shotsFired", PlayerPrefs.GetFloat("shotsFired") + 1);

        //RayCast Hit:
        int layerMask = 1 << 6;
        layerMask = ~layerMask;
        Vector3 direction = GetDirection();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, direction, out hit, Mathf.Infinity, layerMask))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.CompareTag("Wall") || hitObject.CompareTag("Ground"))
            {
                Debug.Log("Hit the maze");
            }

            if (hitObject.CompareTag("Creature"))
            {
                Debug.Log("Hit a creature");
                hitObject.GetComponent<ICreature>().TakeDamage(damage);
            }
            TrailRenderer trail = Instantiate(bulletTrail, bulletPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));
        }
        else
        {
            Debug.DrawRay(bulletPoint.position, direction * 1000, Color.white);
            Debug.Log("Hit nothing");
            TrailRenderer trail = Instantiate(bulletTrail, bulletPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));
        }
    }
    Vector3 GetDirection()
    {
        Vector3 direction = Camera.main.transform.forward;
        if(addBulletSpread)
        {
            direction += new Vector3(
            Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
            Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
            Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
            );
            direction.Normalize();
        }
        Debug.Log(direction);
        return direction;
    }

    IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        trail.time *= hit.distance;
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hit.point;
        //Trail hit
        //Stop animation
        Destroy(trail.gameObject, trail.time);
    }
}


