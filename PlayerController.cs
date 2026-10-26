using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	[Export]
	public float rotationSpeed = 8.0f;

	public Vector3 lastDirection = Vector3.Back;	

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Node3D cameraPivot = GetNode<Node3D>("CameraPivot");
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 direction = (cameraPivot.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		
		//Updates LastDirection var if player is in motion
		lastDirection = direction != Vector3.Zero? direction :lastDirection;

		//Temp playerModel and animationPlayer vars. Need to swap out when we get the actual player model!
		Node3D playerModel = GetNode<Node3D>("playerTesterModel");
		var animationPlayer = playerModel.GetNode<AnimationPlayer>("AnimationPlayer");
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}
		
		if(velocity.X!=0 || velocity.Z!=0)
		{
			animationPlayer.Play("PlayerAnims/walk");
			animationPlayer.SpeedScale = 1.6f;
		}
		else
		{
			animationPlayer.Play("PlayerAnims/idle");
			animationPlayer.SpeedScale = 1f;
		}
		Velocity = velocity;

		//Rotates player in direction they're moving in.
		var angle = Vector3.Back.SignedAngleTo(lastDirection, Vector3.Up);

		float weight = Mathf.Clamp(rotationSpeed * (float)delta, 0f, 1f);// could experiment with  weight function or have a skip frame function to render at a lower fps than game

		playerModel.Rotation = new Vector3(
			0,
			Mathf.LerpAngle(playerModel.Rotation.Y, angle, weight),
			0
		);

		MoveAndSlide();
	}
}
