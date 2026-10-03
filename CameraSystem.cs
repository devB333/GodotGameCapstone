using Godot;
using System;


public partial class CameraSystem : Node3D
{
	private Vector2 camreaRotation = Vector2.Zero;
	[Export]
	public float sensitivity = .002f;
	private Node3D pivot;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pivot = GetNode<Node3D>("../CameraPivot");
		Input.MouseMode = Input.MouseModeEnum.Captured;// makes mouse go away
		camreaRotation.X = pivot.Rotation.Y; // set rotation to inital point to stop sudden jerk when moving camera for the first time
		//GD.Print(camreaRotation.X);
	}

    public override void _Input(InputEvent @event)// check if mouseMotion
    {
       if (@event is InputEventMouseMotion)
		{
			InputEventMouseMotion mouse = (InputEventMouseMotion) @event;
			Vector2 movement = mouse.ScreenRelative; // vector 2 of how the mouse has moved. Tells you how much the mouse has moved since the last frame
			//GD.Print(movement.X);// we only care about the x movement
			cameraMove(movement);
		}
    }

	//what were doing here is scaling movmenet by sensitivy and then adding it to the cameraRotation, cameraRotation is how the pivot is rotated 
	// that is constantly being tracked for consistency, and to prevent floating point errors from transform we genrate a new quaternion every time by moving 1 axis and feeding it the rotation
	// then we make a new basis and apply that to the pivots basis. 
	private void cameraMove(Vector2 movement)
	{
		camreaRotation -= movement * sensitivity;
		//pivot.Basis = new Basis();


		Vector3 yAxis = new Godot.Vector3(0,1,0);
		Quaternion rotate = new Quaternion(yAxis, camreaRotation.X);

		pivot.Basis = new Basis(rotate);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
