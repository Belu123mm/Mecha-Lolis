using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public TPGranade granadeHability;
    public Animator anim;
    public GameObject bullet;
    public GameObject cañon;
    public float recoil;
    public float cooldown;
    public int bulletCount;
    public int maxBullets;
    public bool canShoot = false;
    public bool lockShoot = false;

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
        anim.SetTrigger("Reload");
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
        if (canShoot && bulletCount > 0 && !lockShoot)
        {
            anim.SetTrigger("Shoot");
            bulletCount--;
            canShoot = false;
            var newbullet = Instantiate(bullet, cañon.transform.position,Quaternion.identity);
            newbullet.transform.forward = cañon.transform.forward;
        }
    }
}
