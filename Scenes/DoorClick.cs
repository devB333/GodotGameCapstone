using Godot;
using System;

public partial class DoorClick : Anchor
{
	private Camera3D camera;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		Hide();
		TargetAnchor = GetNode<Marker3D>("/root/Dev/TryDoor/Marker3D");
		globalStateRegistry = GlobalStateRegistry.instance;
		target = GetNode<IStateParent>("/root/Dev/TryDoor").id;
		camera = GetViewport().GetCamera3D();
		
		

		Pressed += OnClick;
		

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		Vector2 screenPos = camera.UnprojectPosition(TargetAnchor.GlobalPosition);
		
		Position = screenPos - Size / 2f;
		
		Show();
		
		GD.Print("dwadw");
	}

	public override void OnClick()
	{
		if (globalStateRegistry.stateParents.ContainsKey(target) &&
		    globalStateRegistry.stateParents[target] is IParentTriggerStateChange stateParent)
		{
			stateParent.parentTriggerStateChange();
		}
	}
}
