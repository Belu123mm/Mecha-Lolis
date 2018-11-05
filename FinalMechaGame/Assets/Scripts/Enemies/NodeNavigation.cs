using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeNavigation : MonoBehaviour {
    [HideInInspector]
    public Node currentNode;
    public float speed;
    [HideInInspector]
    public Rigidbody myRigidBody;
    public float percent;
    public float distance;

    public void MoveToNext() {
        myRigidBody.velocity = myRigidBody.transform.forward * speed;
    }
    public void mOVEbUTsLOWER() {
        myRigidBody.velocity = myRigidBody.transform.forward * (speed * percent);
    }

    public bool IsOnDistance() {
        if ( currentNode.Distance(transform.position) < distance ) {
            return true;
        } else
            return false;
    }
    public Vector3 position {
        get {
            return myRigidBody.transform.position;
        }
    }
    public void NextNode() {
        currentNode = currentNode.next;
        myRigidBody.transform.forward = -myRigidBody.transform.position + currentNode.position;
    }

}
