using Game;
using Godot;
using System;
using My_Csharp_Node;
using System.Threading.Tasks;
using DEBUG;
using System.Text.RegularExpressions;
using Level.Static;
using Test;
using NETDNS;
using System.Text.Json;
using System.Text;
using Data;
using Game.Cheak;

namespace Level;
/// <summary>
/// 关卡主脚本
/// </summary>
public partial class Level_Master_Script : Node2D{
	/// <summary>
	/// 物体死亡
	/// </summary>
	/// <param name="level"></param>
	[Signal] public delegate void Object_KillEventHandler(Level.Object.LevelObject level);
	/// <summary>
	/// 关卡Data实例更改行时
	/// </summary>
	/// <param name="Data_Object"></param>
	[Signal]
	public delegate void Object_Change_LineEventHandler(Level.Object.LevelObject Data_Object);
	/// <summary>
	/// 物体高度变更时
	/// </summary>
	/// <param name="Data_Object"></param>
	[Signal]
	public delegate void Object_Change_HeightEventHandler(Level.Module.ObjectPhysics Data_Object);
	#region 变量
	[Export] public bool DeBug {get;set;} = true;
	[ExportCategory("看什么?难道你不知道脚本里有中文注释吗?")]
	[ExportGroup("BGM")][Export] public String Level_BGMID {get;set;} = "0";
	/// <summary>
	/// 节点生成
	/// </summary>
	[ExportGroup("Layer")]
	[Export] public String[] Node_Index {get;set;} = ["Equipment","Master","Light"];
	/// <summary>
	/// 图层分配
	/// </summary>
	[Export] public int[] Layer_Index {get;set;} = [0,1,2];
	/// <summary>
	/// 类型分配
	/// 0 = Node,
	/// 1 = Viewport,
	/// </summary>
	[Export] public int[] Node_Type {get;set;} = [0,0,1];
	/// <summary>
	/// 选中的草坪
	/// </summary>
	[ExportGroup("Variant")]
	[Export] public Lawn Selected_Lawn {get;set;}
	/// <summary>
	/// 草坪生成数组
	/// </summary>
	[Export] public Godot.Collections.Array<Godot.Collections.Array<int>> Lawn_Array {get;set;} = new Godot.Collections.Array<Godot.Collections.Array<int>>()
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
	[Export] public Godot.Collections.Array<Godot.Collections.Array<Godot.Vector2>> Lawn_Offset_Array {get;set;} = new Godot.Collections.Array<Godot.Collections.Array<Vector2>>([[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[]]);
	/// <summary>
	/// 草坪实例化后的数据
	/// </summary>
	[Export] public Godot.Collections.Array<Godot.Collections.Array<Lawn>> Lawn_Data {get;set;} = new Godot.Collections.Array<Godot.Collections.Array<Lawn>>([[]]);
	/// <summary>
	/// 物体索引
	/// </summary>
	[Export] public Godot.Collections.Array<Godot.Collections.Array<Level.Module.ObjectPhysics>> Lawn_Object_Index {get;set;} = new Godot.Collections.Array<Godot.Collections.Array<Level.Module.ObjectPhysics>>();
	/// <summary>
	/// 物体索引坐标偏移
	/// </summary>
	[Export] public Godot.Collections.Array<Godot.Vector2> Check_Position_Offset {get;set;} = new Godot.Collections.Array<Vector2>();
	/// <summary>
	/// 自动生成草坪
	/// </summary>
	[Export] public bool Auto_Spawn_Lawn {get;set;} = true;
	/// <summary>
	/// 草坪每次生成后的偏移
	/// </summary>
	[Export] public Godot.Vector2 Lawn_Spawn_Offect {get;set;} = new Godot.Vector2(80,80);
	/// <summary>
	/// 草坪开始生成坐标
	/// </summary>
	[Export] public Godot.Vector2 Lawn_Spawn_Position {get;set;} = new Godot.Vector2(-428,-181);
	/// <summary>
	/// 用于实例化的草坪场景
	/// </summary>
	[Export] public PackedScene LawnScene {get;set;} = null;
	/// <summary>
	/// 摄像机
	/// </summary>
	[ExportGroup("Node")]
	[Export] public Godot.Camera2D Camera2D {get;set;} = null;
	/// <summary>
	/// 相机坐标
	/// </summary>
	[Export] public Godot.Vector2 Camera2D_Position {get;set;} = new Godot.Vector2(0,0);
	/// <summary>
	/// 相机偏移
	/// </summary>
	[Export] public Godot.Vector2 Camera2D_Offset {get;set;} = new Godot.Vector2(0,-25);
	/// <summary>
	/// 相机聚焦
	/// </summary>
	[Export] public Godot.Vector2 Camera2D_Zoom {get;set;} = new Godot.Vector2(1,1);
	/// <summary>
	/// 缓动帧率
	/// </summary>
	[Export] public int Fps_Easing {get;set;} = 30;
	/// <summary>
	/// 草坪场景
	/// </summary>
	[Export] public Tween Camera2D_Easing {get;set;} = null;
	[ExportGroup("Get_Node")]
	[Export] public Node2D Lawn_Node {get;set;} = null;
	/// <summary>
	/// 光源
	/// </summary>
	[ExportGroup("Light")]
	[Export] public float Light {get;set;} = 1;
	/// <summary>
	/// 光源偏移
	/// </summary>
	[Export] public float Light_Offset {get;set;} = 1;
	/// <summary>
	/// Ready执行完成
	/// </summary>
	[Export] public bool Game_Reset_Done {get;set;} = false;
	/// <summary>
	/// 已完成种子获取
	/// </summary>
	internal bool Seed_OK {get;set;} = false;
	/// <summary>
	/// 临时音频缩放
	/// </summary>
	internal byte Temp_audio_Scale {get;set;} = 0;
	/// <summary>
	/// 临时倒计时
	/// </summary>
	internal float Temp_audio_await_timer {get;set;} = 0; 
	#endregion
	/// <summary>
	/// 用于摄像机缓动的process
	/// </summary>
	/// <param name="delta"></param>
	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (Temp_audio_Scale != Level_Script.audio_Scale)
		{
			Temp_audio_Scale = Level_Script.audio_Scale;
			Temp_audio_await_timer = 3;
		}
		else
		{
			if (Temp_audio_await_timer > 0){
				Temp_audio_await_timer -= (float)delta;
			}
			else
			{
				if (Level_Script.audio_Scale != 0)
				{
					Level_Script.audio_Scale = 0;
				}
			}
		}
		if (Seed_OK == false){
			if (Game.Get_GlobalNode.CommandEdit != null)
			{
				Seed_OK = true;
				Game.Get_GlobalNode.CommandEdit.Player_send += Get_Player_Send;
			}
		}
		if (Static.Summand.Level_Object != this) // 初始化生成物体
		{
			Static.Summand.Level_Object = this;
			Static.Summand.Tree = GetTree();
		}
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
		var s = Game.Get_GlobalNode.Node_Data.Get_Node<CanvasLayer>("Current_Level_UI", Get_GlobalNode.Node_Data.Mode_Type.Name);
		if (s != null)
		{
			s.QueueFree();
		}
		Game.Get_GlobalNode.Node_Data.Remove_Node("Current_Level_UI", Get_GlobalNode.Node_Data.Mode_Type.Name);
		Game.Get_GlobalNode.Node_Data.Screening_Not_Null_Node();

		Touch.Touch_Index.clear();
		Game.Get_GlobalNode.Get_Card_Data(GetTree()).Initialization();
		Tween Twee = CreateTween();
        PackedScene Scene = Game.ResourceScene.LoadScene("uid://bllinxtvttldn");
		Game.Get_GlobalNode.Get_Muisc_Engine(GetTree()).new_playMuisc("CH:选卡");
		Camera2D_Zoom = new Godot.Vector2(1.1f,1.1f);
        Twee.TweenProperty(this,new Godot.NodePath(Level.Level_Master_Script.PropertyName.Camera2D_Position),new Vector2(140,0),1);
		await ToSignal(Twee,Tween.SignalName.Finished);
		CanvasLayer layer = Scene.Instantiate<CanvasLayer>();
		AddChild(layer);
	}
	/// <summary>
	/// 游戏开始时，负责开始游戏
	/// </summary>
	public async void Game_Start()
	{
		Audio_Plus s = new Audio_Plus();
		s.Audio_Type = Audio_Plus.Audio.Souds;
		s.Auto_QueneFree = true;
		s.Stream = Game.Get_GlobalNode.Get_Audio_List(GetTree()).Get_Souds("MVZ2:Ready");
		AddChild(s);
		s.Play();
		await Game.Tip.Set_Ready_Text(true,0.5d,true,2,1,"好!");
		await Task.Delay(500);
		await Game.Tip.Set_Ready_Text(true,0.5d,true,2,1,"准备!");
		await Task.Delay(500);
		await Game.Tip.Set_Ready_Text(true,0.5d,true,2,1,"安放器械!!!");
		await Task.Delay(1000);
		Game.Tip.Set_Ready_Text("");
		Game.Get_GlobalNode.Get_Muisc_Engine(GetTree()).new_playMuisc(((Level.Level_Master_Script)GetTree().CurrentScene).Level_BGMID);
		Game.Get_GlobalNode.Get_Card_Data(GetTree()).CD_Initialization();
		if (Game.Get_GlobalNode.Node_Data.Get_Node<UIObject.LevelUi>("LevelUI") != null){
			Game.Get_GlobalNode.Node_Data.Get_Node<UIObject.LevelUi>("LevelUI")?.Card_Initialization();
		}
		if (Game.Get_GlobalNode.Node_Data.Get_Node<Control>("LevelUI2", Get_GlobalNode.Node_Data.Mode_Type.Name) != null){
			Game.Get_GlobalNode.Node_Data.Get_Node<Control>("LevelUI2", Get_GlobalNode.Node_Data.Mode_Type.Name).QueueFree();
		}
		Touch.Touch_Index.Set_Index_Enable(1,true);
		Static.Summand.While_Start();
		if (Game.Get_GlobalNode.Node_Data.Get_Node<UIObject.LevelUi>("LevelUI2") != null){
			Game.Get_GlobalNode.Node_Data.Get_Node<UIObject.LevelUi>("LevelUI2").QueueFree();
		}
	}
	/// <summary>
	/// 完成选卡
	/// </summary>
	public async void Completed_Selected_Card()
	{
		Game.Get_GlobalNode.Node_Data.Get_Node<Node2D>("Equipment").YSortEnabled = true;
		Game.Get_GlobalNode.Node_Data.Get_Node<Node2D>("Monster").YSortEnabled = true;
		Tween Twee = CreateTween();
		Twee.TweenProperty(this,new Godot.NodePath(Level.Level_Master_Script.PropertyName.Camera2D_Position),new Vector2(-105,0),1);
		await ToSignal(Twee,Tween.SignalName.Finished);
	}
	/// <summary>
	/// 读取玩家发送的指令
	/// </summary>
	/// <param name="Seed_Why"></param>
	public void Get_Player_Send(String Seed_Why)
	{
		
		if (Seed_Why.IndexOf("/ReStart") != -1)
		{
			choose_Card();
			Summand.While_End();
		}else if (Seed_Why.IndexOf("/Exit") != -1)
		{
			GetTree().Quit();
		}else if (Seed_Why.IndexOf("/Set ") != -1)
		{
			string ps = Regex.Replace(Seed_Why,@"[^0-9]+","");
			short p = (short)ps.ToInt();
			Game.Level_Script.Equipment_Capable = p;
		}else if(Seed_Why.IndexOf("/GetData") != -1)
		{
			DEBUG.Info.Print(Game.Static.PlayerData.Player_Data);
		}else if(Seed_Why.IndexOf("/SaveInfo") != -1)
		{
			DEBUG.Info.Save_Info("C:\\Info.txt");
		}else if(Seed_Why.IndexOf("/CreateServer") != -1)
		{
			Server.Create_Server();
		}else if(Seed_Why.IndexOf("/Join") != -1)
		{
			Server.Join_Server();
		}else if(Seed_Why.IndexOf("/OffServer") != -1)
		{
			Server.Off_Server();
		}else if(Seed_Why.IndexOf("/Save") != -1)
		{
			Summand.Save();
		}
		if (Server.Server_Stream != null)
		{
			if (Server.TCPServer != null)
			{
				Server.Server_Send_Data(Seed_Why);
			}
			else
			{
				Server.Player_Send_Data(Seed_Why);
			}
		}
	}
	#region 草坪方法
	/// <summary>
	/// 使选中的草坪根据条件变更颜色
	/// </summary>
	/// <param name="This"></param>
	public void Lawn_Change_Color(Level.Lawn This)
	{
		Game.Card_Data card_Data = Get_GlobalNode.Get_Card_Data(GetTree());
		if (card_Data == null){return;}
		if (card_Data.Selected_raw_Object == null){return;}
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
			Lawn_Current_Color(lawn);
		}
		foreach(Godot.Collections.Array<Lawn> lawns in Lawn_Data)
		{
			Lawn_Current_Color(lawns[This.ArrayPosition.X]);
		}
		GlobalData Temp_data = card_Data.Selected_raw_Object.Mode_Data.gameing_Mode.Card_Data;
		String Get_Object_Type = Temp_data.Object_Type.ToString();
		if (Level_Script.Lawn == This && This.Current_Object.Has_Key_Object(Get_Object_Type) == null && Temp_data.Object_Type == MVZ2.Type.ObjectType.Normal && (Temp_data.Reliant_Tag.Count <= 0 && Temp_data.Reliant_UUID.Count <= 0)){
			Change_Lawn(This,new Color(0,1,0,1));
		}else if (Cheak.Cheak_Data(This.Current_Object.Has_Key_Object(Get_Object_Type), Temp_data))
		{
			Change_Lawn(This,new Color(0,1,0,1));
		}
		else
		{
			Change_Lawn(This,new Color(1,0,0,1),false);
		}
	}
	internal void Change_Lawn(Level.Lawn This,Color color,bool Summand_Phantom = true)
	{
		if (Summand_Phantom == true){
			This.Summand_Phantom();
		}
		This.Color = color;
		Selected_Lawn = This;
		Level_Script.Lawn = This;
	}
	/// <summary>
	/// 根据器械状态判定此草坪能否被选用
	/// </summary>
	/// <param name="lawn"></param>
	public void Lawn_Current_Color(Level.Lawn lawn)
	{
		Card_Data data = Game.Get_GlobalNode.Get_Card_Data(GetTree());
		MVZ2.Type.ObjectType Types = data.Selected_raw_Object.Mode_Data.gameing_Mode.Card_Data.Object_Type;
		if (Types == MVZ2.Type.ObjectType.Normal)
		{
			if (lawn.Current_Object.Has_Key_Object("Normal") == null)
			{
				lawn.Color = new Color(1,1,1,0.5f);
			}
		}
	}
	public void Summand_Lawn()
	{
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
				Object_Kill += Lawn.Object_Kill;
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
	/// <summary>
	/// 初始化草坪行数索引
	/// </summary>
	public void Reset_Lawn_Index()
	{
		Lawn_Object_Index.Resize(Lawn_Array.Count);
	}
	/// <summary>
	/// 移动索引物体
	/// </summary>
	public void Move_Lawn_Index(Level.Module.ObjectPhysics Data_Object,int Index)
	{
		if (!Check_Lawn_Index(Index)){return;}
		if (!Check_Lawn_Index(Data_Object.Lawn_Index)){return;}
		int Temp_Index = Data_Object.Lawn_Index;
		if (Temp_Index == Index){return;}
		Data_Object.Lawn_Index = Index;
		Add_Lawn_Index(Data_Object,Index);
		Remove_Lawn_Index(Data_Object,Temp_Index);
		EmitSignal("Object_Change_Line",Data_Object);
	}
	/// <summary>
	/// 添加索引物体
	/// </summary>
	public void Add_Lawn_Index(Level.Module.ObjectPhysics Data_Object,int Index)
	{
		if (!Check_Lawn_Index(Index)){return;}
		Lawn_Object_Index[Index].Add(Data_Object);
	}
	/// <summary>
	/// 删除索引物体
	/// </summary>
	public void Remove_Lawn_Index(Level.Module.ObjectPhysics Data_Object,int Index)
	{
		if (!Check_Lawn_Index(Index)){return;}
		Lawn_Object_Index[Index].Remove(Data_Object);
	}
	/// <summary>
	/// 遍历数组删除空值
	/// </summary>
	/// <param name="Auto_Delete"></param>
	public void Remove_Lawn_Index(bool Auto_Delete)
	{
		for(int i = 0;i < Lawn_Data.Count;i++)
		{
			foreach(Level.Object.LevelObject Data_Object in Lawn_Object_Index[i])
			{
				if (Data_Object is null)
				{
					Lawn_Object_Index[i].Remove(Data_Object);
				}
			}
		}
	}
	/// <summary>
	/// 检查此草坪行索引是否存在
	/// </summary>
	/// <param name="Index"></param>
	/// <returns></returns>
	private bool Check_Lawn_Index(int Index)
	{
		if (Index < Lawn_Object_Index.Count)
		{
			return true;
		}
		return false;
	}
	/// <summary>
	/// 获取草坪索引
	/// </summary>
	/// <param name="This"></param>
	/// <returns></returns>
	public int Get_LawnIndex(Godot.Vector2 Position,Godot.Vector2 Offset_Position)
    {
		// 草坪生成坐标
        float Position_Y = Lawn_Spawn_Position.Y;
        // 增加索引偏移量
		float IndexNumber = Lawn_Spawn_Offect.Y;
		// 当前草坪索引
        int Current_Lawn_Index = 0;
		// 最大索引
        int MaxIndex = Lawn_Object_Index.Count;
        // Y坐标偏移
		float Y_offset = Get_This_Offset(Position).Y;
		// 坐标索引
		float Index = Position.Y + Offset_Position.Y + Y_offset;
        
		while(Index >= Position_Y)
        {
            if (Index >= Position_Y)
            {
                Index -= IndexNumber;
                Current_Lawn_Index += 1;
            }
        }
        Current_Lawn_Index -= 1;
        if (Current_Lawn_Index >= MaxIndex)
        {
            Current_Lawn_Index = MaxIndex -1;
        }
        if (Current_Lawn_Index < 0)
        {
            Current_Lawn_Index = 0;
        }
        return Current_Lawn_Index;
    }
	/// <summary>
	/// 获取偏移坐标
	/// </summary>
	/// <param name="This"></param>
	/// <returns></returns>
	public Vector2 Get_This_Offset (Godot.Vector2 position)
	{
		Vector2 Back_Vector2 = Vector2.Zero;
		if (Check_Position_Offset.Count == 0)
		{
			return Vector2.Zero;
		}
		foreach(Vector2 vector in Check_Position_Offset)
		{
			Back_Vector2 = vector;
			if (position.X > vector.X)
			{
				break;
			}
		}
		return Back_Vector2;
	}
	/// <summary>
	/// 获取当前偏移索引
	/// </summary>
	/// <param name="This"></param>
	/// <returns></returns>
	public int Get_Offset_Index(Level.Object.LevelObject This)
	{
		int Back_Index = -1;
		if (Check_Position_Offset.Count == 0)
		{
			return -1;
		}
		foreach(Vector2 vector in Check_Position_Offset)
		{
			Back_Index += 1;
			if (This.Position.X > vector.X)
			{
				return Back_Index;
			}
		}
		return -1;
	}
    #endregion
	/// <summary>
	/// 退出树时
	/// </summary>
    public override void _ExitTree()
    {
        base._ExitTree();
		Object_Kill -= Static.Summand.Monster_Kill;
	}
	/// <summary>
	/// 初始化
	/// </summary>
	public override async void _Ready() {
		base._Ready();
		Object_Kill += Static.Summand.Monster_Kill;
		Reset_Lawn_Index();
		Level_Script.Level_Object = this;
		LawnScene = Game.ResourceScene.LoadScene("uid://dim8rk13omwvv");
		Touch.Touch_Index.Set_Index_Enable(0,false);
		Game.Get_GlobalNode.Node_Data.Clear_Node();
		Game.Get_GlobalNode.Node_Data.Add_Node(this,"Level");
		summand_Node();
		await Task.Delay(100);
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
		if (Auto_Spawn_Lawn)
		{
			Summand_Lawn();
		}
		Game_Reset_Done = true;
	}
	/// <summary>
	/// 生成节点(差不多就是初始化)
	/// </summary>
	public void summand_Node()
	{
		for (int Name_Index = 0; Name_Index < Node_Index.Length; Name_Index++)
		{
			DEBUG.Info.Print(Name_Index);
			//检测
			Node Get_Node = GetNodeOrNull(Node_Index[Name_Index]);
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