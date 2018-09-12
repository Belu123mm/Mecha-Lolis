using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelDrone : Model {
    public float speed;
    public Rigidbody rigidbody;
    public EventFSM<DroneState> stateMachine;
    public NodeGroup nodegroup;
    public Node currentNode;
    float quietsin;

    public ModelDrone( float s, Rigidbody rb, NodeGroup ng ) {
        speed = s;
        rigidbody = rb;
        nodegroup = ng;
        currentNode = nodegroup._first;
        Debug.Log(currentNode);
        SetStateMachine();
    }
    public override void SetStateMachine() {
        var quiet = new State<DroneState>("QUIET");
        var moving = new State<DroneState>("MOVE");
        var alert = new State<DroneState>("ALERT");
        var rotate = new State<DroneState>("ROTATE");
        var search = new State<DroneState>("SEARCH");

        quiet.AddTransition(DroneState.move, moving);
        quiet.AddTransition(DroneState.alert, alert);
        quiet.AddTransition(DroneState.rotate, rotate);

        moving.AddTransition(DroneState.quiet, quiet);
        moving.AddTransition(DroneState.rotate, rotate);
        moving.AddTransition(DroneState.alert, alert);

        alert.AddTransition(DroneState.search, search);

        search.AddTransition(DroneState.quiet, quiet);
        search.AddTransition(DroneState.move, moving);
        search.AddTransition(DroneState.rotate, rotate);
        search.AddTransition(DroneState.alert, alert);

        quiet.OnUpdate += () => Quiet();

        moving.OnUpdate += () => MoveToNext();

        stateMachine = new EventFSM<DroneState>(quiet);
    }

    //FUNCIONES

    public void RotateTo() {
        currentNode = currentNode.next;
        rigidbody.transform.forward = rigidbody.transform.position + currentNode.position;

    }
    public void MoveToNext() {
        rigidbody.velocity = rigidbody.transform.forward * speed;
    }

    public void Quiet() {
        quietsin += Time.deltaTime;
        rigidbody.velocity = new Vector3(0, Mathf.Cos(quietsin * 2) + 0.25f, 0);
    }
    public Vector3 position {
        get {
            return rigidbody.transform.position;
        }
    }


}
