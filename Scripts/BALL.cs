using Godot;
using System;

public partial class BALL : MeshInstance3D
{
	public Callable callable;
	public MiddleOfDiscs middleOfDiscs;
	public bool BallCollided = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		middleOfDiscs = GetTree().Root.GetNode<MainCtest>("Main").middleOfDiscs;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void BallCollide(Node3D body)
	{
		if (body != middleOfDiscs.CollisionShape && body != middleOfDiscs.RicochetArea && body != middleOfDiscs.character)
		{
			BallCollided = true;
		}
		GD.Print("Ball Collided");// area 3d is not treated the same as body, new function to handle collision with middle collider and ricochet area
	}
	private void BallCollideArea3D(Area3D body)
	{
		if (body == middleOfDiscs.FloorArea)
		{
			BallCollided = true;
		}
	}
}
