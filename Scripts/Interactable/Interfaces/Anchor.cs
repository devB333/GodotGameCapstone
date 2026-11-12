using Godot;
using System;

/**
 * This class is used to anchor a texture btn to a Marker3D for interactable surfaces
 */
public abstract partial class Anchor : TextureButton, IInteractableBtn
{
	[Export]
	public Marker3D TargetAnchor;
	
	public GlobalStateRegistry globalStateRegistry { get; set;}
	public Guid target { get; set;}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += OnClick; // subs the OnClick method to listen when pressed
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public virtual void OnClick()
	{
		IStateParent parent = globalStateRegistry.stateParents[target];
	}
}
