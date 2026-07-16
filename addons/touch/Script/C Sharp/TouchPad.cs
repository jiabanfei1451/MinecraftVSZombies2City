using Godot;
using System;
using System.Threading.Tasks;

[GlobalClass]
[Tool]
public partial class TouchPad : Godot.Control
{
	[ExportGroup("TouchPad")]
	[ExportSubgroup("Attribute")]
	[Export] public Godot.Vector2 Scope = Vector2.Zero;
	[Export] public Godot.Vector2 Area_Offect = Vector2.Zero;
	
	[Export] public bool Auto_Settings = true;
	[Export] public float Long_Click_Variant = 1;
	[ExportSubgroup("Variant")]
	[Export] public Godot.Collections.Array<int> Touch_Index;
	[Export] public bool Pressed = false;
	[Export] public bool Touch_Pressed = false;
	public enum on_Click_Type
	{
		not = -1,
		Click = 0,
		Long_Click = 1,
		Drag = 2,
		foucs = 3,
		
	}
	[Export] public on_Click_Type Click_Type = on_Click_Type.not;
	[Export] public double Pressed_Time = 0;
	public enum Cycle_Type
	{
		_PhysicsProcess = 0,
		While = 1
	}
	public int While_Number = 60; 
	[Export] public Cycle_Type Cycle_mode = Cycle_Type._PhysicsProcess;
	// Called when the node enters the scene tree for the first time.
	enum Touch_Type
	{
		Mouse = 0,
		Touch = 1
	}
	public override async void _Ready() {
		base._Ready();
		if (Cycle_mode == Cycle_Type.While)
		{
			while (true){
				await Task.Delay(1000/While_Number);
				Plus_Time(1/While_Number);
			}
		}
	}
	public override async void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (Cycle_mode == Cycle_Type._PhysicsProcess){
		Plus_Time(delta);
		}
	}
	public async void Plus_Time(Double Time)
	{
		if (Pressed == true){
			Pressed_Time += Time;}
		else{
			Pressed_Time = 0;}
		if(Pressed_Time >= Long_Click_Variant && Click_Type == on_Click_Type.Click)
		{
			Click_Type = on_Click_Type.Long_Click;
		}
	}
	public override void _Input(InputEvent @event) {
		base._Input(@event);
		判定(@event);
	}
	
	public void 判定(Godot.InputEvent @event)
	{
		Godot.Vector2 _ViewPort_Scale_ = new Godot.Vector2(GetGlobalTransformWithCanvas()[0].X,GetGlobalTransformWithCanvas()[1].Y);
		
		Godot.Vector2 This_GlobalPosition = GetGlobalTransformWithCanvas()[2];
		Godot.Vector2 ViewPort_Size;
		Godot.Vector2 Event_Postiion;
		
		
		Touch_Type type = Touch_Type.Mouse;

		bool Drag = false;
		if (Auto_Settings == true){
			Event_Postiion = Size * _ViewPort_Scale_;
		}else{
			Event_Postiion = Scope;
		}
		if (@event is InputEventScreenTouch){
			InputEventScreenTouch Touchindex = (InputEventScreenTouch)@event;
			if (Touch_Index.Contains(Touchindex.Index) == false && Touchindex.Pressed == true){
			Event_Postiion = Touchindex.Position;
			Touch_Index.Add(Touchindex.Index);
			Drag = false;
			type = Touch_Type.Touch;
			}
			else if(Touchindex.Pressed == false)
			{
			Touch_Index.Remove(Touchindex.Index);
			}
			Touch_Pressed = Touchindex.Pressed;
		}else if(@event is InputEventScreenDrag){
			InputEventScreenDrag Touchindex = (InputEventScreenDrag)@event;
			Event_Postiion = Touchindex.Position;
			if (Touch_Index.Contains(Touchindex.Index) == false){
				Event_Postiion = Touchindex.Position;
				Touch_Index.Add(Touchindex.Index);
				Drag = true;
				type = Touch_Type.Touch;
			}
		}else if(@event is InputEventMouseButton){
			InputEventMouseButton Touchindex = (InputEventMouseButton)@event;
			Event_Postiion = Touchindex.Position;

			Drag = false;
			type = Touch_Type.Mouse;
			Touch_Pressed = Touchindex.Pressed;

		}else if(@event is InputEventMouseMotion){
			InputEventMouseMotion Touchindex = (InputEventMouseMotion)@event;
			Event_Postiion = Touchindex.Position;
			Drag = true;
			type = Touch_Type.Mouse;
		}
		if (Auto_Settings == true){
			ViewPort_Size = Size * _ViewPort_Scale_;
		}
		else{
			ViewPort_Size = Scope;
		}
		bool 算 = 计算(Event_Postiion,This_GlobalPosition,ViewPort_Size);
		if (算 == true){
			if (type == Touch_Type.Touch){
				if (Touch_Pressed == true && Drag == false){
					Pressed = true;
					Click_Type = on_Click_Type.Click;
				}else if(Drag == true && Pressed == true){
					Click_Type = on_Click_Type.Drag;
				}else if(Drag == true && Touch_Pressed == false && Pressed == false){
					Click_Type = on_Click_Type.foucs;
				}else if(Touch_Pressed == false)
				{
					Pressed = false;
				}
			}
			else
			{
				if (Touch_Pressed == true && Drag == false){
					Pressed = true;
					Click_Type = on_Click_Type.Click;
				}else if(Drag == true && Pressed == true){
					Click_Type = on_Click_Type.Drag;
				}else if(Drag == true && Touch_Pressed == false && Pressed == false){
					Click_Type = on_Click_Type.foucs;
				}else if(Touch_Pressed == false)
				{
					Pressed = false;
				}
			}
		}else{
			Click_Type = on_Click_Type.not;
		}
	}

	public bool 计算(Godot.Vector2 Event_Position,Godot.Vector2 Object_Position,Godot.Vector2 Scope)
	{	
		Godot.Vector2 Calculation = (Object_Position - Event_Position) * -1;
		if (Calculation.X < 0 || Calculation.Y < 0 || Calculation.X > Scope.X || Calculation.Y > Scope.Y)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
}
