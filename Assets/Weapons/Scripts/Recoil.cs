using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    Transform vertRotation;
    public float recoilSpeed = 2;
    public float recoilTimer = 0;

    void Start()
    {
        vertRotation = GameObject.FindGameObjectWithTag("Player").transform.Find("VerticalRotation");
    }

    void Update()
    {
        if (recoilTimer > 0)
        {
            vertRotation.Rotate(-recoilSpeed * Time.deltaTime, 0, 0);
            recoilTimer -= Time.deltaTime;
        }
        else
        {
            recoilTimer = 0;
        }
    }

    public void bulletFired(float time)
    {
        recoilTimer += time;
    }
}
