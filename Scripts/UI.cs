using Godot;
using System;

public partial class UI : Node2D
{
	[Export]
	public float maxHealth, currentHealth;
	[Export]
	public int money;
	public enum classes { Shadow, Fury, Mana, Momentum };
	[Export]
	public classes playerClass;
	//mana class stuff
	[Export]
	public float maxMana, currentMana;


}
