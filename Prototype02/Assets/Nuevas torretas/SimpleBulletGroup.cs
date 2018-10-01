using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBulletGroup : BulletGroup {


    public Transform canon;
    public Bullet circle;
    public float speed;

    public override void Shoot()
    {
        Bullet bullets = Instantiate(circle, canon.position, Quaternion.identity);

        Vector3 direction = -bullets.transform.position + target.transform.position;
        bullets.transform.forward = direction;
        bullets.Movement += () => Moving(bullets.transform);
    }
    public void Moving(Transform t) {
        t.position += t.forward * Time.deltaTime * speed;
    }
}
