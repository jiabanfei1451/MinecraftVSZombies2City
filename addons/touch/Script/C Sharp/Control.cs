using Godot;
using System;

[GlobalClass]
[Tool]
public partial class Control : Godot.Control
{
	[ExportGroup("TouchPad")]
	[ExportSubgroup("Variant")]
	
	[Export] public bool Pressed = false;
	public enum on_Click_Type
	{
		not = -1,
		Click = 0,
		Long_Click = 1
	}
	[Export] public on_Click_Type Click_Type = on_Click_Type.not;
	[Export] public float Pressed_Time = 0;
	public enum Cycle_Type
	{
		_PhysicsProcess = 0,
		While = 1
	}
	[Export] public Cycle_Type Cycle_mode = Cycle_Type._PhysicsProcess;
	// Called when the node enters the scene tree for the first time.
}
