using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SimpleBullets))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Sight))]

public class SimpleTurret : Turret, IEnemy {
    public EventFSM<TurretState> stateMachine;
    public event Action<TurretState> OnInput = delegate { };

    SimpleBullets bulletGenerator;

    float timer;
    // Use this for initialization
    public override void Start() {
        base.Start();
        bulletGenerator = GetComponent<SimpleBullets>();
        bulletGenerator.target = target;
        SetStateMachine();
        OnInput += Input => stateMachine.Feed(Input);

    }

    public void SetStateMachine() {
        var moving = new State<TurretState>("MOVE");
        var shooting = new State<TurretState>("SHOOT");
        var iterate = new State<TurretState>("ITERATE");
        var quiet = new State<TurretState>("QUIET");

        moving.AddTransition(TurretState.shooting, shooting);
        moving.AddTransition(TurretState.iterate, iterate);
        shooting.AddTransition(TurretState.moving, moving);
        shooting.AddTransition(TurretState.iterate, iterate);
        shooting.AddTransition(TurretState.quiet, quiet);
        iterate.AddTransition(TurretState.moving, moving);
        iterate.AddTransition(TurretState.shooting, shooting);
        quiet.AddTransition(TurretState.shooting, shooting);

        moving.OnUpdate = () => navigation.MoveToNext();

        shooting.OnUpdate = () => Shoot();

        if(hasNavigation)
        iterate.OnEnter = () => navigation.NextNode();

        stateMachine = new EventFSM<TurretState>(iterate);


    }

    // Update is called once per frame
    void Update() {
        if (!hasNavigation) {
            if ( sight.GetSight() )
                OnInput(TurretState.shooting);
            else
                OnInput(TurretState.quiet);
        }
        else if ( navigation.currentNode ) {
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
   }
    public void Shoot() { //TODO corutina de esto xd
        timer += Time.deltaTime;
        if ( timer > timetoshoot ) {
            bulletGenerator.Shoot();
            timer = 0;
        }
        if (hasNavigation)
        navigation.mOVEbUTsLOWER();
    }

}