using Godot;
using System;

public partial class OpenDoor : IState, IState.IOptionalTriggerStateChange
{
	
	public override void _Ready()
	{
		this.name = "open";
	}
	
	public override void Enter()
	{
		if (node is Node3D node3D)
		{
			node3D.Quaternion = new Quaternion(new Vector3(0, 1, 0), Mathf.DegToRad(90f));
		}
	}

	public override void Exit()
	{
		
	}

	public override void Process()
	{
		
	}

	public void triggerStateChange()
	{
		if (node is IStateParent stateParent)
		{
			base.switchState("CloseDoor");
		}
	}
}
