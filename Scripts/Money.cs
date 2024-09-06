using Godot;
using System;

public partial class Money : TextEdit
{
	[Export]
	public UI Ui;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Ui.money = Ui.money >= 0 ? Ui.money : 0;
		Ui.money = Ui.money <= 9999 ? Ui.money : 9999;
		Text = Ui.money.ToString();
	}
}
