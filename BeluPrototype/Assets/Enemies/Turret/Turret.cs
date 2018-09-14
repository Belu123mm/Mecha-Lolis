using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public BulletGroup bulletgroup;
    public Transform target;
    public float timetoshoot;
    public AreaController control;
	// Use this for initialization
	void Start () {
        ModelTurret _m = new ModelTurret(target, timetoshoot, bulletgroup);
        control = new AreaController(_m);

    }

    // Update is called once per frame
    void Update () {
        control.OnUpdate();
        control.GetTarget(target);
    }
}
