using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon_Shotgun : MonoBehaviour, IWeapon
{
    [Header("Stats")]
    public float damage;
    public float firerate, setMagTotal, reloadTime, bulletsPerShot;
    public float ammoInMag { get; set; }
    public float ammoMagTotal { get; set; }
    public float ammoTotal { get; set; }
    public Transform lookingAt;

    [Header("Shooting")]
    public Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    public Transform bulletPoint;


    [Header("Visuals")]
    public TrailRenderer bulletTrail;
    public ParticleSystem shootingSystem;
    public ParticleSystem hitSystem;

    [Header("Audio")]
    public AudioSource shootSound;
    public AudioSource reloadSound;

    [Header("Other")]
    float reloadTimer;
    public bool isFiring { get; set; }
    public bool isReloading { get; set; }
    public bool isADSing { get; set; }
    public float fireTime { get; set; }

    public void Initialize()
    {
        Debug.Log("Initializing weapon");
        ammoMagTotal = setMagTotal;
        this.enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Input_Manager>().updateWeapon(this);
        lookingAt = GameObject.Find("LookingAt").transform;
        isFiring = false;
        isReloading = false;
        isADSing = false;
        fireTime = 0;
        Reload();
    }

    void Update()
    {
        //Aim gun
        transform.LookAt(lookingAt);

        fireTime += Time.deltaTime;

        GetComponent<WeaponItem>().vfx.Stop();

        //Fire
        if (isFiring)
        {
            isFiring = false;
            if (fireTime >= firerate)
            {
                Fire();
            }
        }

        //ADS
        if (isADSing)
        {
            Vector3 newposition = Vector3.Lerp(transform.position, transform.parent.parent.Find("ADS_Weapon_Pos").position, Time.deltaTime * 10f);
            transform.position = newposition;
        }
        else
        {
            Vector3 newposition = Vector3.Lerp(transform.position, transform.parent.position, Time.deltaTime * 10f);
            transform.position = newposition;
        }

        //Reload
        if (isReloading)
        {
            if (ammoInMag == ammoMagTotal) return;
            if (reloadTimer == 0)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<HUD>().startInteractTimer(reloadTime);
                reloadSound.Play();
            }
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= reloadTime) Reload();
        }
        else reloadTimer = 0;
    }

    public void Reload()
    {
        Debug.Log("Reloaded");
        isReloading = false;
        if (ammoTotal == 0) return;
        ammoTotal -= ammoMagTotal - ammoInMag;
        ammoInMag = ammoMagTotal;
        if (ammoTotal < 0)
        {
            ammoInMag += ammoTotal;
            ammoTotal = 0;
        }
    }

    public void Fire()
    {
        fireTime = 0;
        //Ammo
        if (ammoInMag == 0)
        {
            return;
        }
        ammoInMag -= 1;

        //Visuals
        shootingSystem.Play();
        shootSound.Play();

        //Recoil
        GetComponent<Recoil>().bulletFired(firerate / 2);

        //Stats
        PlayerPrefs.SetFloat("shotsFired", PlayerPrefs.GetFloat("shotsFired") + 1);

        //RayCast Hit:
        int layerMask = LayerMask.GetMask("World", "Creature", "Wall");

        for (int i = 0; i < bulletsPerShot; i++) {
            Vector3 direction = GetDirection();

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, direction, out hit, Mathf.Infinity, layerMask))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.CompareTag("Wall") || hitObject.CompareTag("Ground"))
                {
                    //Debug.Log("Hit the maze");
                }

                if (hitObject.CompareTag("Creature"))
                {
                    //Debug.Log("Hit a creature");
                    hitObject.GetComponent<ICreature>().TakeDamage(damage);
                }
                TrailRenderer trail = Instantiate(bulletTrail, bulletPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
                Debug.DrawRay(bulletPoint.position, hit.point, Color.green);
            }
            else
            {
                Debug.DrawRay(bulletPoint.position, direction * 1000, Color.red);
                //Debug.Log("Hit nothing");
                TrailRenderer trail = Instantiate(bulletTrail, bulletPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
            }
        }
    }
    Vector3 GetDirection()
    {
        Vector3 direction = Camera.main.transform.forward;

        direction += new Vector3(
            Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
            Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
            Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
        );
        direction.Normalize();
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
        Destroy(trail.gameObject, trail.time);
        GameObject hitParticle = Instantiate(hitSystem.gameObject);
        //GameObject hitParticle = hitSystem.gameObject;
        hitParticle.transform.position = hit.point;
        hitParticle.GetComponent<ParticleSystem>().Play();
    }

    public void Drop(Transform otherTrans)
    {
        transform.parent = otherTrans.parent;
        transform.position = otherTrans.position;
        transform.rotation = otherTrans.rotation;
        GetComponent<WeaponItem>().SetLayerRecursively(gameObject, 3);
        gameObject.tag = "Item";
        ammoInMag = 0;
        GetComponent<WeaponItem>().vfx.Play();
        this.enabled = false;
    }
}
