using Godot;
using System;

public partial class Momentum : Node2D
{
	[Export]
	public UI Ui;
	[Export]
	public ColorRect momentumBar;
	[Export]
	public float momentumWidth, momentumLength;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!(Ui.playerClass == UI.classes.Momentum))
		{
			Visible = false;
		}
		momentumBar.Size = new Vector2(momentumWidth, momentumLength);
		Ui.currentMomentum = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Minus))
		{
			Ui.currentMomentum -= 10;
		}
		if (Input.IsKeyPressed(Key.Equal))
		{
			Ui.currentMomentum += 10;
		}
		momentumBar.Size = new Vector2(Ui.currentMomentum / Ui.maxMomentum * momentumWidth, momentumLength) <= new Vector2(momentumWidth, momentumLength) ? new Vector2(Ui.currentMomentum / Ui.maxMomentum * momentumWidth, momentumLength) : new Vector2(momentumWidth, momentumLength);
		Ui.currentMomentum = Ui.currentMomentum <= Ui.maxMomentum ? Ui.currentMomentum : Ui.maxMomentum;
	}
}
