using Godot;
using System;

public partial class Movement : CharacterBody3D
{
	[Export]
	public float speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector3 newVelocity = Vector3.Zero;

		if (Input.IsKeyPressed(Key.W))
		{
			newVelocity -= Vector3.Back * speed;
		}
		if (Input.IsKeyPressed(Key.S))
		{
			newVelocity -= Vector3.Forward * speed;
		}
		if (Input.IsKeyPressed(Key.A))
		{
			newVelocity -= Vector3.Right * speed;
		}
		if (Input.IsKeyPressed(Key.D))
		{
			newVelocity -= Vector3.Left * speed;
		}


		Velocity = newVelocity;
		MoveAndSlide();
	}
}
