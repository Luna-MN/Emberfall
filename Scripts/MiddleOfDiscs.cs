using Godot;
using System;
using System.Collections.Generic;

public partial class MiddleOfDiscs : Node3D
{
	// balls
	[Export]
	public MeshInstance3D[] Balls;
	[Export]
	public PackedScene BallScene;
	public BALL[] CreatedBalls = new BALL[3];
	public Callable callable;
	public float BallSpeed = 5.0f;
	[Export]
	public Area3D FloorArea3D, RicochetArea3D, CollisionArea3D;

	// character
	[Export]
	public CharacterBody3D character;

	//material
	[Export]
	public Material[] material;
	private int matNum = 1;

	// ball handling
	private float rotationSpeed = 1.0f;
	private float angle = 0.0f;
	private float radius = 1.0f;
	public bool pressed = false, elementThrow = false;
	private Vector3 targetPosition;
	private float moveSpeed = 10.0f;
	[Export]
	public CollisionShape3D CollisionShape;

	// ray tracing
	private Godot.Collections.Dictionary rayA;
	private PhysicsDirectSpaceState3D spaceState;
	private Vector2 mousePos;
	private Camera3D cam;
	// ricochet
	private List<Node3D> ricochet = new List<Node3D>();
	private Node3D closestNode;
	[Export]
	public int maxRicochet = 3;
	[Export]
	public CollisionShape3D RicochetArea;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		callable = new Callable(this, "BallCollide");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Rotate the balls
		RotateBalls((float)delta);
		// move balls
		if (!pressed && elementThrow == false)
		{
			targetPosition = character.Position;
		}
		else if (pressed)
		{
			targetPosition = new Vector3(ScreenPointToRay().X, 0.5f, ScreenPointToRay().Z);
		}
		MoveBallsTowardsTarget((float)delta);
		for (int i = 0; i < 3; i++)
		{
			if (CreatedBalls[i] != null)
			{
				CreatedBalls[i].Position = LinearInterpolate(ScreenPointToRay(), (float)(BallSpeed * delta), CreatedBalls[i].Position);
				if (CreatedBalls[i].Position.DistanceTo(ScreenPointToRay()) < 0.1f || CreatedBalls[i].BallCollided == true)
				{
					CreatedBalls[i].QueueFree();
					GD.Print("Ball Deleted");
					CreatedBalls[i] = null;
				}
			}
		}
	}

	private void RotateBalls(float delta)
	{
		// Update the rotation angle
		angle += rotationSpeed * delta;

		// Rotate Ball1
		Balls[0].Position = new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));

		// Rotate Ball2
		Balls[1].Position = new Vector3(radius * Mathf.Cos(angle + Mathf.Pi * 2 / 3), 0, radius * Mathf.Sin(angle + Mathf.Pi * 2 / 3));

		// Rotate Ball3
		Balls[2].Position = new Vector3(radius * Mathf.Cos(angle + 4 * Mathf.Pi / 3), 0, radius * Mathf.Sin(angle + 4 * Mathf.Pi / 3));
	}

	private void MoveBallsTowardsTarget(float delta)
	{
		// Move Ball1 towards the target position
		Position = LinearInterpolate(targetPosition, moveSpeed * delta, Position);
		// element throw check
		if (Position.DistanceTo(targetPosition) < 0.1f)
		{
			elementThrow = false;
			moveSpeed = 100.0f;
			maxRicochet = 3;
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

	// this is just so it only detects on press
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			// element change
			if (Input.IsKeyPressed(Key.F))
			{
				foreach (var Ball in Balls)
				{
					Ball.Mesh.SurfaceSetMaterial(0, material[matNum]);
				}
				matNum = (matNum + 1) <= 2 ? matNum + 1 : 0;
			}
			if (Input.IsKeyPressed(Key.E) && keyEvent.Pressed)
			{
				int i = 0;
				foreach (var Ball in Balls)
				{
					CreatedBalls[i] = BallScene.Instantiate<BALL>();// to do, spread balls
					GetParent().AddChild(CreatedBalls[i]);
					CreatedBalls[i].Mesh.SurfaceSetMaterial(0, material[matNum]);
					CreatedBalls[i].Position = Position;
					i++;
				}
			}
		}
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			// disc follow mouse
			if (Input.IsMouseButtonPressed(MouseButton.Left))
			{
				pressed = !pressed;
				moveSpeed = pressed ? 2.0f : 100.0f;
			}
			// disc throw
			if (Input.IsMouseButtonPressed(MouseButton.Right) && pressed == false)
			{
				targetPosition = Position + (ScreenPointToRay() - Position).Normalized() * 6.0f;
				elementThrow = true;
				// element throw speed
				moveSpeed = 5.0f;
			}
		}
	}
	public Vector3 LinearInterpolate(Vector3 b, float t, Vector3 p)
	{
		p = p.Lerp(b, t);
		return p;
	}
	// collision	
	private void BodyEntered(Node3D body)
	{
		GD.Print("Body Entered");
		Vector3 closest = new Vector3(Mathf.Inf, Mathf.Inf, Mathf.Inf);
		if (maxRicochet > 0 && elementThrow == true)
		{
			foreach (Node3D node in ricochet)
			{
				if (node.Position.DistanceTo(Position) < closest.DistanceTo(Position) && node != body)
				{
					closest = node.Position;
					closestNode = node;
				}
			}
			GD.Print(closestNode);
			targetPosition = closestNode.Position;
			maxRicochet--;
		}
	}
	// ricochet
	private void RicEnter(Node3D body)
	{
		GD.Print("Ric Entered");
		ricochet.Add(body);
	}
	private void RicExit(Node3D body)
	{
		GD.Print("Ric Exited");
		ricochet.Remove(body);
	}
}