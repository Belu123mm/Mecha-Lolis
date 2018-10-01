using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(NodeNavigation))]
[RequireComponent(typeof(SimpleBulletGroup))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Sight))]

public class SimpleTurret : MonoBehaviour {
    public EventFSM<TurretState> stateMachine;
    public event Action<TurretState> OnInput = delegate { };
    public float timeToShoot;
    public NodeNavigation navigation;
    public SimpleBulletGroup group;
    public Transform target;
    public new Rigidbody rigidbody;
    public Sight sight;
    public float distance;
    float timer;
    // Use this for initialization
    void Start () {
        navigation = GetComponent<NodeNavigation>();
        group = GetComponent<SimpleBulletGroup>();
        rigidbody = GetComponent<Rigidbody>();
        sight = GetComponent<Sight>();

        group.target = target;
        sight.targetTransform = target;
        navigation.rigidbody = rigidbody;
        SetStateMachine();
        OnInput += Input => stateMachine.Feed(Input);

    }

    private void SetStateMachine() {
        var moving = new State<TurretState>("MOVE");
        var shooting = new State<TurretState>("SHOOT");
        var iterate = new State<TurretState>("ITERATE");

        moving.AddTransition(TurretState.shooting, shooting);
        moving.AddTransition(TurretState.iterate, iterate);
        shooting.AddTransition(TurretState.moving, moving);
        shooting.AddTransition(TurretState.iterate, iterate);
        iterate.AddTransition(TurretState.moving, moving);
        iterate.AddTransition(TurretState.shooting, shooting);

        moving.OnUpdate = () => navigation.MoveToNext();

        shooting.OnUpdate = () => Shooting();

        iterate.OnEnter = () => navigation.NextNode();

        stateMachine = new EventFSM<TurretState>(iterate);

    }

    // Update is called once per frame
    void Update () {
        if ( navigation.currentNode ) {
            if ( navigation.IsOnDistance(distance) ) {
                OnInput(TurretState.iterate);
            }else
            if ( sight.GetSight() ) {
                OnInput(TurretState.shooting);
            }else {
            OnInput(TurretState.moving);
            }
        }
        stateMachine.Update();
	}
    void Shooting() { //TODO corutina de esto xd
        timer += Time.deltaTime;
        if (timer > timeToShoot ) {
            group.Shoot();
            timer = 0;
        }
        navigation.mOVEbUTsLOWER();
    }
}
