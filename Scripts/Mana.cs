using Godot;
using System;

public partial class Mana : Node2D
{
	[Export]
	public UI Ui;
	[Export]

	public ColorRect manaBar;
	[Export]
	public float manaWidth, manaLength;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!(Ui.playerClass == UI.classes.Mana))
		{
			Visible = false;

		}
		manaBar.Size = new Vector2(manaWidth, manaLength);
		Ui.currentMana = Ui.maxMana;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Minus))
		{
			Ui.currentMana -= 10;
		}
		if (Input.IsKeyPressed(Key.Equal))
		{
			Ui.currentMana += 10;
		}
		manaBar.Size = new Vector2(Ui.currentMana / Ui.maxMana * manaWidth, manaLength) <= new Vector2(manaWidth, manaLength) ? new Vector2(Ui.currentMana / Ui.maxMana * manaWidth, manaLength) : new Vector2(manaWidth, manaLength);
		Ui.currentMana = Ui.currentMana <= Ui.maxMana ? Ui.currentMana : Ui.maxMana;
	}
}
