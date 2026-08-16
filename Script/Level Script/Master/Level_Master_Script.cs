using Game;
using Godot;
using System;
using DEBUG;

namespace Level;
/// <summary>
/// 关卡主脚本
/// </summary>
public partial class Level_Master_Script : Node2D{
	#region 变量
	[ExportCategory("看什么?难道你不知道脚本里有中文注释吗?")]
	/// <summary>
	/// 节点生成
	/// </summary>
	[ExportGroup("Layer")]
	[Export] public String[] Node_Index = ["Equipment","Master","Light"];
	/// <summary>
	/// 图层分配
	/// </summary>
	[Export] public int[] Layer_Index = [0,1,2];
	/// <summary>
	/// 类型分配
	/// 0 = Node,
	/// 1 = Viewport
	/// </summary>
	[Export] public int[] Node_Type = [0,0,1];
	/// <summary>
	/// 选中的草坪
	/// </summary>
	[ExportGroup("Variant")]
	[Export] public Lawn Selected_Lawn;
	/// <summary>
	/// 草坪生成数组
	/// </summary>
	[Export] public Godot.Collections.Array<Godot.Collections.Array<int>> Lawn_Array = new Godot.Collections.Array<Godot.Collections.Array<int>>()
	{
		new Godot.Collections.Array<int>(){0,0,0,0,0,0,0,0,0},
		new Godot.Collections.Array<int>(){0,0,0,0,0,0,0,0,0},
		new Godot.Collections.Array<int>(){0,0,0,0,0,0,0,0,0},
		new Godot.Collections.Array<int>(){0,0,0,0,0,0,0,0,0},
		new Godot.Collections.Array<int>(){0,0,0,0,0,0,0,0,0},
	};
	/// <summary>
	/// 草坪偏移
	/// </summary>
	[Export] public Godot.Collections.Array<Godot.Collections.Array<Godot.Vector2>> Lawn_Offset_Array = new Godot.Collections.Array<Godot.Collections.Array<Vector2>>([[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[]]);
	/// <summary>
	/// 草坪实例化后的数据
	/// </summary>
	[Export] public Godot.Collections.Array<Godot.Collections.Array<Lawn>> Lawn_Data = new Godot.Collections.Array<Godot.Collections.Array<Lawn>>([[]]);
	/// <summary>
	/// 自动生成草坪
	/// </summary>
	[Export] public bool Auto_Spawn_Lawn = true;
	/// <summary>
	/// 草坪每次生成后的偏移
	/// </summary>
	[Export] public Godot.Vector2 Lawn_Spawn_Offect = new Godot.Vector2(80,80);
	/// <summary>
	/// 草坪开始生成坐标
	/// </summary>
	[Export] public Godot.Vector2 Lawn_Spawn_Position = new Godot.Vector2(-428,-181);
	/// <summary>
	/// 用于实例化的草坪场景
	/// </summary>
	[Export] public PackedScene LawnScene;
	/// <summary>
	/// 摄像机
	/// </summary>
	[ExportGroup("Node")]
	[Export] public Godot.Camera2D Camera2D = null;
	/// <summary>
	/// 相机坐标
	/// </summary>
	[Export] public Godot.Vector2 Camera2D_Position = new Godot.Vector2(0,0);
	/// <summary>
	/// 相机偏移
	/// </summary>
	[Export] public Godot.Vector2 Camera2D_Offset = new Godot.Vector2(0,-25);
	/// <summary>
	/// 相机聚焦
	/// </summary>
	[Export] public Godot.Vector2 Camera2D_Zoom = new Godot.Vector2(1,1);
	/// <summary>
	/// 缓动帧率
	/// </summary>
	[Export] public int Fps_Easing = 30;
	/// <summary>
	/// 草坪场景
	/// </summary>
	[Export] public Tween Camera2D_Easing = null;
	[ExportGroup("Get_Node")]
	[Export] public Node2D Lawn_Node;
	/// <summary>
	/// 光源
	/// </summary>
	[ExportGroup("Light")]
	[Export] public float Light = 1;
	/// <summary>
	/// 光源偏移
	/// </summary>
	[Export] public float Light_Offset = 1;
	/// <summary>
	/// 用于摄像机缓动的process
	/// </summary>
	/// <param name="delta"></param>
	#endregion
	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (Camera2D == null){return;}
		if (Camera2D_Easing != null){Camera2D_Easing.Kill();}
		Camera2D_Easing = CreateTween();
		Camera2D_Easing.TweenProperty(Camera2D,new Godot.NodePath(Godot.Camera2D.PropertyName.Position),Camera2D_Position,delta * (double)Fps_Easing);
		Camera2D_Easing.Parallel().TweenProperty(Camera2D,new Godot.NodePath(Godot.Camera2D.PropertyName.Offset),Camera2D_Offset,delta * (double)Fps_Easing);
		Camera2D_Easing.Parallel().TweenProperty(Camera2D,new Godot.NodePath(Godot.Camera2D.PropertyName.Zoom),Camera2D_Zoom,delta * (double)Fps_Easing);
	}
	/// <summary>
	/// 选卡
	/// </summary>
	public async void choose_Card()
    {
		Tween Twee = CreateTween();
        PackedScene Scene = GD.Load<PackedScene>("uid://bllinxtvttldn");
		Game.Get_GlobalNode.Get_Muisc_Engine(GetTree()).new_playMuisc("CH:选卡");
		Camera2D_Zoom = new Godot.Vector2(1.1f,1.1f);
        Twee.TweenProperty(this,new Godot.NodePath(Level.Level_Master_Script.PropertyName.Camera2D_Position),new Vector2(140,0),1);
		await ToSignal(Twee,Tween.SignalName.Finished);
		CanvasLayer layer = Scene.Instantiate<CanvasLayer>();
		AddChild(layer);
	}
	public async void Completed_Selected_Card()
	{
		Tween Twee = CreateTween();
		Twee.TweenProperty(this,new Godot.NodePath(Level.Level_Master_Script.PropertyName.Camera2D_Position),new Vector2(-105,0),1);
		await ToSignal(Twee,Tween.SignalName.Finished);
	}
	public override void _Ready() {
		base._Ready();
		LawnScene = GD.Load<PackedScene>("uid://dim8rk13omwvv");
		Touch.Touch_Index.Set_Index_Enable(0,false);
		Game.Get_GlobalNode.Node_Data.Clear_Node();
		Game.Get_GlobalNode.Node_Data.Add_Node(this,"Level");
		summand_Node();
		if (Camera2D == null)
		{
			Camera2D = new Camera2D(); 
			Camera2D.Position = Camera2D_Position;
			Camera2D.Offset = Camera2D_Offset;
			Camera2D.Zoom = Camera2D_Zoom;
			AddChild(Camera2D);
			Game.Get_GlobalNode.Node_Data.Add_Node(Camera2D,"Camera2D");
		}
		Lawn_Node = GetNode<Node2D>("Lawn");
		Lawn_Data.Resize(Lawn_Array.Count);
		if (Auto_Spawn_Lawn){
		for (int Y = 0; Y < Lawn_Array.Count; Y++)
		{
			for (int X = 0; X < Lawn_Array[Y].Count; X++)
			{
				Godot.Vector2 Spawn_Offset = new Godot.Vector2(0,0);
				if (Y < Lawn_Offset_Array.Count)
					{
						if (X < Lawn_Offset_Array[Y].Count)
						{
							Spawn_Offset = Lawn_Offset_Array[Y][X];
						}
					}
				if (Lawn_Array[Y][X] == 0){
				Level.Lawn Lawn = LawnScene.Instantiate<Level.Lawn>();
				Lawn.ArrayPosition = new Vector2I(X,Y);
				Lawn.Position = Lawn_Spawn_Position + new Godot.Vector2(Lawn_Spawn_Offect.X * X,Lawn_Spawn_Offect.Y * Y) + Spawn_Offset;
				Lawn.Name = "Lawn(" + string.Concat(X) + "," + string.Concat(Y) + ")";
				Lawn.ME_Join += Lawn_Green;
				Lawn_Data[Y].Add(Lawn);
				Lawn_Node.AddChild(Lawn);
				}else if(Lawn_Array[Y][X] == -1)
					{
						Lawn s = new Lawn();
						Lawn_Data[Y].Add(s);
						s.QueueFree();

					}
			}
		}
		}
	}
	/// <summary>
	/// 使选中的草坪变为绿色
	/// </summary>
	/// <param name="This"></param>
	public void Lawn_Green(Level.Lawn This)
	{
		if (Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object == null){return;}
		foreach (var Arra in Lawn_Data)
		{
			foreach (Lawn ARR in Arra)
			{
				ARR.Free_Object();
				ARR.Color =  new Color(0,0,0,0);
			}
		}
		foreach (Lawn lawn in Lawn_Data[This.ArrayPosition.Y])
		{
			lawn.Color = new Color(1,1,1,0.5f);
		}
		foreach(Godot.Collections.Array<Lawn> lawns in Lawn_Data)
		{
			lawns[This.ArrayPosition.X].Color = new Color(1,1,1,0.5f);
		}
		if (Level_Script.Lawn == This && This.Current_Object.Equipment_Object == null){
			Card_Data.GlobalData Temp_Data = Get_GlobalNode.Get_Card_Data(GetTree()).Selected_raw_Object.Mode_Data.gameing_Mode.Card_Data;
			PackedScene Scene = Temp_Data.Scene;
			Node2D new_Node2d = Scene.Instantiate<Node2D>();
			new_Node2d.Name = "-1+1-1+1_CS";
			if (new_Node2d is Level.Object.Data)
			{
				Level.Object.Data Temp_Node = (Level.Object.Data)new_Node2d;
				Temp_Node.Enable = false;
			}
			This.AddChild(new_Node2d);
			new_Node2d.Position = Temp_Data.Map_Offset;
			new_Node2d.Modulate = new Color(1,1,1,0.5f);
			This.Color = new Color(0,1,0,1);
			Selected_Lawn = This;
			Level_Script.Lawn = This;
		}
		else
		{
			This.Color = new Color(1,0,0,1);
			Selected_Lawn = This;
			Level_Script.Lawn = This;
		}
	}
	/// <summary>
	/// 生成节点(差不多就是初始化)
	/// </summary>
	public void summand_Node()
	{
		Game.Temp_Node.Clear_Node();
		for (int Name_Index = 0; Name_Index < Node_Index.Length; Name_Index++)
		{
			DEBUG.Info.Print(Name_Index);
			//检测
			Node Get_Node = GetNode(Node_Index[Name_Index]);
			if (Get_Node != null)
			{
				Game.Get_GlobalNode.Node_Data.Add_Node(Get_Node,Node_Index[Name_Index]);
			}
			//否则生成
			else
			{
				switch (Node_Type[Name_Index])
				{
					case 0:
						Get_Node = new Node2D();
						Get_Node.Name = Node_Index[Name_Index];
						AddChild(Get_Node);
						Game.Get_GlobalNode.Node_Data.Add_Node(Get_Node,Node_Index[Name_Index]);
						break;
					case 1:
						// 生成SubViewprot
						Get_Node = new SubViewport();
						Get_Node.Name = Node_Index[Name_Index];
						AddChild(Get_Node);
						Game.Get_GlobalNode.Node_Data.Add_Node(Get_Node,Node_Index[Name_Index]);
						// SubViewport设置
						SubViewport viewport = (SubViewport)Get_Node; // 转换
						Get_Node = new Sprite2D(); //生成纹理承载节点
						Sprite2D sprite = (Sprite2D)Get_Node; // 转换
						ViewportTexture texture = viewport.GetTexture();
						viewport.TransparentBg = true;
						Get_Node.Name = Node_Index[Name_Index] + "Sprite2D"; //继承纹理名称
						AddChild(Get_Node);
						Game.Get_GlobalNode.Node_Data.Add_Node(Get_Node,Node_Index[Name_Index]); //注册名称
						sprite.Texture = texture; //获取纹理
						break;
					case 2:
						Get_Node = new CanvasGroup();
						Get_Node.Name = Node_Index[Name_Index];
						AddChild(Get_Node);
						Game.Get_GlobalNode.Node_Data.Add_Node(Get_Node,Node_Index[Name_Index]);
						break;
				}
			
			}
			if (Name_Index < Layer_Index.Length)
			{
				if (Get_Node is Node2D)
				{
					Node2D Get_Node2D = (Node2D)Get_Node;
					Get_Node2D.ZIndex = Layer_Index[Name_Index];
				}
			}
		}
		DEBUG.Info.Print(Game.Get_GlobalNode.NodeData);
	}
}