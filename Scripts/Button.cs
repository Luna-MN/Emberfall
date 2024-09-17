using Godot;
using System;

public partial class Button : Godot.Button
{
	[Export]
	public PackedScene Scene;
	[Export]
	public Node2D currentScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public override void _Pressed()
	{
		Node2D SI = Scene.Instantiate<Node2D>();
		GetTree().Root.AddChild(SI);
		currentScene.QueueFree();
	}
}
