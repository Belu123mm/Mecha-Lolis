using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour {

    public bool canGrab;
    public bool grabbed;
    float timer;
    public float timetograb;
    public Bullet bullet;
    private Bullet _grabbed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( canGrab ) {
        timer += Time.deltaTime;
        }

        if ( timer > timetograb ) {
            canGrab = false;
            _grabbed = null;
            timer = 0;
        }

        if ( Input.GetKeyDown(KeyCode.Q) ) {
            if ( grabbed ) {
                grabbed = false;
                canGrab = false;
                _grabbed = null;
                Bullet _b = Instantiate(bullet, this.transform.position, Quaternion.identity);
                _b.transform.forward = this.transform.forward;
                _b.speedOscilation = 10;
                _b.Movement += () => _b.transform.position += _b.speedOscilation * _b.transform.forward * Time.deltaTime;
                
            } else if (canGrab){
                grabbed = true;
                canGrab = false;
                Destroy(_grabbed.gameObject);
            }
        }

	}
    private void LateUpdate() {
    }
    private void OnTriggerEnter( Collider other ) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ballz") ) {
            canGrab = true;
            _grabbed = other.gameObject.GetComponent<Bullet>();
        } 
    }
}
