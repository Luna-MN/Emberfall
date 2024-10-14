using Godot;
using System;
using System.Collections.Generic;

public partial class Shadow_attacks : Node3D
{
	[Export]
	public CharacterBody3D character;
	private List<Node3D> ShadeList = new List<Node3D>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = character.Position;
	}
	private Node3D closestShade()
	{
		if (ShadeList.Count > 0)
		{
			Vector3 closest = new Vector3(Mathf.Inf, Mathf.Inf, Mathf.Inf);
			Node3D closestNode = null;
			foreach (Node3D shade in ShadeList)
			{
				if (shade.Position.DistanceTo(character.Position) < closest.DistanceTo(character.Position))
				{
					closest = shade.Position;
					closestNode = shade;
				}
			}
			return closestNode;
		}
		return null;
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (Input.IsKeyPressed(Key.Space))
			{
				character.Position = closestShade().Position;
			}
		}
	}
	private void BodyEntered(Node body)
	{
		if (body.Name.ToString().Contains("Shade"))
		{
			ShadeList.Add((Node3D)body);
			GD.Print("Shade entered");
		}
	}
	private void BodyExited(Node body)
	{
		if (body.Name.ToString().Contains("Shade"))
		{
			ShadeList.Remove((Node3D)body);
			GD.Print("Shade exited");
		}
	}
}
