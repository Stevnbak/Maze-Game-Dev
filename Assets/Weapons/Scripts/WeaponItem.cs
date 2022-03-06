using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour, IInteractable
{
    public float time { get; set; }
    public float setTime;
    public ParticleSystem vfx;
    public Sprite icon;
    public bool hovering { get; set; }
    public GameObject inputPopup;
    Transform player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        time = setTime;
        vfx.Play();
    }

    void Update()
    {
        inputPopup.transform.LookAt(player.position);

        if (hovering)
            inputPopup.SetActive(true);
        else inputPopup.SetActive(false);
    }

    public void interact()
    {
        inputPopup.SetActive(false);
        GameObject weaponPos = player.Find("Weapon_Pos").gameObject;
        vfx.Stop();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>().ammo += weaponPos.GetComponentInChildren<IWeapon>().ammoInMag;
        weaponPos.GetComponentInChildren<IWeapon>().Drop(transform);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<HUD>().WeaponIcon.sprite = icon;
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
