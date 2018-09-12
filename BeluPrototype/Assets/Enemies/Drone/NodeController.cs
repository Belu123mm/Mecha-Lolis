using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NodeController : IControl {

    public event Action<DroneState> OnInput = delegate { };
    private ModelDrone m;
    public float timer;     //TODO tal vez corrutina para eso :v
    public bool readytorotate;

    public NodeController( ModelDrone model ) {
        m = model;
        OnInput += Input => m.stateMachine.Feed(Input);
    }
    public void OnUpdate() {
        m.stateMachine.Update();
        if ( m.currentNode ) {
            if ( m.stateMachine.current.name != "QUIET" && m.currentNode.Distance(m.position) > 2 ) {
                OnInput(DroneState.rotate);
            }

            if ( m.currentNode.Distance(m.position) > 2 && m.stateMachine.current.name != "MOVE"  && !readytorotate) {
                OnInput(DroneState.move);
            }
            if ( m.currentNode.Distance(m.position) < 2 && m.stateMachine.current.name == "MOVE" && readytorotate) {
                OnInput(DroneState.rotate);
                readytorotate = false;
            }
            if (m.currentNode.Distance(m.position) > 2 && !readytorotate ) {
                readytorotate = true;
            }

        }
        else {
            OnInput(DroneState.quiet);
        }
        Debug.Log(m.stateMachine.current.name);

    }
}
public enum DroneState {
    quiet,
    move,
    rotate,
    alert,
    search
}

