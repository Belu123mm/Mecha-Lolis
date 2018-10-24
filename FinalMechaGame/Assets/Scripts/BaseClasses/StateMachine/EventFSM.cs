using System.Collections.Generic;
using System;

public class EventFSM<TFeed> {
	public State<TFeed> current;

	//Constructor
	public EventFSM( State<TFeed> initialState )
	{
		current = initialState;
		current.OnEnter();
	}

	public void Update()
	{
		current.OnUpdate();
	}

	public void Feed( TFeed feed )
	{
		var next = current.GetTransition(feed);
		if ( next != null )
		{
			current.OnExit();
			next.OnEnter();
			current = next;
		}
	}
}