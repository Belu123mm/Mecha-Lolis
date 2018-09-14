using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelTurret
{
    public EventFSM<TurretState> stateMachine;

    public Transform target;
    public float timer;
    public float timetoshot;
    public BulletGroup bulletGroup;
    public ModelTurret(Transform t, float tts,BulletGroup bg)
    {
        target = t;
        bulletGroup = bg;
        bulletGroup.target = target;
        timetoshot = tts;
        SetStateMachine();
    }
    public void SetStateMachine()
    {
        var quiet = new State<TurretState>("QUIET");
        var shooting = new State<TurretState>("SHOOT");

        quiet.AddTransition(TurretState.shooting, shooting);

        shooting.AddTransition(TurretState.quiet, quiet);

        quiet.OnUpdate += () => Quiet();

        shooting.OnUpdate += () => Shoot();

        stateMachine = new EventFSM<TurretState>(quiet);

    }
    public void Shoot()
    {
        if (timer > timetoshot)
        {
            bulletGroup.target = target;
            bulletGroup.Shoot();
            timer = 0;
        }
    }
    public void Quiet()
    {

    }
    
}
