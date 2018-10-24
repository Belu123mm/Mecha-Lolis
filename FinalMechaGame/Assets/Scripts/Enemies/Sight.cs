using UnityEngine;

public class Sight : MonoBehaviour {
    public int viewAngle;
    public float viewDistance;

    private Vector3 _dirToTarget;
    private float _angleToTarget;
    private float _distanceToTarget;
    [HideInInspector]
    public Transform targetTransform;
    public LayerMask level;

    // Update is called once per frame
    void Update() {
        _dirToTarget = (targetTransform.position - transform.position).normalized;
        _angleToTarget = Vector3.Angle(transform.forward, _dirToTarget);
        _distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
    }

    public bool isEnemyOnSigh() {
        if ( _angleToTarget <= viewAngle && _distanceToTarget <= viewDistance ) {
            RaycastHit rch;
            bool obstaclesBetween = false;

            //Se hace un chequeo de colisiones
            if ( Physics.Raycast(transform.position, _dirToTarget, out rch, _distanceToTarget) )
                if ( rch.collider.gameObject.layer == level )
                    obstaclesBetween = true;
            return !obstaclesBetween;
        }
        return false;
    }

    public void OnDrawGizmos() {
        /*
        Dibujamos una línea desde el NPC hasta el enemigo.
        Va a ser de color verde si lo esta viendo, roja sino.
        */
        if ( isEnemyOnSigh() )
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        if (Application.isPlaying)
        Gizmos.DrawLine(transform.position, targetTransform.position);

        /*
        Dibujamos los límites del campo de visión.
        */
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * viewDistance));

        Vector3 rightLimit = Quaternion.AngleAxis(viewAngle, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + (rightLimit * viewDistance));

        Vector3 leftLimit = Quaternion.AngleAxis(-viewAngle, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + (leftLimit * viewDistance));
    }
}