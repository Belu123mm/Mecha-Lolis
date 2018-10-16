using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBullets : BulletGroup {


    public Transform canon;
    public EnemyBullet circle;
    public float speed;

    public override void Shoot()
    {
        EnemyBullet bullets = Instantiate(circle, canon.position, Quaternion.identity);

        Vector3 direction = -bullets.transform.position + target.transform.position;
        bullets.transform.forward = direction;
        bullets.Movement += () => Moving(bullets.transform);
    }
    public void Moving(Transform t) {
        t.position += t.forward * Time.deltaTime * speed;
    }
}
