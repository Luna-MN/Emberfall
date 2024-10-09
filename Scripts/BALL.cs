using Godot;
using System;

public partial class BALL : Area3D
{
	public Callable callable;
	public MiddleOfDiscs middleOfDiscs;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		middleOfDiscs = GetNode<MiddleOfDiscs>("/root/Node3D/MiddleOfDiscs");
		Connect("body_entered", middleOfDiscs.callable);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
