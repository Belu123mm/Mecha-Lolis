﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
[ExecuteInEditMode]
public class EnemyBullet : MonoBehaviour {

    public delegate void OnMovement();
    public OnMovement Movement;
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
    public LayerMask DamagableLayer;
    public int damage;

    // Use this for initialization
    void Start() {
        if ( !Application.isPlaying )
            Selection.activeGameObject = this.gameObject;

    }
    void Update() {
        Movement();
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(center, 1);
    }
    public void OnTriggerEnter( Collider other ) {
        var obj = other.gameObject;
        if ( obj.layer == DamagableLayer ) {
            obj.GetComponent<IDamageable>().AddDamage(damage);
            Destroy(this.gameObject);
        }

    }
}