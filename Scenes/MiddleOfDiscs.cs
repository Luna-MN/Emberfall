using Godot;
using System;

public partial class MiddleOfDiscs : Node3D
{
	[Export]
	public MeshInstance3D[] Balls;
	[Export]
	public CharacterBody3D character;
	[Export]
	public Material[] material;
	private float rotationSpeed = 1.0f; // Speed of rotation
	private float angle = 0.0f; // Current angle of rotation
	private float radius = 1.0f; // Radius of the circle
	public bool pressed = false;
	private Godot.Collections.Dictionary rayA;
	private PhysicsDirectSpaceState3D spaceState;
	private Vector2 mousePos;
	private Camera3D cam;
	private int matNum = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		RotateBalls((float)delta);
		if (!pressed)
		{
			Position = character.Position;
		}
		else
		{
			Position = new Vector3(ScreenPointToRay().X, 0.5f, ScreenPointToRay().Z);
		}


		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			pressed = true;
		}
		if (Input.IsMouseButtonPressed(MouseButton.Right))
		{
			pressed = false;
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
				matNum = (matNum + 1);
				if (matNum == 3)
				{
					matNum = 0;
				}
			}
		}
	}
}