using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGroup : MonoBehaviour {

    public int numberOfBullets;
    [Range(0,360)]
    public int angle;
    public Bullet circle;
    public float degrees;
    public float radians;
    public float radiusX;
    public float radiusY;

    [Tooltip("It has to be VERY high, like 100+")]
    public float speedOscilation;
    public Vector3 center;
    public int bulletsCount;

    public float speed;





    // Use this for initialization
    void Start () {
        // la cantidad de balas son las que se van a instancear
        //Las balas son expansivas o es una plancha? 
        //O expansivas en plancha? :V
        //Si son en plancha
        //Cada bala va a acomodarse segun una grilla, con un valor de separacion
        //Van a tener una velocidad
        //Velocidad constante o creciente
        Shoot();        

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Shoot() {
        for ( int i = 1; i < angle; i += angle / numberOfBullets ) {
            print("HEU");
            Bullet bullets = Instantiate(circle);
            Vector3 direction;
            direction.z = Mathf.Cos((i + degrees) * Mathf.Deg2Rad);
            direction.y = 0;
            direction.x = Mathf.Sin((i + degrees) * Mathf.Deg2Rad);

            bullets.transform.forward = direction;
            bullets.degrees = degrees;
            bullets.radians = radians;
            bullets.radiusX = radiusX;
            bullets.radiusY = radiusY;
            bullets.speed = speed;
            bullets.speedOscilation = speedOscilation;

            //Debug
            Debug.DrawRay(transform.position, bullets.transform.forward, Color.green);
        }

    }



// Use this for initialization

// Update is called once per frame
}
