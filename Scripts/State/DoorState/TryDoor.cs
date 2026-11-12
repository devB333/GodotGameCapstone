using Godot;
using System;
using System.Collections.Generic;

public partial class TryDoor : Node3D, IStateParent, IParentTriggerStateChange
{
	public Guid id { get; } =  Guid.NewGuid();// unique guid for Global Registry
	public IState activeState { get; set; } = null;
	[Export]
	public Godot.Collections.Array<IState> states { get; set; }// list that holds all states
	public Dictionary<string, IState> stateNameMap { get; set; }// used for O(1) access
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		stateNameMap = new Dictionary<string, IState>();
		
		foreach (IState state in states)
		{
			state.node = this;
			stateNameMap.Add(state.name, state);
		}

		activeState = stateNameMap["closed"];
		activeState.Enter();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void switch_state(string stateName)
	{
		activeState = stateNameMap[stateName];
		activeState.Enter();
	}

	public void parentTriggerStateChange()
	{
		if (activeState is IState.IOptionalTriggerStateChange stateChange)
		{
			stateChange.triggerStateChange();
		}
	}
}
