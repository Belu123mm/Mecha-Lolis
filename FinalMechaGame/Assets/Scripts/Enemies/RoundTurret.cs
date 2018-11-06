using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(RoundBullets))]
[RequireComponent(typeof(Sight))]

public class RoundTurret : Turret, IEnemy, IDamageable {
    public EventFSM<TurretState> stateMachine;
    public event Action<TurretState> OnInput = delegate { };
    public TurretVFX vfx;

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

    //Esto hay que chequearlo.
    public void Update() {
        timer += Time.deltaTime;
        if ( !hasNavigation )
        {
            if ( sight.isEnemyOnSigh() )
                OnInput(TurretState.shooting);
            else
                OnInput(TurretState.quiet);
        }
        else if ( navigation.currentNode )
        {
            if ( navigation.IsOnDistance() )
                OnInput(TurretState.iterate);
            else if ( sight.isEnemyOnSigh() )
                    OnInput(TurretState.shooting);
            else OnInput(TurretState.moving);
        }
        stateMachine.Update();
        state = stateMachine.current.name;
    }

    public void SetStateMachine() {
        var iterate = new State<TurretState>("ITERATE");
        var quiet = new State<TurretState>("QUIET");
        var shooting = new State<TurretState>("SHOOT");
        var moving = new State<TurretState>("MOVING");
        var dying = new State<TurretState>("DYING");

        quiet.AddTransition(TurretState.shooting, shooting);
        quiet.AddTransition(TurretState.moving, moving);
        quiet.AddTransition(TurretState.iterate, iterate);
        quiet.AddTransition(TurretState.dying, dying);

        shooting.AddTransition(TurretState.quiet, quiet);
        shooting.AddTransition(TurretState.moving, moving);
        shooting.AddTransition(TurretState.iterate, iterate);
        shooting.AddTransition(TurretState.dying, dying);

        moving.AddTransition(TurretState.moving, shooting);
        moving.AddTransition(TurretState.iterate, iterate);
        moving.AddTransition(TurretState.dying, dying);

        iterate.AddTransition(TurretState.moving, moving);
        iterate.AddTransition(TurretState.shooting, shooting);
        iterate.AddTransition(TurretState.dying, dying);


        quiet.OnEnter += () => vfx.ClearAnimations();
        quiet.OnUpdate += () => Quiet();
        quiet.OnUpdate += () => vfx.Quiet();

        shooting.OnEnter += () => vfx.ClearAnimations();
        shooting.OnUpdate = () => Shoot();
        shooting.OnExit += () => vfx.ClearAnimations();
        shooting.OnExit += () => StopCoroutine(Shooting());
        //Aqui lo hace una vez uwu

        moving.OnEnter = () => vfx.ClearAnimations();
        moving.OnUpdate += () => navigation.MoveToNext();
        moving.OnUpdate += () => vfx.OnMovement(navigation.speed);


        if ( hasNavigation ) {
            iterate.OnEnter += () => navigation.NextNode();
            iterate.OnEnter += () => vfx.ClearAnimations();
            iterate.OnEnter += () => vfx.Rotate();
        }

        dying.OnEnter = () => StartCoroutine(Dying());

        stateMachine = new EventFSM<TurretState>(quiet);

    }
    public void Shoot() {
        if ( timer > timetoshoot ) {
            StartCoroutine(Shooting());
            timer = 0;
        }
        if ( hasNavigation ) {
            navigation.mOVEbUTsLOWER();
            vfx.OnMovement(navigation.speed * navigation.percent);
        } else
            vfx.Quiet();


    }
    IEnumerator Shooting() {
        for ( int i = 0; i < burstQuantity; i++ ) {
            bulletGenerator.Shoot();
            vfx.Shooting();
            yield return new WaitForSeconds(timeBtBurst);
        }

    }

    public void Quiet() {


    }

    IEnumerator Dying() {
        var rb = GetComponent<Rigidbody>();
        if ( rb ) {
            rb.velocity = Vector3.zero;
        }
        Collider[] cls = GetComponents<Collider>();
        foreach ( var c in cls ) {
            c.enabled = false;
        }
        vfx.Dying();
        yield return new WaitForSeconds(4);
        Destroy(this.gameObject);
    }


    public void AddDamage(int Damage)
    {
        Life -= Damage;
        vfx.OnDamage();
        if (Life <= 0)
        {
            GameModelManager.instance.Points += 10;
            GameModelManager.instance.UpdatePoints();
            print(gameObject.name + " Se ha morido");
            OnInput(TurretState.dying);
        }
        //print(name + " ha recibido " + Damage + " puntos de daño!");
    }

}

public enum TurretState {
    shooting,
    quiet,
    moving,
    iterate,
    dying
}