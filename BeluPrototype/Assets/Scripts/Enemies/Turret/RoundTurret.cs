using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(RoundBullets))]
[RequireComponent(typeof(Sight))]

public class RoundTurret : Turret, IEnemy {
    public EventFSM<TurretState> stateMachine;
    public event Action<TurretState> OnInput = delegate { };

    float timer;
    RoundBullets bulletGenerator;

    public void Awake() {
        bulletGenerator = GetComponent<RoundBullets>();
    }
    public override void Start() {
        base.Start();
        SetStateMachine();
        OnInput += Input => stateMachine.Feed(Input);
        bulletGenerator.target = target;

    }
    public void Update() {
        timer += Time.deltaTime;

        if ( sight.GetSight() ) {
            OnInput(TurretState.shooting);
        } else {
            OnInput(TurretState.quiet);
        }

        stateMachine.Update();

    }

    public void SetStateMachine() {
        var quiet = new State<TurretState>("QUIET");
        var shooting = new State<TurretState>("SHOOT");

        quiet.AddTransition(TurretState.shooting, shooting);

        shooting.AddTransition(TurretState.quiet, quiet);

        quiet.OnUpdate += () => Quiet();

        shooting.OnUpdate += () => Shoot();

        stateMachine = new EventFSM<TurretState>(quiet);

    }
    public void Shoot() {
        if ( timer > timetoshoot ) {
            bulletGenerator.target = target;
            bulletGenerator.Shoot();
            timer = 0;
        }
    }
    public void Quiet() {

    }


}
public enum TurretState {
    shooting,
    quiet,
    moving,
    iterate
}