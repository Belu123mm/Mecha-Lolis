using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : MonoBehaviour{
    public Node next;
    public Node previous;
    public int id;
    public Color color = Color.green;

    public void OnDrawGizmos() {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 1);
        if (next) Gizmos.DrawLine(transform.position, next.transform.position);
    }
    public Vector3 position {
        get {
            return transform.position;
        }
    }
    public float Distance(Vector3 v3) {
        return Vector3.Distance(v3, position);
    }
}
