using Godot;
using System;
using System.Collections.Generic;

public partial class Shadow_attacks : Node3D
{
	[Export]
	public CharacterBody3D character;
	private List<Node3D> ShadeList = new List<Node3D>();
	Timer timer;
	public bool attached = false, dashed = false;
	private Godot.Collections.Dictionary rayA;
	private PhysicsDirectSpaceState3D spaceState;
	private Vector2 mousePos;
	private Camera3D cam;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = character.Position;
		if (timer != null && timer.IsStopped())
		{
			((Movement)character).inhibitMovement = false;
			attached = false;
			dashed = false;
		}
	}
	private Node3D closestShade()
	{
		if (ShadeList.Count > 0)
		{
			Vector3 closest = new Vector3(Mathf.Inf, Mathf.Inf, Mathf.Inf);
			Node3D closestNode = null;
			foreach (Node3D shade in ShadeList)
			{
				if (shade.Position.DistanceTo(character.Position) < closest.DistanceTo(character.Position))
				{
					GD.Print(shade.Position);
					closest = shade.Position;
					closestNode = shade;
				}
			}
			return closestNode;
		}
		return null;
	}
	public void inhibitMovement()
	{
		timer = new Timer { OneShot = true, WaitTime = 3.0f, Autostart = true };
		AddChild(timer);
		timer.Start();
		((Movement)character).inhibitMovement = true;
	}
	public void DashForward()
	{
		if (attached && !dashed)
		{
			character.Position += (ScreenPointToRay() - character.Position).Normalized() * 10;
			character.Position = new Vector3(character.Position.X, 0, character.Position.Z);
			dashed = true;
		}
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (Input.IsKeyPressed(Key.Space))
			{
				character.Position = closestShade().Position;
				attached = true;
				inhibitMovement();
			}
		}
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Right)
			{
				DashForward();
			}
		}
	}
	private void BodyEntered(Node body)
	{
		if (body.Name.ToString().Contains("Shade"))
		{
			ShadeList.Add((Node3D)body);
			GD.Print("Shade entered");
		}
	}
	private void BodyExited(Node body)
	{
		if (body.Name.ToString().Contains("Shade"))
		{
			ShadeList.Remove((Node3D)body);
			GD.Print("Shade exited");
		}
	}
	// ray tracing
	public Vector3 ScreenPointToRay()
	{
		// Get the mouse position
		spaceState = GetWorld3D().DirectSpaceState;
		mousePos = GetViewport().GetMousePosition();
		// Get the camera
		cam = GetTree().Root.GetCamera3D();
		// Get the ray
		var rayO = cam.ProjectRayOrigin(mousePos);
		var rayE = rayO + cam.ProjectRayNormal(mousePos) * 2000;
		var query = PhysicsRayQueryParameters3D.Create(rayO, rayE);
		query.CollideWithAreas = true;
		query.CollisionMask = 1 << 2;
		// Get the raycast
		rayA = spaceState.IntersectRay(query);
		// Return the position of the raycast
		if (rayA != null)
			return (Vector3)rayA["position"];
		return new Vector3(0, 0, 0);
	}
}
