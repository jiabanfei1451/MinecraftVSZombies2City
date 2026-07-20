using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Threading.Tasks;

[GlobalClass()]
[Icon("uid://sqy1gdavdj6y")]
public partial class TouchPad : Godot.Control
{
	[Signal]
	public delegate void Button_DownEventHandler(TouchPad pad,Godot.Vector2 Event_Position);
	[Signal]
	public delegate void Button_UPEventHandler(TouchPad pad,Godot.Vector2 Event_Position);
	[Signal]
	public delegate void Button_PressedEventHandler(TouchPad pad,Godot.Vector2 Event_Position);
	[Signal]
	public delegate void Button_Long_PressedEventHandler(TouchPad pad,Godot.Vector2 Event_Position);

	[Signal]
	public delegate void Start_DragEventHandler(TouchPad pad,Godot.Vector2 Event_Position,Godot.Vector2 Velocity);
	[Signal]
	public delegate void Drag_IngEventHandler(TouchPad pad,Godot.Vector2 Event_Position,Godot.Vector2 Velocity);
	[Signal]
	public delegate void End_DragEventHandler(TouchPad pad,Godot.Vector2 Event_Position);
	[Signal]
	public delegate void Focus_JoinEventHandler(TouchPad pad,Godot.Vector2 Event_Position);
	[Signal]
	public delegate void Focus_ExitEventHandler(TouchPad pad,Godot.Vector2 Event_Position);

	[Signal]
	public delegate void Button_DownvoidEventHandler();
	[Signal]
	public delegate void Button_UPvoidEventHandler();
	[Signal]
	public delegate void Button_PressedvoidEventHandler();
	[Signal]
	public delegate void Button_Long_PressedvoidEventHandler(TouchPad pad,Godot.Vector2 Event_Position);

	[Signal]
	public delegate void Start_DragvoidEventHandler();
	[Signal]
	public delegate void Drag_IngvoidEventHandler();
	[Signal]
	public delegate void End_DragvoidEventHandler();
	[Signal]
	public delegate void Focus_JoinvoidEventHandler();
	[Signal]
	public delegate void Focus_ExitvoidEventHandler();



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
	[Export] public int While_Number = 60; 
	[Export] public Cycle_Type Cycle_mode = Cycle_Type._PhysicsProcess;
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready() {
		base._Ready();
		if (Cycle_mode == Cycle_Type.While)
		{
			while (true){
				Plus_Time(1d / While_Number);
				await Task.Delay(1000 / While_Number);
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
	
	public async void 判定(Godot.InputEvent @event)
	{
		if (Enable == true){
		Godot.Vector2 Viewport_Position = GetGlobalTransformWithCanvas()[2];
		Godot.Vector2 ViewPort_Scale = new Vector2(GetGlobalTransformWithCanvas()[0].X,GetGlobalTransformWithCanvas()[1].Y);
		Godot.Vector2 ViewPort_Size = ViewPort_Scale * Size;
		Godot.Vector2 Input_Position = Vector2.Zero;

		if (!Auto_Settings){
			ViewPort_Size = Scope;
		}

		Vec2 Temp_Vec2;
		Temp_Vec2 = Get_Touch_Velocity(@event);
		bool OK;
		if (Temp_Vec2.Not_Position){return;}
		OK = Touch_Calculation(Temp_Vec2.Position,Viewport_Position,ViewPort_Size);
		if (TouchPad_Mode == _TouchPad_Mode.Normal){
			if (Temp_Vec2.Input_Type == Vec2.Button_Type.Touch){
				if (Temp_Vec2.Event_Type == Vec2.Button_Event_Type.Button){
					if (Temp_Vec2.Pressed == true){
						if (!OK){return;}
						Set_Touch_Index(0,Temp_Vec2.Index);
						Pressed = true;
						Click_Type = on_Click_Type.Click;
						EmitSignal("Button_UPvoid");
						EmitSignal("Button_UP",this,Temp_Vec2.Position);
					}
					else{
						if (Drag)
							{
								EmitSignal("End_Drag",this,Temp_Vec2.Position);
								EmitSignal("End_Dragvoid");
							}
						EmitSignal("Button_Downvoid");
						EmitSignal("Button_Down",this,Temp_Vec2.Position);
						if (Click_Type == on_Click_Type.Click)
							{
								EmitSignal("Button_Pressedvoid");
								EmitSignal("Button_Pressed",this,Temp_Vec2.Position);
							}else if(Click_Type == on_Click_Type.Long_Click)
							{
								EmitSignal("Button_Long_Pressedvoid");
								EmitSignal("Button_Long_Pressed",this,Temp_Vec2.Position);
							}
						Pressed = false;
						Drag = false;
						Focus = false;
						Click_Type = on_Click_Type.not;
						Set_Touch_Index(1,Temp_Vec2.Index);
					}
				}else if(Temp_Vec2.Event_Type == Vec2.Button_Event_Type.Drag){
					if (Pressed == true && Get_Touch_Index(Temp_Vec2.Index) != -1){
						Click_Type = on_Click_Type.Drag;
						if (Drag == false && Temp_Vec2.Enable_Drag)
							{
								EmitSignal("Start_Dragvoid",this,Temp_Vec2.Position,Temp_Vec2.Velocity);
								EmitSignal("Start_Drag");
							}
							else
							{
								EmitSignal("Drag_Ing",this,Temp_Vec2.Position,Temp_Vec2.Velocity);
								EmitSignal("Drag_Ingvoid");
							}
						Drag = true;
					}
					if (OK)
					{
						if (Click_Type == on_Click_Type.not){Click_Type = on_Click_Type.foucs;}
						if (!Focus){
						EmitSignal("Focus_Join",this,Temp_Vec2.Position);
						EmitSignal("Focus_Joinvoid");
						}
						Focus = true;
						}
					else
					{	
						if (Click_Type == on_Click_Type.foucs){Click_Type = on_Click_Type.not;}
						if (Focus){
						EmitSignal("Focus_Exit",this,Temp_Vec2.Position);
						EmitSignal("Focus_Exitvoid");
					}
						Focus = false;
					}
				}
			}
			else if(Temp_Vec2.Input_Type == Vec2.Button_Type.Mouse)
			{
				if (Temp_Vec2.Event_Type == Vec2.Button_Event_Type.Button)
				{
					if (Temp_Vec2.Pressed == true)
					{
						if (!OK){return;}
						Pressed = true;
						Click_Type = on_Click_Type.Click;
					}
					else
					{
						if (Drag)
							{
								EmitSignal("End_Drag",this,Temp_Vec2.Position);
								EmitSignal("End_Dragvoid");
							}
						Pressed = false;
						Drag = false;
						Click_Type = on_Click_Type.not;
					}
				}
				else if(Temp_Vec2.Event_Type == Vec2.Button_Event_Type.Drag)
				{
					if (Pressed == true && Temp_Vec2.Enable_Drag)
					{
						Click_Type = on_Click_Type.Drag;
						if (Drag == false)
							{
								EmitSignal("Start_Dragvoid",this,Temp_Vec2.Position,Temp_Vec2.Velocity);
								EmitSignal("Start_Drag");
							}
							else
							{
								EmitSignal("Drag_Ing",this,Temp_Vec2.Position,Temp_Vec2.Velocity);
								EmitSignal("Drag_Ingvoid");
							}
						Drag = true;
					}
					if (OK)
					{
						if (Click_Type == on_Click_Type.not){Click_Type = on_Click_Type.foucs;
						}
						if (!Focus){
						EmitSignal("Focus_Join",this,Temp_Vec2.Position);
						EmitSignal("Focus_Joinvoid");
						}
						Focus = true;
						}
					else
					{
						if (Click_Type == on_Click_Type.foucs){Click_Type = on_Click_Type.not;}
						if (Focus){
						EmitSignal("Focus_Exit",this,Temp_Vec2.Position);
						EmitSignal("Focus_Exitvoid");
					}
						Focus = false;
					}
				}
			}
		}
		}
	}
	/// <summary>
	/// 如果 <seealso cref="int"/> Index 存在时返回当前的Index 否则返回 -1
	/// </summary>
	/// <param name="Index"></param>
	/// <returns></returns>
	public int Get_Touch_Index(int Index)
	{
		if (!Touch_Index.Contains(Index)){return -1;}
		return Index;
	}
	/// <summary>
	/// 用于添加删除触控索引 0是添加 1是删除
	/// </summary>
	/// <param name="Mode">0添加 1删除</param>
	public void Set_Touch_Index(int Mode = 0,int Index = -99){
		if (Index == -99){return;}
		switch (Mode)
		{
			case 0:
				if (!Touch_Index.Contains(Index))
				{
					Touch_Index.Add(Index);
				}
				break;
			case 1:
				if (Touch_Index.Contains(Index))
				{
					Touch_Index.Remove(Index);
				}
				break;
		}
	}
	public Vec2 Get_Touch_Velocity(Godot.InputEvent @event)
	{
		Vec2 vec = new Vec2(); 
		if (@event is InputEventScreenDrag){
			InputEventScreenDrag drag = (InputEventScreenDrag)@event;
			vec.Position = drag.Position;
			vec.Velocity = drag.Velocity;
			vec.Not_Position = false;
			vec.Not_Velocity = false;
			vec.Index = drag.Index;
			vec.Input_Type = Vec2.Button_Type.Touch;
			vec.Event_Type = Vec2.Button_Event_Type.Drag;
			return vec;
		}
		if(@event is InputEventMouseMotion){
			InputEventMouseMotion drag = (InputEventMouseMotion)@event;
			vec.Position = drag.Position;
			vec.Velocity = drag.Velocity;
			vec.Not_Position = false;
			vec.Not_Velocity = false;
			vec.Input_Type = Vec2.Button_Type.Mouse;
			vec.Event_Type = Vec2.Button_Event_Type.Drag;
			if (vec.Velocity.X > Drag_Velocity_Scope.X||vec.Velocity.X < -Drag_Velocity_Scope.X || vec.Velocity.Y > Drag_Velocity_Scope.Y || vec.Velocity.Y < -Drag_Velocity_Scope.Y)
			{
				vec.Enable_Drag = true;
			}
			return vec;
		}
		if(@event is InputEventScreenTouch){
			InputEventScreenTouch Touch = (InputEventScreenTouch)@event;
			vec.Position = Touch.Position;
			vec.Pressed = Touch.Pressed;
			vec.Not_Position = false;
			vec.Not_Velocity = true;
			vec.Index = Touch.Index;
			vec.Input_Type = Vec2.Button_Type.Touch;
			vec.Event_Type = Vec2.Button_Event_Type.Button;
			return vec;
		}
		if(@event is InputEventMouseButton){
			InputEventMouseButton mouseButton = (InputEventMouseButton)@event;
			vec.Position = mouseButton.Position;
			vec.Pressed = mouseButton.Pressed;
			vec.Not_Position = false;
			vec.Not_Velocity = true;
			vec.Input_Type = Vec2.Button_Type.Mouse;
			vec.Event_Type = Vec2.Button_Event_Type.Button;
			return vec;
		}
		return null;
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
	public class Vec2 : Object
	{
		public enum Button_Type{
			Null = -1,
			Mouse = 0,
			Touch = 1,
		}
		public enum Button_Event_Type
		{
			Null = -1,
			Drag = 0,
			Button = 1,
		}
		public Godot.Vector2 Position;
		public Godot.Vector2 Velocity;
		public bool Enable_Drag = false;
		public bool Not_Velocity;
		public bool Not_Position;
		public int Index;
		public bool Pressed = false;
		public Button_Event_Type Event_Type = Button_Event_Type.Null;
		public Button_Type Input_Type = Button_Type.Null;

		~Vec2(){
		}
	}
}
