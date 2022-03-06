using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI AmmoCount;
    public TextMeshProUGUI TotalAmmo;
    public Image WeaponIcon;
    public Image Crosshair;
    public Slider HealthBar;
    public Image Interact;
    public GameObject Objectives;
    public TextMeshProUGUI objPopupTitle;
    public TextMeshProUGUI objPopupSub;
    public GameObject reloadPopUp;

    [Header("Info")]
    public float popupTime;
    float lastPopup;
    GameObject player;
    public float health, maxHealth, ammo, totalAmmo, magTotal, mapTime = 0;
    public bool interactInProgress, showingObjectiveScreen = false;
    float t, time;
    IGameController gameController;

    void Start()
    {
        gameController = GetComponent<IGameController>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //Update info
        health = player.GetComponent<PlayerInfo>().health;
        maxHealth = player.GetComponent<PlayerInfo>().maxHealth;
        ammo = player.GetComponentInChildren<IWeapon>().ammoInMag;
        magTotal = player.GetComponentInChildren<IWeapon>().ammoMagTotal;
        totalAmmo = player.GetComponentInChildren<IWeapon>().ammoTotal;

        //Calculate health percent
        float healthPercent = health / maxHealth;

        //Update HUD
        AmmoCount.text = ammo.ToString();
        TotalAmmo.text = totalAmmo.ToString();
        HealthBar.value = healthPercent;
        if(ammo == magTotal) 
            reloadPopUp.SetActive(false);
        else reloadPopUp.SetActive(true);

        //Show ojbective screen?
        Objectives.SetActive(showingObjectiveScreen);

        //Objective popup
        if(gameController.time > lastPopup + popupTime) objPopupTitle.transform.parent.gameObject.SetActive(false);

        //Interact update
        if (interactInProgress)
        {;
            t += Time.deltaTime;
            float fill = t / time;
            Interact.fillAmount = fill;
            if (t >= time)
            {
                interactInProgress = false;
                Interact.fillAmount = 0;
            }
        }

        //Map time
        if(GameObject.Find("Map").transform.GetChild(0).gameObject.activeSelf)
        {
            mapTime += Time.deltaTime;
        }
    }

    public void startInteractTimer(float time)
    {
        interactInProgress = true;
        t = 0;
        this.time = time;
    }

    public void stopInteractTimer()
    {
        interactInProgress = false;
        Interact.fillAmount = 0;
    }

    public void ObjectivePopup(string name, float count, float goal)
    {
        lastPopup = gameController.time;
        objPopupTitle.text = name;
        objPopupSub.text = "Progress: " + count + "/" + goal + "\n(" + count / goal * 100 + "%)";
        objPopupTitle.transform.parent.gameObject.SetActive(true);
    }
}
