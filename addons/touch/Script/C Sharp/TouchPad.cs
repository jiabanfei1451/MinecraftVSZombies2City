using Godot;
using System;
using System.Threading.Tasks;
namespace Touch;
[GlobalClass()]
[Icon("uid://sqy1gdavdj6y")]
public partial class TouchPad : Godot.Control
{
	#region Signal
	/// <summary>
	/// 触点按下时
	/// </summary>
	/// <param name="pad"></param>
	/// <param name="Event_Position"></param>
	[Signal]public delegate void Button_DownEventHandler(TouchPad pad,Godot.Vector2 Event_Position);
	/// <summary>
	/// 触点抬起时
	/// </summary>
	/// <param name="pad"></param>
	/// <param name="Event_Position"></param>
	[Signal]public delegate void Button_UPEventHandler(TouchPad pad,Godot.Vector2 Event_Position);
	/// <summary>
	/// 点击时
	/// </summary>
	/// <param name="pad"></param>
	/// <param name="Event_Position"></param>
	[Signal]public delegate void Button_PressedEventHandler(TouchPad pad,Godot.Vector2 Event_Position);
	/// <summary>
	/// 长按时
	/// </summary>
	/// <param name="pad"></param>
	/// <param name="Event_Position"></param>
	[Signal]public delegate void Button_Long_PressedEventHandler(TouchPad pad,Godot.Vector2 Event_Position);

	/// <summary>
	/// 拖拽开始时
	/// </summary>
	/// <param name="pad"></param>
	/// <param name="Event_Position"></param>
	/// <param name="Velocity"></param>
	[Signal]public delegate void Start_DragEventHandler(TouchPad pad,Godot.Vector2 Event_Position,Godot.Vector2 Velocity);
	/// <summary>
	/// 拖拽移动时
	/// </summary>
	/// <param name="pad"></param>
	/// <param name="Event_Position"></param>
	/// <param name="Velocity"></param>
	[Signal]public delegate void Drag_IngEventHandler(TouchPad pad,Godot.Vector2 Event_Position,Godot.Vector2 Velocity);
	/// <summary>
	/// 拖拽结束时
	/// </summary>
	/// <param name="pad"></param>
	/// <param name="Event_Position"></param>
	[Signal]public delegate void End_DragEventHandler(TouchPad pad,Godot.Vector2 Event_Position);
	/// <summary>
	/// 焦点进入时
	/// </summary>
	/// <param name="pad"></param>
	/// <param name="Event_Position"></param>
	[Signal]public delegate void Focus_JoinEventHandler(TouchPad pad,Godot.Vector2 Event_Position);
	/// <summary>
	/// 焦点离开时
	/// </summary>
	/// <param name="pad"></param>
	/// <param name="Event_Position"></param>
	[Signal]public delegate void Focus_ExitEventHandler(TouchPad pad,Godot.Vector2 Event_Position);
	/// <summary>
	/// 按下时的空值方法
	/// </summary>
	[Signal]public delegate void Button_DownvoidEventHandler();
	/// <summary>
	/// 抬起时的空值方法
	/// </summary>
	[Signal]public delegate void Button_UPvoidEventHandler();
	/// <summary>
	/// 点击时的空值方法
	/// </summary>
	[Signal]public delegate void Button_PressedvoidEventHandler();
	/// <summary>
	/// 长按时的空值方法
	/// </summary>
	/// <param name="pad"></param>
	/// <param name="Event_Position"></param>
	[Signal]public delegate void Button_Long_PressedvoidEventHandler(TouchPad pad,Godot.Vector2 Event_Position);

	/// <summary>
	/// 拖拽开始时的空值方法
	/// </summary>
	[Signal]public delegate void Start_DragvoidEventHandler();
	/// <summary>
	/// 拖拽移动时的空值方法
	/// </summary>
	[Signal]public delegate void Drag_IngvoidEventHandler();
	/// <summary>
	/// 拖拽结束时的空值方法
	/// </summary>
	[Signal]public delegate void End_DragvoidEventHandler();
	/// <summary>
	/// 焦点进入时的空值方法
	/// </summary>
	[Signal]public delegate void Focus_JoinvoidEventHandler();
	/// <summary>
	/// 焦点离开时的空值方法
	/// </summary>
	[Signal]public delegate void Focus_ExitvoidEventHandler();

	#endregion
	#region Variant
	/// <summary>
	/// 范围 仅Auto_Settings关闭时有效
	/// </summary>
	[ExportGroup("TouchPad")]
	[ExportSubgroup("Attribute")]
	[Export] public Godot.Vector2 Scope = Godot.Vector2.Zero;
	/// <summary>
	/// 拖拽判定范围
	/// </summary>
	[Export] public Godot.Vector2 Drag_Velocity_Scope = new Godot.Vector2(5,5);
	/// <summary>
	/// 触摸判定偏移
	/// </summary>
	[Export] public Godot.Vector2 Area_Offect = Godot.Vector2.Zero;
	
	/// <summary>
	/// 自动设置
	/// </summary>
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
	/// <summary>
	/// 检索索引是否启用
	/// </summary>
	[Export] public int Auto_Enable_Index = 0;
	/// <summary>
	/// 自动根据索引启用
	/// </summary>
	[Export] public bool Auto_Set_Enable = true;
	/// <summary>
	/// 触摸模式
	/// </summary>
	public enum _TouchPad_Mode
	{
		/// <summary>
		/// 普通模式
		/// </summary>
		Normal = 0
	}
	/// <summary>
	/// 触摸模式
	/// </summary>
	[Export] public _TouchPad_Mode TouchPad_Mode = _TouchPad_Mode.Normal;  
	/// <summary>
	/// 触摸索引
	/// </summary>
	[ExportSubgroup("Variant")]
	[Export] public Godot.Collections.Array<int> Touch_Index;
	/// <summary>
	/// 按下
	/// </summary>
	[Export] public bool Pressed = false;
	/// <summary>
	/// 拖拽
	/// </summary>
	[Export] public bool Drag = true;
	/// <summary>
	/// 焦点
	/// </summary>
	[Export] public bool Focus = false;
	/// <summary>
	/// 按下状态
	/// </summary>
	public enum on_Click_Type
	{
		/// <summary>
		/// 无
		/// </summary>
		not = -1,
		/// <summary>
		/// 点击
		/// </summary>
		Click = 0,
		/// <summary>
		/// 长按
		/// </summary>
		Long_Click = 1,
		/// <summary>
		/// 拖拽
		/// </summary>
		Drag = 2,
		/// <summary>
		/// 焦点
		/// </summary>
		foucs = 3,
		
	}
	/// <summary>
	/// 触摸类型
	/// </summary>
	[Export] public on_Click_Type Click_Type = on_Click_Type.not;
	/// <summary>
	/// 按下时长
	/// </summary>
	[Export] public double Pressed_Time = 0;
	/// <summary>
	/// 循环类型
	/// </summary>
	public enum Cycle_Type
	{
		/// <summary>
		/// 由_PhysicsProcess执行
		/// </summary>
		_PhysicsProcess = 0,
		/// <summary>
		/// 由无限循环执行
		/// </summary>
		While = 1
	}
	/// <summary>
	/// 无限循环帧数
	/// </summary>
	[Export] public int While_Number = 60; 
	/// <summary>
	/// 循环类型
	/// </summary>
	[Export] public Cycle_Type Cycle_mode = Cycle_Type._PhysicsProcess;
	#endregion
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
	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (Cycle_mode == Cycle_Type._PhysicsProcess){
		Plus_Time(delta);
		}
	}
	public void Plus_Time(Double Time)
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
		if (Auto_Set_Enable == true)
		{
			if (Touch.Touch_Index.Get_Index(Auto_Enable_Index) == false)
			{
				return;
			}
		}
		Godot.Vector2 Viewport_Position = GetGlobalTransformWithCanvas()[2];
		Godot.Vector2 ViewPort_Scale = new Vector2(GetGlobalTransformWithCanvas()[0].X,GetGlobalTransformWithCanvas()[1].Y);
		Godot.Vector2 ViewPort_Size = ViewPort_Scale * Size;
		Godot.Vector2 Input_Position = Vector2.Zero;

		if (!Auto_Settings){
			ViewPort_Size = Scope;
		}

		Vec2 Temp_Vec2 = null;
		Temp_Vec2 = Get_Touch_Velocity(@event);
		if (Temp_Vec2 == null){return;}
		bool OK = false;
		if (Temp_Vec2.Not_Position){return;}
		OK = Touch_Calculation(Temp_Vec2.Position,Viewport_Position,ViewPort_Size);
		//普通模式
		if (TouchPad_Mode == _TouchPad_Mode.Normal){
			//触摸设备
	#region 触摸部分
			if (Temp_Vec2.Input_Type == Vec2.Button_Type.Touch){
				//按钮
				if (Temp_Vec2.Event_Type == Vec2.Button_Event_Type.Button){
					//按下状态判定
					if (Temp_Vec2.Pressed == true){
						if (!OK){return;}
						Set_Touch_Index(0,Temp_Vec2.Index);
						Pressed = true;
						Click_Type = on_Click_Type.Click;
						EmitSignal("Button_Downvoid");
						EmitSignal("Button_Down",this,Temp_Vec2.Position);
						if (OK && Enable_Focus){
						//如果当前状态为not则设置焦点
							if (Click_Type == on_Click_Type.not){Click_Type = on_Click_Type.foucs;}
							
							if (!Focus){
								EmitSignal("Focus_Join",this,Temp_Vec2.Position);
								EmitSignal("Focus_Joinvoid");
							}
						//设置焦点状态
						Focus = true;
						}
					}
					else{
						//拖拽状态
						if (Drag){
							EmitSignal("End_Drag",this,Temp_Vec2.Position);
							EmitSignal("End_Dragvoid");
						}
						//抬起时
						EmitSignal("Button_UPvoid");
						EmitSignal("Button_UP",this,Temp_Vec2.Position);

						//触发点击事件
						if (Click_Type == on_Click_Type.Click)
						{
							EmitSignal("Button_Pressedvoid");
							EmitSignal("Button_Pressed",this,Temp_Vec2.Position);
						}
						//触发长按事件
						else if(Click_Type == on_Click_Type.Long_Click){
							EmitSignal("Button_Long_Pressedvoid");
							EmitSignal("Button_Long_Pressed",this,Temp_Vec2.Position);
						}
						//设定状态
						Pressed = false;
						Drag = false;
						Focus = false;
						Click_Type = on_Click_Type.not;
						Set_Touch_Index(1,Temp_Vec2.Index);
					}
				//判定为拖拽行为
				}else if(Temp_Vec2.Event_Type == Vec2.Button_Event_Type.Drag){
					//如果按下为真 然后获取索引 检测是否启用拖拽
					if (Pressed == true && Get_Touch_Index(Temp_Vec2.Index) != -1 && Enable_Drag){
						//设定状态
						Click_Type = on_Click_Type.Drag;
						//条件是否满足
						if (Drag == false && Temp_Vec2.Enable_Drag){
							//触发拖拽开始时
							if (!Velocity(Temp_Vec2.Velocity)){return;}
							EmitSignal("Start_Dragvoid",this,Temp_Vec2.Position,Temp_Vec2.Velocity);
							EmitSignal("Start_Drag");
							Drag = true;
						}else
						{
							//触发拖拽中
							EmitSignal("Drag_Ing",this,Temp_Vec2.Position,Temp_Vec2.Velocity);
							EmitSignal("Drag_Ingvoid");
						}
						
					}
					//焦点
					if (OK && Enable_Focus)
					{
						//如果当前状态为not则设置焦点
						if (Click_Type == on_Click_Type.not){Click_Type = on_Click_Type.foucs;}
						if (!Focus){
							EmitSignal("Focus_Join",this,Temp_Vec2.Position);
							EmitSignal("Focus_Joinvoid");
						}
						//设置焦点状态
						Focus = true;
						}
					else
					{	
						//焦点离开时事件触发
						if (Click_Type == on_Click_Type.foucs){Click_Type = on_Click_Type.not;}
						if (Focus){
							EmitSignal("Focus_Exit",this,Temp_Vec2.Position);
							EmitSignal("Focus_Exitvoid");
					}
						Focus = false;
					}
				}
			}
#endregion
#region 鼠标设备
			//鼠标设备
			else if(Temp_Vec2.Input_Type == Vec2.Button_Type.Mouse)
			{
				//按下
				if (Temp_Vec2.Event_Type == Vec2.Button_Event_Type.Button)
				{
					//按下
					if (Temp_Vec2.Pressed == true)
					{
						if (!OK){return;}
						Pressed = true;
						Click_Type = on_Click_Type.Click;
					}
					//结束拖拽
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
				//拖拽
				else if(Temp_Vec2.Event_Type == Vec2.Button_Event_Type.Drag)
				{
					//按下状态检测 拖拽启用状态
					if (Pressed == true && Temp_Vec2.Enable_Drag && Enable_Drag)
					{
						//拖拽开始时
						Click_Type = on_Click_Type.Drag;
						if (Drag == false)
							{
								if (!Velocity(Temp_Vec2.Velocity)){return;}
								EmitSignal("Start_Dragvoid",this,Temp_Vec2.Position,Temp_Vec2.Velocity);
								EmitSignal("Start_Drag");
								Drag = true;
							}
							//拖拽中
							else
							{
								EmitSignal("Drag_Ing",this,Temp_Vec2.Position,Temp_Vec2.Velocity);
								EmitSignal("Drag_Ingvoid");
							}
					}
					//焦点进入检测
					if (OK)
					{
						//焦点进入时
						if (Click_Type == on_Click_Type.not){Click_Type = on_Click_Type.foucs;
						}
						if (!Focus){
						EmitSignal("Focus_Join",this,Temp_Vec2.Position);
						EmitSignal("Focus_Joinvoid");
						}
						Focus = true;
						}
					else//焦点离开时
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
	#endregion
	/// <summary>
	/// 如果 <seealso cref="int"/> Index 存在时返回当前的Index 否则返回 -1
	/// </summary>
	/// <param name="Index"></param>
	/// <returns></returns>
	public int Get_Touch_Index(int Index)
	{
		if (Touch_Index != null && !Touch_Index.Contains(Index)){return -1;}
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
				if (Touch_Index != null && Touch_Index.IndexOf(Index) == -1)
				{
					Touch_Index.Add(Index);
				}
				break;
			case 1:
				if (Touch_Index != null && Touch_Index.IndexOf(Index) != -1)
				{
					Touch_Index.Remove(Index);
				}
				break;
		}
	}
	/// <summary>
	/// 检测速度能否触发滑动
	/// </summary>
	/// <param name="velocity"></param>
	/// <returns></returns>
	public bool Velocity(Godot.Vector2 velocity)
	{
		bool x = false;
		bool y = false;
		if (velocity.X > Drag_Velocity_Scope.X){
			DEBUG.Info.Print(1);
			x = true;
		}
		if (velocity.Y > Drag_Velocity_Scope.Y){
			DEBUG.Info.Print(2);
			y = true;
			}
		if (velocity.X < -Drag_Velocity_Scope.X){
			DEBUG.Info.Print(3);
			x = true;
			}
		if (velocity.Y < -Drag_Velocity_Scope.Y){
			DEBUG.Info.Print(4);
			y = true;
		}
		DEBUG.Info.Print(Drag_Velocity_Scope);
		DEBUG.Info.Print(velocity);
		DEBUG.Info.Print(x,y);
		if (x || y){return true;}
		return false;
	}
	/// <summary>
	/// 获取数值
	/// </summary>
	/// <param name="event"></param>
	/// <returns></returns>
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
	/// <summary>
	/// 计算是否进入范围
	/// </summary>
	/// <param name="Event_Position"></param>
	/// <param name="Object_Position"></param>
	/// <param name="Scope"></param>
	/// <returns></returns>
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
	/// <summary>
	/// 临时数据
	/// </summary>
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
