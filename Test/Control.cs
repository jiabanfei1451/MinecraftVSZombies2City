using Godot;
using System;

public partial class Control : TouchPad
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Focus_Join += Sd;
	}
	static public void Sd(TouchPad pad = null,Godot.Vector2 Event_Position = new Vector2())
	{
		GD.Print(1);
		return;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
