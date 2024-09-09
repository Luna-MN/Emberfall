using Godot;
using System;

public partial class Shadow : Node2D
{
	[Export]
	public ColorRect[] shadowRects;
	[Export]
	public UI Ui;
	private int oldI = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!(Ui.playerClass == UI.classes.Shadow))
		{
			Visible = false;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Minus))
		{
			Ui.currentShadow -= 1;
		}
		if (Input.IsKeyPressed(Key.Equal))
		{
			Ui.currentShadow += 1;
		}
	}
	public void updateShadowRects(int i)
	{
		GD.Print(i);

		if (oldI == i - 1 && i < shadowRects.Length - 1 && i > 0)
		{
			shadowRects[i].Visible = true;
			oldI = i;
		}
		else if (oldI == i + 1 && i < shadowRects.Length - 1 && i > 0)
		{
			shadowRects[i].Visible = false;
			oldI = i;
		}

		else if (oldI < i && i < shadowRects.Length - 1 && i > 0)
		{
			for (int x = oldI - i; x < i; x++)
			{
				shadowRects[x].Visible = true;
			}
			oldI = i;
		}
		else if (oldI > i && i < shadowRects.Length - 1 && i > 0)
		{
			for (int x = i; x < oldI - i; x++)
			{
				shadowRects[x].Visible = false;
			}
			oldI = i;
		}

	}
}
