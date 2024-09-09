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

	// fury class stuff
	[Export]
	public float maxFury, currentFury;

	// momentum class stuff
	[Export]
	public float maxMomentum, currentMomentum;
	// shadow class stuff
	[Export]
	public Shadow shadow;
	private int currentShadowLocal = 0;
	[Export]
	public int currentShadow
	{
		get
		{
			return currentShadowLocal;
		}
		set
		{
			currentShadowLocal += value;
			GD.Print(currentShadowLocal);
			if (currentShadowLocal < 0)
			{
				currentShadowLocal = 0;
			}
			if (currentShadowLocal > shadow.shadowRects.Length - 1)
			{
				currentShadowLocal = shadow.shadowRects.Length - 1;
			}
			shadow.updateShadowRects(currentShadowLocal);
		}
	}

}
