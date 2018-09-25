using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    public float timeToFall;
    public Vector3 startPosition;
    public bool startFalling;
    public Rigidbody rb;
    public bool falling;
    public float timer;
    public float timeToRecover;
    public void Start() {
        startPosition = this.transform.position;    
    }
    public void Update() {
        if ( startFalling ) {
            timer += Time.deltaTime;
            if (timer > timeToFall ) {
                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                startFalling = false;
                falling = true;
                timer = 0;
            }
        }
        if ( falling ) {
            timer += Time.deltaTime;
            if ( timer > timeToRecover ) {
                transform.position = startPosition;
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

                falling = false;
                timer = 0;
            }
        }

    }
    public void OnCollisionEnter( Collision collision ) {
        print("Chocked");
        if (collision.gameObject.tag == "Player") {
            startFalling = true;
        }
    }
}
