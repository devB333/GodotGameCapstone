using Godot;
using System;
using System.Collections.Generic;




public partial class Camera3d : Camera3D
{

	[Export] public Node3D Target;
	[Export(PropertyHint.Layers3DPhysics)] public uint WallMask = 2;

	private Stack<StaticBody3D> wallTracker;

	private HashSet<Node> faded;// if object has already been faded
	private HashSet<Node> hits;// if object has been hit that iteration

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		wallTracker = new Stack<StaticBody3D>();

		faded = new HashSet<Node>();
		hits = new HashSet<Node>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
		var spaceState = GetWorld3D().DirectSpaceState;
		var end = GetNode<Camera3D>("Camera3D").GlobalPosition;
		var origin = GetNode<CharacterBody3D>("CharBody").GlobalPosition;

		var rayCastQuery = PhysicsRayQueryParameters3D.Create(origin, end);
		rayCastQuery.CollideWithAreas = true;
		rayCastQuery.Exclude = new Godot.Collections.Array<Rid>();
		rayCastQuery.Exclude.Add(GetNode<CharacterBody3D>("CharBody").GetRid());

		var result = spaceState.IntersectRay(rayCastQuery);

		while (result.Count > 0)
		{
		// there is an obj that has collided, occulde it. 
			CollisionObject3D collision = (CollisionObject3D) result["collider"];
			rayCastQuery.Exclude.Add(collision.GetRid());
			Node hit = detectIfComposite(collision);

			hits.Add(hit);// adds hit parent to be the hit tracker
			result = spaceState.IntersectRay(rayCastQuery);
		
		}
		//---------------------------------------- start for eaches for occulsion

		foreach (Node fadedNode in faded)
		{
			if(!hits.Contains(fadedNode))// the object hit is not faded so it must be faded
			{
				unFade(fadedNode);
				faded.Remove(fadedNode);// remove the occulsion
			}
			// do nothing if its contained in hits because it should still be occulded
		}

		foreach (Node hit in hits)
		{
			if(!faded.Contains(hit))// add it to the fadedKV
			{
				fadeObj(hit);
				faded.Add(hit);
			}
		}
		
    }

	private void unFade(Node obj)
	{// this will bring back the wall.
		if (obj is OcclusionGroup)
		{
			
		}
		else
		{
			MeshInstance3D mesh = (MeshInstance3D) obj.GetNode("MeshInstance3D");
			if(mesh.MaterialOverride)
			{
				
			}
		}
	}


	private void fadeObj(Node obj)
	{
		
	}

	private Node detectIfComposite(Node current)
	{
		Node orignalCollider = current;

		current = current.GetParent();

		while(current != null)
			{
				if (current is OcclusionGroup)// its a group so return that
					return current;
				else
					current = current.GetParent();
			}

		return orignalCollider;// there was no OcclusionGroup found so the collisonObject must be a body, so return that
		
	}

}

