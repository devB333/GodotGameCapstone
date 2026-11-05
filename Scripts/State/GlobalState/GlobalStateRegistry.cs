using Godot;
using System;
using System.Collections.Generic;

public partial class GlobalStateRegistry : Node
{// AutoLoad Singletion
	public Dictionary<Guid, IStateParent> stateParents { get; private set;}
	public static GlobalStateRegistry instance { get; private set;}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()// this will need to load from worldState serialization system as well
	{
		instance = this;
		this.stateParents = new Dictionary<Guid, IStateParent>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
