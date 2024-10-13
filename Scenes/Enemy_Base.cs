using Godot;
using System;

public partial class Enemy_Base : RigidBody3D
{
	[Export]
	public PackedScene Shade;
	[Export]
	public ShadowClassTest main;
	public float health = 100.0f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		deathCheck();
	}
	public void deathCheck()
	{
		if (health <= 0)
		{
			if (main.Class == ShadowClassTest.classes.shadow)
			{
				Shade.Instantiate();
			}
			QueueFree();
		}
	}
}
