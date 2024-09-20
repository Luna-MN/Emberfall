using Godot;
using System;

public partial class MiddleOfDiscs : Node3D
{
	// balls
	[Export]
	public MeshInstance3D[] Balls;

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
	public bool pressed = false;
	private Vector3 targetPosition;
	private float moveSpeed = 100.0f;

	// ray tracing
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
		// Rotate the balls
		RotateBalls((float)delta);
		// move balls
		if (!pressed)
		{
			targetPosition = character.Position;
		}
		else
		{
			targetPosition = new Vector3(ScreenPointToRay().X, 0.5f, ScreenPointToRay().Z);
		}
		MoveBallsTowardsTarget((float)delta);
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
		LinearInterpolate(targetPosition, moveSpeed * delta);
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
			if (Input.IsKeyPressed(Key.F))
			{
				foreach (var Ball in Balls)
				{
					Ball.Mesh.SurfaceSetMaterial(0, material[matNum]);
				}
				matNum = (matNum + 1) <= 2 ? matNum + 1 : 0;
			}
		}
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			if (Input.IsMouseButtonPressed(MouseButton.Left))
			{
				pressed = !pressed;
				moveSpeed = pressed ? 2.0f : 100.0f;
			}
			if (Input.IsMouseButtonPressed(MouseButton.Right) && pressed == false)
			{
				targetPosition = Position + ((ScreenPointToRay() - Position).Normalized()) * 6.0f;
			}
		}
	}
	public void LinearInterpolate(Vector3 b, float t)
	{
		Position = Position.Lerp(b, t);
	}
}