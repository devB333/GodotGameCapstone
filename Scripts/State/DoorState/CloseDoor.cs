using Godot;
using System;

public partial class CloseDoor : IState, IState.IOptionalTriggerStateChange
{
	public override void Enter()
	{
		if (node is Node3D node3D)
		{
			node3D.Quaternion = Quaternion.Identity;

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
			base.switchState("OpenDoor");
		}
	}
}
