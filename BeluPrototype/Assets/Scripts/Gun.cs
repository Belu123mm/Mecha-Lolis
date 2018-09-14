using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
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
    }

    // Update is called once per frame
    void Update () {
        SmoothShoot();

        if (Input.GetMouseButton(0) && bulletCount > 0)
        {
            if (canShoot)
                shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            reload();
        }
	}

    private void reload()
    {
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
        bulletCount--;
        canShoot = false;
        var newbullet = Instantiate(bullet);
        newbullet.transform.position = cañon.transform.position;
        newbullet.transform.forward = cañon.transform.forward;
    }
}
