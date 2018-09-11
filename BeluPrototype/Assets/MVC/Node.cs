using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour{
    public Node next;
    public Node previous;

    public Color color = Color.green;

    public void OnDrawGizmos() {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 1);
    }
    public Vector3 position() {
        return transform.position;
    }

}
