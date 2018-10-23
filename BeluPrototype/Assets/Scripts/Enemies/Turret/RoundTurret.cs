using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(RoundBullets))]
[RequireComponent(typeof(Sight))]

public class RoundTurret : Turret, IEnemy {
    public EventFSM<TurretState> stateMachine;
    public event Action<TurretState> OnInput = delegate { };
    public TurretVFX vfx;

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


        if ( !hasNavigation ) {
            if ( sight.GetSight() )
                OnInput(TurretState.shooting);
            else
                OnInput(TurretState.quiet);
        } else if ( navigation.currentNode ) {
            if ( navigation.IsOnDistance() ) {
                OnInput(TurretState.iterate);
            } else
            if ( sight.GetSight() ) {
                OnInput(TurretState.shooting);
            } else {
                OnInput(TurretState.moving);
            }
        }
        stateMachine.Update();
        Debug.Log(stateMachine.current.name);
        Debug.Log(hasNavigation);


    }

    public void SetStateMachine() {
        var iterate = new State<TurretState>("ITERATE");
        var quiet = new State<TurretState>("QUIET");
        var shooting = new State<TurretState>("SHOOT");
        var moving = new State<TurretState>("MOVING");

        quiet.AddTransition(TurretState.shooting, shooting);
        quiet.AddTransition(TurretState.moving, moving);
        quiet.AddTransition(TurretState.iterate, iterate);

        shooting.AddTransition(TurretState.quiet, quiet);
        shooting.AddTransition(TurretState.moving, moving);
        shooting.AddTransition(TurretState.iterate, iterate);

        moving.AddTransition(TurretState.moving, shooting);
        moving.AddTransition(TurretState.iterate, iterate);

        iterate.AddTransition(TurretState.moving, moving);
        iterate.AddTransition(TurretState.shooting, shooting);


        quiet.OnUpdate += () => Quiet();
        quiet.OnEnter += () => vfx.ClearAnimations();
        quiet.OnEnter += () => vfx.Quiet();


        shooting.OnUpdate += () => Shoot();
        shooting.OnUpdate += () => vfx.OnMovement(navigation.speed);

        moving.OnEnter = () => vfx.ClearAnimations();
        moving.OnUpdate += () => navigation.MoveToNext();
        moving.OnUpdate += () => vfx.OnMovement(navigation.speed);


        if ( hasNavigation ) {
            iterate.OnEnter += () => navigation.NextNode();
            iterate.OnEnter += () => vfx.ClearAnimations();
            iterate.OnEnter += () => vfx.Rotate();
        }


        stateMachine = new EventFSM<TurretState>(quiet);

    }
    public void Shoot() {
        if ( timer > timetoshoot ) {
            bulletGenerator.target = target;
            bulletGenerator.Shoot();
            timer = 0;
            vfx.Shooting();
        }
        if ( hasNavigation )
            navigation.mOVEbUTsLOWER();

    }
    public void Quiet() {

    }


}
public enum TurretState {
    shooting,
    quiet,
    moving,
    iterate,
    dying
}