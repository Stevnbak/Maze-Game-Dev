using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour, IInteractable
{
    public float time { get; set; }
    public float setTime;
    public ParticleSystem vfx;

    void Awake()
    {
        time = setTime;
        vfx.Play();
    }

    public void interact()
    {
        GameObject weaponPos = GameObject.FindGameObjectWithTag("Player").transform.Find("Weapon_Pos").gameObject;
        vfx.Stop();
        weaponPos.GetComponentInChildren<IWeapon>().Drop(transform);
        GetComponent<IWeapon>().ammoTotal = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>().ammo;
        GetComponent<IWeapon>().Initialize();
        transform.parent = weaponPos.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        SetLayerRecursively(gameObject, 9);
        gameObject.tag = "Weapon";
        this.enabled = false;
    }

    public void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
