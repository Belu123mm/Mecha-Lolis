using UnityEngine;
using System;

[RequireComponent(typeof(RoundBullets))]
[RequireComponent(typeof(Sight))]

public class RoundTurret : Turret, IEnemy, IDamageable {
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


        quiet.OnEnter += () => vfx.ClearAnimations();
        quiet.OnEnter += () => vfx.Quiet();
        quiet.OnUpdate += () => Quiet();
        quiet.OnUpdate += () => vfx.Quiet();

        shooting.OnEnter = () => vfx.ClearAnimations();
        shooting.OnUpdate += () => Shoot();
        shooting.OnExit = () => vfx.ClearAnimations();

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
            bulletGenerator.Shoot();
            timer = 0;
            vfx.Shooting();
        }
        if ( hasNavigation ) {
            navigation.mOVEbUTsLOWER();
            vfx.OnMovement(navigation.speed * navigation.percent);
        }else 
            vfx.Quiet();
    }
    public void Quiet() {

    }

    public void AddDamage(int Damage)
    {
        Life -= Damage;
        if (Life <= 0)
        {
            GameModelManager.instance.Points += 10;
            GameModelManager.instance.UpdatePoints();
            print(gameObject.name + " Se ha morido");
            OnInput(TurretState.dying);
        }
        print(name + " ha recibido " + Damage + " puntos de daño!");
    }
}

public enum TurretState {
    shooting,
    quiet,
    moving,
    iterate,
    dying
}