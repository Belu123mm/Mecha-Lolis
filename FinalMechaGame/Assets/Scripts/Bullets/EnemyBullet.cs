using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[ExecuteInEditMode]
#endif
public class EnemyBullet : MonoBehaviour {
    public static Action<GameObject> OnDeactivate;

    public Action Movement = delegate { };
    public float speedOscilation;
    public Vector3 center;
    public int bulletsCount;

    public int numberOfBullets;
    [Range(0, 360)]
    public int angle;
    public float degrees;
    public float radians;
    public float radiusX;
    public float radiusY;
    public int DamagableLayer;
    public int damage;

    public static void InitializeBullet(GameObject bulletObj)
    {
        bulletObj.gameObject.SetActive(true);
    }
    public static void DeactivateBullet(GameObject bulletObj)
    {
        bulletObj.gameObject.SetActive(false);
        bulletObj.GetComponent<TrailRenderer>().Clear();
        bulletObj.transform.position = Vector3.zero;
        bulletObj.transform.rotation = Quaternion.identity;
    }


    void Start() {
#if UNITY_EDITOR
        if ( !Application.isPlaying )
            Selection.activeGameObject = gameObject;
#endif
    }
    void Update() {
        Movement();
    }

    public void OnTriggerEnter( Collider other ) {
        var obj = other.gameObject;
        if (obj.layer == 0) OnDeactivate(gameObject); //Si la bala coliciona con el mundo.
        //Si coliciona con el player.
        if ( obj.tag == "Player")
        {
            OnDeactivate(gameObject);
            obj.GetComponentInParent<IDamageable>().AddDamage(damage);
        }
    }

    #region Visualización en Editor.
    //private void OnDrawGizmos() {
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawSphere(center, 1);
    //}
    #endregion
}