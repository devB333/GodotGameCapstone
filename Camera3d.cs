using Godot;
using System;
using System.Collections.Generic;
using System.Linq;




//----------------------------------------------------------
//This script handles camera occulsion for objs in the cameras way
public partial class Camera3d : Camera3D
{

	[Export] public Node3D Target;
	[Export(PropertyHint.Layers3DPhysics)] public uint WallMask = 2;

	[Export] public float transitionFadeSpeed = 5f;
	[Export] public float collisionFadeEnd = 0.3f;
	private float collisionUnfadeEnd = 1;

	private Stack<StaticBody3D> wallTracker;

	private HashSet<Node> faded;// if object has already been faded
	private HashSet<Node> hits;// if object has been hit that iteration

	private HashSet<fadeObj> transitions; // objects currently in transition

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		wallTracker = new Stack<StaticBody3D>();

		faded = new HashSet<Node>();
		hits = new HashSet<Node>();
		transitions = new HashSet<fadeObj>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		foreach (fadeObj transition in transitions.ToList())
		{
			bool tracker = false; // tracks if the condiiotn is still met

			if(transition.type == "unfade")
				tracker = false;
			else if (transition.type == "fade")
				tracker = true;

			if(hits.Contains(transition.nodeKey) != tracker)
			{
				String oppType = "";
				float oppEnd = 0f;
				if (transition.type == "unfade")
				{
					oppType = "fade";
					oppEnd = collisionFadeEnd;
				}
				else
				{
					oppType = "unfade";
					oppEnd = collisionUnfadeEnd;
				}
				fadeObj oppTransition = new fadeObj(transition.mat.AlbedoColor.A, oppEnd, transition.mat, transition.nodeKey, oppType);

				transitions.Remove(transition);
				transitions.Add(oppTransition);

				continue;
			}

			if (Mathf.Abs(transition.end - transition.start) <= 0.1f)// if at the end of the lerp just snap to end
			{
				Color finalColor = transition.mat.AlbedoColor;
				finalColor.A = transition.end;
				transition.mat.AlbedoColor = finalColor;
				transitions.Remove(transition);
				continue;
			}

			Color fadeAlpha = transition.mat.AlbedoColor;

			fadeAlpha.A = (float) Mathf.Lerp(transition.start, transition.end, transitionFadeSpeed * delta);


			GD.Print($"new alpha: {fadeAlpha.A}");
			transition.mat.AlbedoColor = fadeAlpha;

			transition.start = fadeAlpha.A;
		}
	}

    public override void _PhysicsProcess(double delta)
    {
		hits.Clear();

		var spaceState = GetWorld3D().DirectSpaceState;
		var end = GlobalPosition;
		var origin = GetNode<CharacterBody3D>("../../..").GlobalPosition;

		var rayCastQuery = PhysicsRayQueryParameters3D.Create(origin, end);
		rayCastQuery.CollideWithAreas = true;
		rayCastQuery.Exclude = new Godot.Collections.Array<Rid>();
		rayCastQuery.Exclude.Add(GetNode<CharacterBody3D>("../../..").GetRid());

		var result = spaceState.IntersectRay(rayCastQuery);


		int safety = 0;

		while (result.Count > 0  && safety < 20)
		{
			safety++;
			//GD.Print(safety);
		// there is an obj that has collided, occulde it. 
			CollisionObject3D collision = (CollisionObject3D) result["collider"];
			var exclude = rayCastQuery.Exclude;
			exclude.Add(collision.GetRid());
			rayCastQuery.Exclude = exclude;

			Node hit = detectIfComposite(collision);// we have to check if its part of an occulsion group so we know how to handle the obj

			hits.Add(hit);// adds hit parent to be the hit tracker
			result = spaceState.IntersectRay(rayCastQuery);
		
			//GD.Print($"Hit: {collision.Name}, RID: {collision.GetRid()}");
		}
		//---------------------------------------- start for eaches for occulsion

		foreach (Node fadedNode in faded.ToList())
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
				fadeCollision(hit);
				faded.Add(hit);
			}
		}
		
    }

	private void unFade(Node obj)
	{// this will bring back the wall.
		//GD.Print("Start Unfade");
			List<MeshInstance3D> meshes = findAllMeshes(obj);

			foreach (MeshInstance3D mesh in meshes)
			{
					for (int i = 0; i < mesh.Mesh.GetSurfaceCount(); i++)
				{
					Material mat = mesh.GetSurfaceOverrideMaterial(i);

					if (mat == null)
						mat = mesh.Mesh.SurfaceGetMaterial(i);// now we have that material and can begin to fade

					if (mat is StandardMaterial3D standMat)
					{
						 StandardMaterial3D uniqueMat = (StandardMaterial3D) standMat.Duplicate();

						 mesh.SetSurfaceOverrideMaterial(i, uniqueMat);
						uniqueMat.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;
						float alphaStart = uniqueMat.AlbedoColor.A; // get the alpha of the albedo color

						//GD.Print($"UNFADE {obj.Name}: {alphaStart} -> {collisionUnfadeEnd}");

						fadeObj transitionAdd = new fadeObj(alphaStart, collisionUnfadeEnd, uniqueMat, obj, "unfade");
						transitions.Add(transitionAdd);
					}
				}
			}
	}


	private void fadeCollision(Node obj)
	{
	
			List<MeshInstance3D> meshes = findAllMeshes(obj);

			foreach (MeshInstance3D mesh in meshes)
			{
					for (int i = 0; i < mesh.Mesh.GetSurfaceCount(); i++)
				{
					Material mat = mesh.GetSurfaceOverrideMaterial(i);

					if (mat == null)
						mat = mesh.Mesh.SurfaceGetMaterial(i);// now we have that material and can begin to fade

					if (mat is StandardMaterial3D standMat)
					{
						 StandardMaterial3D uniqueMat = (StandardMaterial3D) standMat.Duplicate();

						 mesh.SetSurfaceOverrideMaterial(i, uniqueMat);
						uniqueMat.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;
						float alphaStart = uniqueMat.AlbedoColor.A; // get the alpha of the albedo color

						//GD.Print($"UNFADE {obj.Name}: {alphaStart} -> {collisionUnfadeEnd}");

						fadeObj transitionAdd = new fadeObj(alphaStart, collisionFadeEnd, uniqueMat, obj, "fade");
						transitions.Add(transitionAdd);
					}
				}
			}
	}

	private Node detectIfComposite(Node current)// checks backwards through the tree to see if its part of a occlusion group
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

	private List<MeshInstance3D> findAllMeshes (Node root)
	{
		Queue<Node> nodeTrace = new Queue<Node>();

		List<MeshInstance3D> meshes = new List<MeshInstance3D>();

		nodeTrace.Enqueue(root);

		while (nodeTrace.Count > 0)
		{
			Node current = nodeTrace.Dequeue();

			if (current is MeshInstance3D)
				meshes.Add((MeshInstance3D) current);

			foreach (Node node in current.GetChildren())
				nodeTrace.Enqueue(node);
		}

		return meshes;
	}

	protected class fadeObj// helper class to keep track of what type of transition needs to be done on what object
	{
		public float start;
		public float end; 

		public StandardMaterial3D mat;

		public Node nodeKey;

		public String type;// type can either be unfade or fade!!!

		public fadeObj(float start, float end, StandardMaterial3D mat, Node nodeKey, String type)
		{
			this.start = start;
			this.end = end;
			this.mat = mat;
			this.nodeKey = nodeKey;
			this.type = type;
		}
	}

}

