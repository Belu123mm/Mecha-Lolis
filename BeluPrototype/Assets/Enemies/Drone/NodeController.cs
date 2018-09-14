using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NodeController : IControl {

    public event Action<DroneState> OnInput = delegate { };
    private ModelDrone m;
    public float timer;     //TODO tal vez corrutina para eso :v
    public bool readytorotate;

    public NodeController(ModelDrone model) {
        m = model;
        OnInput += Input => m.stateMachine.Feed(Input);
    }
    public void OnUpdate() {
        if (m.currentNode)
        {
            if (m.currentNode.Distance(m.position) < 0.5f)
            {
                OnInput(DroneState.rotate);
            }
            else if (m.currentNode.gameObject.activeInHierarchy)
            {
                OnInput(DroneState.move);
            }
        }
        else
        {
            OnInput(DroneState.quiet);
        }

        /*
        if (m.currentNode.Distance(m.position) > 2) {
            OnInput(DroneState.move);
        }
        else if (m.currentNode.Distance(m.position) < 2)
        {
            OnInput(DroneState.rotate);
        }
        */
        m.stateMachine.Update();
    }
}
public enum DroneState {
    quiet,
    move,
    rotate,
    alert,
    search
}

