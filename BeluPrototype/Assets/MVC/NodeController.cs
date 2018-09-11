using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NodeController : IControl {

    public event Action<Input> OnInput = delegate { };
    private Model m;
    public NodeController( Model model ) {
        m = model;
        OnInput += Input => m.stateMachine.Feed(Input);
    }
    public void OnUpdate() {
        m.stateMachine.Update();

    }
}
