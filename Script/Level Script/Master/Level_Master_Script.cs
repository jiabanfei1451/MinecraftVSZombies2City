using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Level;
/// <summary>
/// 关卡主脚本
/// </summary>
public partial class Level_Master_Script : Node2D{
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
	/// 选中的草坪
	/// </summary>
	[ExportGroup("Variant")]
	[Export] public ColorRect Selected_Lawn;
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
	[Export] public Godot.Collections.Array<Godot.Collections.Array> Lawn_Data = new Godot.Collections.Array<Godot.Collections.Array>([[]]);
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
	[Export] public PackedScene LawnScene = GD.Load<PackedScene>("uid://dim8rk13omwvv");
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
	[ExportGroup("Get_Node")]
	[Export] public Node2D Lawn_Node;
	/// <summary>
	/// 光源
	/// </summary>
	[ExportGroup("Light")]
	[Export] public float Light = 1;
	/// <summary>
	/// 用于摄像机缓动的process
	/// </summary>
	/// <param name="delta"></param>
	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (Camera2D == null){return;}
		CreateTween().TweenProperty(Camera2D,new Godot.NodePath(Godot.Camera2D.PropertyName.Position),Camera2D_Position,delta * (double)Fps_Easing);
		CreateTween().TweenProperty(Camera2D,new Godot.NodePath(Godot.Camera2D.PropertyName.Offset),Camera2D_Offset,delta * (double)Fps_Easing);
		CreateTween().TweenProperty(Camera2D,new Godot.NodePath(Godot.Camera2D.PropertyName.Zoom),Camera2D_Zoom,delta * (double)Fps_Easing);
	}
	/// <summary>
	/// 选卡
	/// </summary>
	public async void choose_Card()
    {
		Tween Twee = CreateTween();
        PackedScene Scene = GD.Load<PackedScene>("uid://bllinxtvttldn");
		Game.Get_GlobalNode.Get_Muisc_Engine(GetTree()).new_playMuisc("CH:选卡");
        Twee.TweenProperty(this,new Godot.NodePath(Level.Level_Master_Script.PropertyName.Camera2D_Position),new Vector2(125,0),1);
		await ToSignal(Twee,Tween.SignalName.Finished);
		CanvasLayer layer = Scene.Instantiate<CanvasLayer>();
		AddChild(layer);
	}
	public async void Completed_Selected_Card()
	{
		Tween Twee = CreateTween();
		Twee.TweenProperty(this,new Godot.NodePath(Level.Level_Master_Script.PropertyName.Camera2D_Position),new Vector2(0,0),1);
		await ToSignal(Twee,Tween.SignalName.Finished);
	}
	public override void _Ready() {
		base._Ready();
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
				Lawn.Position = Lawn_Spawn_Position + new Godot.Vector2(Lawn_Spawn_Offect.X * X,Lawn_Spawn_Offect.Y * Y) + Spawn_Offset;
				Lawn.Name = "Lawn(" + string.Concat(X) + "," + string.Concat(Y) + ")";
				Lawn.ME_Join += Lawn_Green;
				Lawn.ME_Exit += Lawn_Alpha;
				Lawn_Data[Y].Add(Lawn);
				Lawn_Node.AddChild(Lawn);
				}else if(Lawn_Array[Y][X] == -1)
					{
						ColorRect s = new ColorRect();
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
	public void Lawn_Green(ColorRect This)
	{
		This.Color = new Color(0,1,0,1);
		Selected_Lawn = This;
		Game.Level_Script.Lawn = This;
	}
	/// <summary>
	/// 让部分草坪变为透明
	/// </summary>
	/// <param name="This"></param>
	public void Lawn_Alpha(ColorRect This)
	{
		This.Color = new Color(0,0,0,0);
		Selected_Lawn = This;
	}
	/// <summary>
	/// 生成节点(差不多就是初始化)
	/// </summary>
	public void summand_Node()
	{
		Game.Temp_Node.Clear_Node();
		for (int Name_Index = 0; Name_Index < Node_Index.Length; Name_Index++)
		{
			GD.Print(Name_Index);
			Node Get_Node = GetNode(Node_Index[Name_Index]);
			if (Get_Node != null)
			{
				Game.Temp_Node.Add_Node(Get_Node,Node_Index[Name_Index]);
			}
			else
			{
				Get_Node = new Node2D();
				Get_Node.Name = Node_Index[Name_Index];
				AddChild(Get_Node);
				Game.Temp_Node.Add_Node(Get_Node,Node_Index[Name_Index]);
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
		GD.Print(Game.Temp_Node.nodes);
	}
}