using Godot;
using System;

public partial class Button : Godot.Button
{
	[Export]
	public Node2D Scene;
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
		Scene.Visible = true;
		currentScene.Visible = false;
	}
}
