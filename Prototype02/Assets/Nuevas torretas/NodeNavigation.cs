using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeNavigation : MonoBehaviour {
    public Node currentNode;
    public float speed;
    public new Rigidbody rigidbody;
    public Sight sight;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void MoveToNext() {
        rigidbody.velocity = rigidbody.transform.forward * speed;
    }


}

public event Action<DroneState> OnInput = delegate { };
private ModelDrone m;
public float timer;     //TODO tal vez corrutina para eso :v
public bool readytorotate;

public NodeController( ModelDrone model ) {
    m = model;
    OnInput += Input => m.stateMachine.Feed(Input);
}
public void OnUpdate() {
    if ( m.currentNode ) {
        if ( m.currentNode.Distance(m.position) < 0.5f ) {
            OnInput(DroneState.rotate);
        } else if ( m.currentNode.gameObject.activeInHierarchy ) {
            OnInput(DroneState.move);
        }
    } else {
        OnInput(DroneState.quiet);
    }

    /*
    if (m.currentNode.Distance(m.position) > 2) {
        OnInput(DroneState.move);
    }
    else if (m.currentNode.Distance(m.position) < 2)
    {
        OnInput(DroneState.rotate);
    }
    */
    m.stateMachine.Update();
}
}
public class ModelDrone {
    public EventFSM<DroneState> stateMachine;
    public NodeGroup nodegroup;
    float quietsin;

    public ModelDrone( float s, Rigidbody rb, NodeGroup ng ) {
        speed = s;
        rigidbody = rb;
        nodegroup = ng;
        currentNode = nodegroup._last;
        SetStateMachine();
    }
    public void SetStateMachine() {
        var quiet = new State<DroneState>("QUIET");
        var moving = new State<DroneState>("MOVE");
        var rotate = new State<DroneState>("ROTATE");
        var alert = new State<DroneState>("ALERT");
        var search = new State<DroneState>("SEARCH");

        quiet.AddTransition(DroneState.move, moving);
        quiet.AddTransition(DroneState.alert, alert);
        quiet.AddTransition(DroneState.rotate, rotate);

        moving.AddTransition(DroneState.quiet, quiet);
        moving.AddTransition(DroneState.rotate, rotate);
        moving.AddTransition(DroneState.alert, alert);

        rotate.AddTransition(DroneState.quiet, quiet);
        rotate.AddTransition(DroneState.move, moving);

        alert.AddTransition(DroneState.search, search);

        search.AddTransition(DroneState.quiet, quiet);
        search.AddTransition(DroneState.move, moving);
        search.AddTransition(DroneState.rotate, rotate);
        search.AddTransition(DroneState.alert, alert);

        quiet.OnUpdate += () => Quiet();

        moving.OnUpdate += () => MoveToNext();

        rotate.OnEnter += () => RotateTo();

        stateMachine = new EventFSM<DroneState>(rotate);
    }

    //FUNCIONES

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
