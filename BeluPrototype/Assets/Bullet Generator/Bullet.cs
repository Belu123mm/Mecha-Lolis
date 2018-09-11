using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speedOscilation;
    public Vector3 center;
    public int bulletsCount;


    public int numberOfBullets;
    [Range(0, 360)]
    public int angle;
    public GameObject circle;
    public float degrees;
    public float radians;
    public float radiusX;
    public float radiusY;
    public float speed;

    // Use this for initialization
    void Start () {
		
	}
    void Update() {
        degrees += 1;

        center += this.transform.forward * speed * Time.deltaTime;

        degrees += speedOscilation * Time.deltaTime;
        radians = degrees * Mathf.Deg2Rad;

        Vector3 realPosition;
        realPosition.x = Mathf.Cos(radians) * radiusX;
        realPosition.y = 0;
        realPosition.z = Mathf.Sin(radians) * radiusY;

        this.transform.position = center + realPosition;

        this.transform.position += this.transform.forward * Time.deltaTime * speed;
        
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(center, 1);
    }

}
