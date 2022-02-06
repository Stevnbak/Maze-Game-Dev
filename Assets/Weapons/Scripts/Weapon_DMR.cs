using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_DMR : MonoBehaviour, IWeapon
{
    [Header("Stats")]
    public float damage;
    public float firerate, setAmmoTotal, ammoMagTotal, reloadTime;
    public float ammoInMag { get; set; }
    public float ammoTotal { get; set; }
    public Transform lookingAt;

    [Header("Shooting")]
    public bool addBulletSpread = true;
    public Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    public Transform bulletPoint;


    [Header("Visuals")]
    public ParticleSystem shootingSystem;
    public ParticleSystem hitSystem;

    //Bools
    public bool isFiring { get; set; }
    public bool isReloading { get; set; }
    public bool isADSing { get; set; }
    public float fireTime { get; set; }

    float reloadTimer;

    void Start()
    {
        ammoTotal = setAmmoTotal;
    }

    public void Initialize()
    {
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
        //Update editor total ammo
        setAmmoTotal = ammoTotal;

        //Aim gun
        transform.LookAt(lookingAt);

        //Fire
        fireTime += Time.deltaTime;
        if (isFiring)
        {
            if (fireTime >= firerate)
            {
                fireTime = 0;
                Fire();
            }
        }


        GetComponent<WeaponItem>().vfx.Stop();

        //Reload
        if (isReloading)
        {
            if (reloadTimer == 0) GameObject.FindGameObjectWithTag("GameController").GetComponent<HUD>().startInteractTimer(reloadTime);
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= reloadTime) Reload();
        }
        else reloadTimer = 0;
    }

    public void Reload()
    {
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
        isFiring = false;
        //Ammo
        if (ammoInMag == 0)
        {
            isFiring = false;
            return;
        }
        //Visuals
        shootingSystem.Play();
        ammoInMag -= 1;

        PlayerPrefs.SetFloat("shotsFired", PlayerPrefs.GetFloat("shotsFired") + 1);

        //RayCast Hit:
        int layerMask = LayerMask.GetMask("World", "Creature", "Wall");
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
            Debug.DrawRay(bulletPoint.position, hit.point, Color.green);
        }
        else
        {
            Debug.DrawRay(bulletPoint.position, direction * 1000, Color.red);
            Debug.Log("Hit nothing");
        }

        GameObject hitParticle = Instantiate(hitSystem.gameObject);
        hitParticle.transform.position = hit.point;
        hitParticle.GetComponent<ParticleSystem>().Play();
    }
    Vector3 GetDirection()
    {
        Vector3 direction = Camera.main.transform.forward;
        if (addBulletSpread)
        {
            direction += new Vector3(
            Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
            Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
            Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
            );
            direction.Normalize();
        }
        return direction;
    }

    public void Drop(Transform otherTrans)
    {
        transform.parent = otherTrans.parent;
        transform.position = otherTrans.position;
        transform.rotation = otherTrans.rotation;
        GetComponent<WeaponItem>().SetLayerRecursively(gameObject, 3);
        gameObject.tag = "Item";
        GetComponent<WeaponItem>().vfx.Play();
        this.enabled = false;
    }
}
