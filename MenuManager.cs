using Godot;
using System;

public partial class MenuManager : Node
{
	public static MenuManager Instance {get; private set;}
	public bool IsMenuOpen {get; set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		IsMenuOpen = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if(Input.IsActionJustPressed("menu")){
			CheckMenuState();
		}
	}

	public void CheckMenuState()
	{
		IsMenuOpen = !IsMenuOpen;
		if (IsMenuOpen)//Is the menu already open?
			{
				GetNode<Control>("../Menu").Visible = true;
				Input.MouseMode = Input.MouseModeEnum.Visible;
				GetTree().Paused = true;
			}
			else//Is the menu not open?
			{
				GetNode<Control>("../Menu").Visible = false;
				Input.MouseMode = Input.MouseModeEnum.Captured;
				GetTree().Paused = false;
			}	
	}

}
