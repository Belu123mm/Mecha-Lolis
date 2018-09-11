
using UnityEngine;
using System;

public class KeyboardController : IControl{
    public event Action<Input> OnInput = delegate { };
    private Model m;
    public KeyboardController(Model model) {
        m = model;
        OnInput += Input => m.stateMachine.Feed(Input);
    }
    public void OnUpdate() {
        m.stateMachine.Update();

    }
}
