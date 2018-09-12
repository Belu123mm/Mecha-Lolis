using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour {
    public Rigidbody rb;
    public ViewDrone view;//Pedimos un view
    public IControl control;
    public float speed;
    public NodeGroup group;

    public void Awake() {
        ModelDrone _m = new ModelDrone(speed,rb,group);//Creo un modelo,y le paso el transform que va a utilizar
        control = new NodeController(_m);

    }
    void Update() {
        control.OnUpdate();
    }

}
