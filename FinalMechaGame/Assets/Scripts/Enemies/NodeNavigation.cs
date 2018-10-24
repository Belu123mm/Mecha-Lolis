using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeNavigation : MonoBehaviour {
    [HideInInspector]
    public Node currentNode;
    public float speed;
    [HideInInspector]
    public new Rigidbody rigidbody;
    public float percent;
    public float distance;

    public void MoveToNext() {
        rigidbody.velocity = rigidbody.transform.forward * speed;
    }
    public void mOVEbUTsLOWER() {
        rigidbody.velocity = rigidbody.transform.forward * (speed * percent);
    }

    public bool IsOnDistance() {
        if ( currentNode.Distance(transform.position) < distance ) {
            return true;
        } else
            return false;
    }
    public Vector3 position {
        get {
            return rigidbody.transform.position;
        }
    }
    public void NextNode() {
        currentNode = currentNode.next;
        rigidbody.transform.forward = -rigidbody.transform.position + currentNode.position;
    }

}
