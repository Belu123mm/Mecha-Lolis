
using UnityEngine;
using System;

public class KeyboardController : IControl{
    public event Action<DroneState> OnInput = delegate { };
    private ModelDrone m;
    public KeyboardController(ModelDrone model) {
        m = model;
        OnInput += Input => m.stateMachine.Feed(Input);
    }
    public void OnUpdate() {
        m.stateMachine.Update();

    }
}
