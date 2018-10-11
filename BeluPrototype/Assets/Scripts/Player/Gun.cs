using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public TPGranade granadeHability;
    public GameObject bullet;
    public GameObject cañon;
    public float recoil;
    public float cooldown;
    public int bulletCount;
    public int maxBullets;

    bool canShoot = false;

    private void Start()
    {
        canShoot = true;

        GameModelManager.instance.AddMouseEvent(InputEventType.Continious, 0, shoot);
        GameModelManager.instance.AddSimpleInputEvent(InputEventType.OnBegin, KeyCode.R,reload);
    }

    // Update is called once per frame
    void Update () {
        SmoothShoot();
	}

    private void reload()
    {
        print("Reloaded");
        bulletCount = maxBullets;
    }

    void SmoothShoot()
    {
        if (!canShoot)
        {
            if (recoil <= 0)
            {
                recoil = cooldown;
                canShoot = true;
                print("Puedo disparar.");
                return;
            }

            recoil -= Time.deltaTime;
        }
    }

    private void shoot()
    {
        if (canShoot && bulletCount > 0)
        {
            bulletCount--;
            canShoot = false;
            var newbullet = Instantiate(bullet);
            newbullet.transform.position = cañon.transform.position;
            newbullet.transform.forward = cañon.transform.forward;
        }
    }
}
