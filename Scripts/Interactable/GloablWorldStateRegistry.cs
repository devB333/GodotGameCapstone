namespace IsoMetricCamera.Scripts.Interactable;
using Godot;
using System;
using System.Collections.Generic;

public partial class GloablInteractableRegistry : Node3D
{
	public static GloablInteractableRegistry Instance { get; private set;}
	public  Dictionary<IInteractable, IInteractableState> InteractableRegistry { get; private set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.InteractableRegistry = new Dictionary<IInteractable, IInteractableState>();
		Instance = this;
	}
	
	

	
}
