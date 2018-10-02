using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFSM<TFeed> {

    public State<TFeed> current;

    public void Update() {
        current.OnUpdate();
    }

    public void Feed( TFeed feed ) {
        var next = current.GetTransition(feed);

        if ( next != null ) {
            current.OnExit();
            next.OnEnter();

            current = next;
        }
    }

    public EventFSM( State<TFeed> initialState ) {
        current = initialState;
        current.OnEnter();
    }

}