using Godot;
using System;

public partial class Fury : Node2D
{
	[Export]
	public UI Ui;
	[Export]
	public ColorRect furyBar;
	[Export]
	public float furyWidth, furyLength;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!(Ui.playerClass == UI.classes.Mana))
		{
			Visible = false;

		}
		furyBar.Size = new Vector2(furyWidth, furyLength);
		Ui.currentFury = Ui.maxFury;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Minus))
		{
			Ui.currentFury -= 10;
		}
		if (Input.IsKeyPressed(Key.Equal))
		{
			Ui.currentFury += 10;
		}
		furyBar.Size = new Vector2(Ui.currentFury / Ui.maxFury * furyWidth, furyLength) <= new Vector2(furyWidth, furyLength) ? new Vector2(Ui.currentFury / Ui.maxFury * furyWidth, furyLength) : new Vector2(furyWidth, furyLength);
		Ui.currentFury = Ui.currentFury <= Ui.maxFury ? Ui.currentFury : Ui.maxFury;
	}
}
