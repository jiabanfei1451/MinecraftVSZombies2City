using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Threading.Tasks;

[GlobalClass]
[Tool]
[Icon("uid://sqy1gdavdj6y")]
public partial class TouchPad : Godot.Control
{
	[ExportGroup("TouchPad")]
	[ExportSubgroup("Attribute")]
	[Export] public Godot.Vector2 Scope = Vector2.Zero;
	[Export] public Godot.Vector2 Drag_Velocity_Scope = new Vector2(5,5);
	[Export] public Godot.Vector2 Area_Offect = Vector2.Zero;
	
	[Export] public bool Auto_Settings = true;
	/// <summary>
	/// 启用状态
	/// </summary>
	[Export] public bool Enable = true;
	/// <summary>
	/// 启用长按
	/// </summary>
	[Export] public bool Enable_Long_Click = true;
	/// <summary>
	/// 启用拖拽
	/// </summary>
	[Export] public bool Enable_Drag = true;
	/// <summary>
	/// 启用焦点
	/// </summary>
	[Export] public bool Enable_Focus = true;
	/// <summary>
	/// 长按阈值
	/// </summary>
	[Export] public float Long_Click_Variant = 1;
	public enum _TouchPad_Mode
	{
		Normal = 0
	}
	[Export] public _TouchPad_Mode TouchPad_Mode = _TouchPad_Mode.Normal;  
	[ExportSubgroup("Variant")]
	[Export] public Godot.Collections.Array<int> Touch_Index;
	[Export] public bool Pressed = false;
	[Export] public bool Drag = true;
	[Export] public bool Focus = false;
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
		if (Enable_Long_Click == true){
			if (Pressed == true){
				Pressed_Time += Time;}
			else{
				Pressed_Time = 0;}
			if(Pressed_Time >= Long_Click_Variant && Click_Type == on_Click_Type.Click)
			{
				Click_Type = on_Click_Type.Long_Click;
			}
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

		if (Auto_Settings == true){
			ViewPort_Size = Size * _ViewPort_Scale_;
		}
		else
		{
			ViewPort_Size = Scope;
		}
		Event_Postiion = Get_Touch_Position(@event);
		String _Get_Type_ = Get_Touch_Type(@event);
		String _Get_EventType_ = Get_TouchEvent_Type(@event);

		bool Calculation = Touch_Calculation(Event_Postiion,This_GlobalPosition,ViewPort_Size);
		switch (TouchPad_Mode)
		{
			case _TouchPad_Mode.Normal:
				if (Calculation == true){
				if (_Get_EventType_ == "Button_True"){
					Handle_TouchPad_Data(@event);
					Click_Type = on_Click_Type.Click;
				}else if(_Get_EventType_ == "Button_False"){
					Handle_TouchPad_Data(@event);
				}
				else if (_Get_EventType_ == "Drag"){
					if (Pressed == true){
						if (Get_Touch_Velocity(@event) == true)
						{
							Set_Drag();
						}
					}else{
						Set_Focus(0);
					}
					Handle_TouchPad_Data(@event);
				}
			}
			else if(_Get_EventType_ == "Button_False"){
				Handle_TouchPad_Data(@event);
			}else if(_Get_EventType_ == "Drag"){
				Handle_TouchPad_Data(@event);
				Click_Type = on_Click_Type.not;
				Set_Focus(1);
			}
			break;
		}
	}
	/// <summary>
	/// 设定拖拽状态暂时没有其他模式
	/// </summary>
	/// <param name="Mode"></param>
	public void Set_Drag(int Mode = 0) // 设定拖拽
	{
		switch (Mode){
			case 0:
			Drag = true;
			Click_Type = on_Click_Type.Drag;		
			break;
		}
	}
	/// <summary>
	/// 用于设定焦点数据
	/// </summary>
	/// <param name="mode">选择处理模式0 ~ 1 0:获得焦点 1:失去焦点</param>
	public void Set_Focus(int mode) // 设定焦点
	{
		if (Enable_Focus == true){
		switch (mode){
			case 0:
				Focus = true;
				Click_Type = on_Click_Type.foucs;
				break;
			case <= 1:
				Focus = false;
				break;
		}
		}
	}
	/// <summary>
	/// 判定触发的类型,预计之后支持右键视为长按
	/// </summary>
	/// <param name="event"></param>
	/// <returns></returns>
	public String Get_TouchEvent_Type(Godot.InputEvent @event){ // 获取类型
		if (@event is InputEventScreenTouch){
			InputEventScreenTouch touch = (InputEventScreenTouch)@event;
			if (touch.Pressed == true){
				return "Button_True";
			}else{
				return "Button_False";
			}
		}else if(@event is InputEventMouseButton){
			InputEventMouseButton mouseButton = (InputEventMouseButton)@event;
			if (mouseButton.Pressed == true){
				if (mouseButton.ButtonIndex == MouseButton.Left){
				return "Button_True";
				}else if(mouseButton.ButtonIndex == MouseButton.Right){
					Pressed_Time = Long_Click_Variant;
					return "Button_True";
				}	
			}else{
				return "Button_False";
			}
		}else if (@event is InputEventScreenDrag){
			InputEventScreenDrag drag = (InputEventScreenDrag)@event;
			if (Touch_Index.Contains(drag.Index)){
			return "Drag";
			}
		}else if(@event is InputEventMouseMotion)
		{
			return "Drag";
		}
		return "null";
	}
	
	public bool Get_Touch_Velocity(Godot.InputEvent @event){ // 获取速度
		Godot.Vector2 Velocity = Vector2.Zero;
		if (Enable_Drag == true){
			if (@event is  InputEventScreenDrag){
				InputEventScreenDrag screenDrag = (InputEventScreenDrag)@event;
				Velocity = screenDrag.ScreenVelocity;
			}
			if (@event is InputEventMouseMotion)
			{
				InputEventMouseMotion screenDrag = (InputEventMouseMotion)@event;
				Velocity = screenDrag.ScreenVelocity;
			}
		}
		if (Velocity.X <= -Drag_Velocity_Scope.X || Velocity.X >= Drag_Velocity_Scope.X || Velocity.Y <= -Drag_Velocity_Scope.Y || Velocity.Y >= Drag_Velocity_Scope.Y){
			return true;
		}
		else{
			return false;
		}
	}
	public String Get_Touch_Type(Godot.InputEvent @event,int Mode = 0){ // 获取类型
		if (Mode == 0){
			if (@event is InputEventScreenTouch or InputEventScreenDrag){
				return "Touch";
			}
			if (@event is InputEventMouseButton or InputEventMouseMotion){
				return "Mouse";
			}
		}
		return "null";
	}
	public void Handle_TouchPad_Data(Godot.InputEvent @event) // 处理数据
	{
		if (@event is InputEventScreenTouch){
			InputEventScreenTouch Touch = (InputEventScreenTouch)@event;
			if (!Touch_Index.Contains(Touch.Index) && Touch.Pressed == true){
				Touch_Index.Add(Touch.Index);
				Pressed = true;
			}else if(Touch_Index.Contains(Touch.Index) &&Touch.Pressed == false){
				Touch_Index.Remove(Touch.Index);
				Pressed = false;
				Drag = false;
			}
		}
		if (@event is InputEventMouseButton){
			InputEventMouseButton Touch = (InputEventMouseButton)@event;
			if (Touch.Pressed == true){
				Pressed = true;
			}else if(Touch.Pressed == false){
				Pressed = false;
				Drag = false;
			}
		}
	}
	public Godot.Vector2 Get_Touch_Position(Godot.InputEvent @event) // 处理坐标
	{
		if (@event is InputEventScreenTouch){
			InputEventScreenTouch ScreenTouch = (InputEventScreenTouch)@event;
			return ScreenTouch.Position;
		}if (@event is InputEventScreenDrag){
			InputEventScreenDrag ScreenTouch = (InputEventScreenDrag)@event;
			return ScreenTouch.Position;
		}if (@event is InputEventMouseButton){
			InputEventMouseButton ScreenTouch = (InputEventMouseButton)@event;
			return ScreenTouch.Position;
		}if (@event is InputEventMouseMotion){
			InputEventMouseMotion ScreenTouch = (InputEventMouseMotion)@event;
			return ScreenTouch.Position;
		}

		return Vector2.Zero;
	}
	public bool Touch_Calculation(Godot.Vector2 Event_Position,Godot.Vector2 Object_Position,Godot.Vector2 Scope)
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
