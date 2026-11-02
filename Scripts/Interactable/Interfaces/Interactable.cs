namespace IsoMetricCamera.Scripts.Interactable;
using Godot;
using System;


public interface IInteractable
{
	public IInteractableState InteractableState { get; set; }
	
	
	public void OnClick();
	
}
