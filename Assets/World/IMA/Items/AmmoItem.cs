using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : MonoBehaviour, IInteractable
{
    public float time { get; set; }
    public float setTime;
    public float ammoBonus;

    void Start()
    {
        time = setTime;
    }

    public void interact()
    {
        IWeapon weapon = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<IWeapon>();
        weapon.ammoTotal += ammoBonus;
        AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, transform.position);
        Destroy(gameObject);
    }
}
