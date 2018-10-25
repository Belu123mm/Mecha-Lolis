using UnityEngine;
using System;
using System.Collections;
[RequireComponent(typeof(SimpleBullets))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Sight))]

public class SimpleTurret : Turret, IEnemy, IDamageable {
    public EventFSM<TurretState> stateMachine;
    public event Action<TurretState> OnInput = delegate { };
    SimpleBullets bulletGenerator;
    public TurretVFX vfx;

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
        var dying = new State<TurretState>("DYING");

        moving.AddTransition(TurretState.shooting, shooting);
        moving.AddTransition(TurretState.iterate, iterate);
        moving.AddTransition(TurretState.dying, dying);

        shooting.AddTransition(TurretState.moving, moving);
        shooting.AddTransition(TurretState.iterate, iterate);
        shooting.AddTransition(TurretState.quiet, quiet);
        shooting.AddTransition(TurretState.dying, dying);

        iterate.AddTransition(TurretState.moving, moving);
        iterate.AddTransition(TurretState.shooting, shooting);
        iterate.AddTransition(TurretState.dying, dying);

        quiet.AddTransition(TurretState.shooting, shooting);
        quiet.AddTransition(TurretState.dying, dying);

        moving.OnUpdate += () => navigation.MoveToNext();
        moving.OnUpdate += () => vfx.OnMovement(navigation.rigidbody.velocity.magnitude);

        shooting.OnUpdate = () => Shoot();

        if ( hasNavigation ) {
            iterate.OnEnter += () => navigation.NextNode();
            iterate.OnEnter += () => vfx.Rotate();
        }

        quiet.OnEnter = () => vfx.Quiet();

        dying.OnEnter = () => StartCoroutine(Dying());

        stateMachine = new EventFSM<TurretState>(iterate);
    }

    // Update is called once per frame
    void Update() {
        if ( !hasNavigation ) {
            if ( sight.isEnemyOnSigh() )
                OnInput(TurretState.shooting);
            else
                OnInput(TurretState.quiet);
        } else if ( navigation.currentNode ) {
            if ( navigation.IsOnDistance() )
                OnInput(TurretState.iterate);
            else if ( sight.isEnemyOnSigh() )
                OnInput(TurretState.shooting);
            else
                OnInput(TurretState.moving);
        }
        stateMachine.Update();
    }
    public void Shoot() { //TODO corutina de esto xd
        timer += Time.deltaTime;
        if ( timer > timetoshoot ) {
            bulletGenerator.Shoot();
            timer = 0;
            vfx.Shooting();
        }
        if ( hasNavigation )
            navigation.mOVEbUTsLOWER();
    }
    IEnumerator Dying() {
        GetComponent<Rigidbody>().useGravity = true;
        vfx.Dying();
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);

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