using Godot;
using System;


public partial class CameraSystem : Node3D
{
	private float _camreaRotation = 0f;
	[Export]
	public float Sensitivity = .01f;
	private Node3D _pivot;

	private Camera3D _camera;
	
	[Export]
	public float ScrollSensitivity = 50f;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_pivot = GetNode<Node3D>("../CameraPivot");
		_camera = GetNode<Camera3D>("../CameraPivot/SpringArm3D/Camera3D");
		Input.MouseMode = Input.MouseModeEnum.Captured;// makes mouse go away
		_camreaRotation = _pivot.Rotation.Y; // set rotation to inital point to stop sudden jerk when moving camera for the first time
		//GD.Print(camreaRotation.X);
	}

	/*
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
    */

	//what were doing here is scaling movmenet by sensitivy and then adding it to the cameraRotation, cameraRotation is how the pivot is rotated 
	// that is constantly being tracked for consistency, and to prevent floating point errors from transform we genrate a new quaternion every time by moving 1 axis and feeding it the rotation
	// then we make a new basis and apply that to the pivots basis. 
	private void CameraMove(float dir)
	{
		
		_camreaRotation -= dir * Sensitivity;
		//pivot.Basis = new Basis();


		Vector3 yAxis = new Godot.Vector3(0,1,0);
		Quaternion rotate = new Quaternion(yAxis, _camreaRotation);

		_pivot.Basis = new Basis(rotate);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float dir = Input.GetAxis("CameraRotateRight", "CameraRotateLeft");
		CameraMove(dir);
		

	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse && mouse.Pressed)
		{
			if (mouse.ButtonIndex == MouseButton.WheelUp)
			{
				GD.Print(_camera.Fov);
				_camera.Fov -= 0.01f * ScrollSensitivity; 
				
			}

			if (mouse.ButtonIndex == MouseButton.WheelDown)
			{
				_camera.Fov += 0.01f  * ScrollSensitivity;
			}
		}
	}
}
