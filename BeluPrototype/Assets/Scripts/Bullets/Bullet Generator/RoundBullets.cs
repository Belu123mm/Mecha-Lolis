using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundBullets : BulletGroup
{
    public int numberOfBullets;
    public Transform canon;
    [Range(0, 360)]
    public EnemyBullet circle;
    float degrees;
    public float radians;
    public float radiusX;
    public float radiusY;
    public float variation;

    [Tooltip("It has to be VERY high, like 100+")]
    public float speedOscilation;
    public Vector3 center;

    public float speed;


    public override void Shoot()
    {
        degrees += variation;

        Debug.Log(transform.forward);
        for (int i = 1; i < 360; i += 360 / numberOfBullets)
        {

            EnemyBullet bullets = Instantiate(circle,canon.position,Quaternion.identity);
            Vector3 direction;
            direction.x = Mathf.Cos((i + degrees) * Mathf.Deg2Rad);
            direction.y =
            direction.z = Mathf.Tan((i + degrees) * Mathf.Deg2Rad);

            bullets.transform.forward = direction;
            bullets.degrees = degrees;
            bullets.radians = radians;
            bullets.radiusX = radiusX;
            bullets.radiusY = radiusY;
            bullets.center = canon.position;
            bullets.speedOscilation = speedOscilation;

            bullets.Movement += () => Moving(bullets);

            //Debug
            Debug.DrawRay(transform.position, bullets.transform.forward, Color.green);
        }

    }
    public void Update() {

    }
    public void Moving(EnemyBullet b)
    {
        b.center += b.transform.forward * speed * Time.deltaTime;

        b.degrees += speedOscilation * Time.deltaTime;
        b.radians = b.degrees * Mathf.Deg2Rad;

        Vector3 realPosition;
        realPosition.x = Mathf.Cos(b.radians) * radiusX / 10;
        realPosition.y = 0;
        realPosition.z = Mathf.Sin(b.radians) * radiusY / 10;

        b.transform.position = b.center + realPosition;

        //b.transform.position += b.transform.forward * Time.deltaTime * speed;
    }


}

