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
		Position = middleOfDiscs.LinearInterpolate(middleOfDiscs.ScreenPointToRay(), (float)(middleOfDiscs.BallSpeed * delta), Position);
		if (Position.DistanceTo(middleOfDiscs.ScreenPointToRay()) < 0.1f || BallCollided == true)
		{
			QueueFree();
		}
	}
	private void BallCollide(Node3D body)
	{
		if (body.Name.ToString().Contains("__"))
		{
			BallCollided = true;
		}
	}
	private void BallCollideArea3D(Area3D body)
	{
		if (body.Name.ToString().Contains("__"))
		{
			BallCollided = true;
		}
	}
}
