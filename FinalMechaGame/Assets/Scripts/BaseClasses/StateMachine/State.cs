using System.Collections.Generic;
using System;

public class State<TFeed> {
	public string name;

	public Action OnEnter = delegate { };
	public Action OnUpdate = delegate { };
	public Action OnExit = delegate { };

	private Dictionary<TFeed, State<TFeed>> transitions = new Dictionary<TFeed, State<TFeed>>();

	//Constructor
	public State( string name )
	{
		this.name = name;
	}

	public void AddTransition( TFeed feed, State<TFeed> next )
	{
		transitions [ feed ] = next;
	}

	public State<TFeed> GetTransition( TFeed feed )
	{
		if ( transitions.ContainsKey(feed) ) return transitions [ feed ];
		return null;
	}
}