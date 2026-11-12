using Godot;
using System;

public interface IInteractableBtn
{
	public Guid target { get; set;}
	public GlobalStateRegistry globalStateRegistry { get; set;}
	// Called when the node enters the scene tree for the first time.
	
	/**
	 * This will trigger the click on the interactable obj
	 */
	protected abstract void OnClick();
	
}
