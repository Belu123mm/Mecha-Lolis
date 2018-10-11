using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AreaController : IControl
{

    public event Action<TurretState> OnInput = delegate { };
    private ModelTurret m;

    public AreaController(ModelTurret model)
    {
        m = model;
        OnInput += Input => m.stateMachine.Feed(Input);
    }
    public void OnUpdate()

    {
        m.timer += Time.deltaTime;
        if (m.target)
        {
            OnInput(TurretState.shooting);
        }
        else
        {
            OnInput(TurretState.quiet);
        }

        m.stateMachine.Update();

    }
    public void GetTarget(Transform t)
    {
        m.target = t;
    }
}
public enum TurretState
{
    shooting,
    quiet
}
