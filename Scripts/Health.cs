using Godot;
using System;

public partial class Health : ColorRect
{
	[Export]
	public UI Ui;
	[Export]
	public float width, length;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Size = new Vector2(width, length);
		Ui.currentHealth = Ui.maxHealth;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Minus))
		{
			Ui.currentHealth -= 10;
		}
		if (Input.IsKeyPressed(Key.Equal))
		{
			Ui.currentHealth += 10;
		}
		Size = new Vector2(Ui.currentHealth / Ui.maxHealth * width, length) <= new Vector2(width, length) ? new Vector2(Ui.currentHealth / Ui.maxHealth * width, length) : new Vector2(width, length);
		Ui.currentHealth = Ui.currentHealth <= Ui.maxHealth ? Ui.currentHealth : Ui.maxHealth;
	}
}
