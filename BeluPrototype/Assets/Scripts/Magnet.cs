using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour {

    public bool inside;
    public Transform magnet;
    public float radius = 5f;
    public float force = 100f;

    void Start() {
        magnet = this.gameObject.transform;
        inside = false;
    }

    public void Update() {
        Collider [] objects = Physics.OverlapSphere(transform.position, radius);
        foreach ( var c in objects ) {
            c.transform.Translate((transform.position - c.transform.position) * Time.deltaTime);
            print(c);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, radius);   
    }
    void OnTriggerEnter( Collider c ) {
        if ( c.gameObject.tag == "Portal" ) {
            inside = true;
        }
    }
    void OnTriggerExit( Collider c ) {
        if ( c.gameObject.tag == "Portal " ) {
            inside = false;
        }

        if ( inside ) {
            Vector3 magnetField = magnet.position - transform.position;
            float index = (radius - magnetField.magnitude) / radius;
            c.GetComponent<Rigidbody2D>().AddForce(force * magnetField * index);
        }
    }
}