using Godot;
using System;

public partial class ShadowClassTest : Node3D
{
	public enum classes { shadow, mana, fury, momentum };
	public classes Class = classes.shadow;
	public int shadowNum = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
