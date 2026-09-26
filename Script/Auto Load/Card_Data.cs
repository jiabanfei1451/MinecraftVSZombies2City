using System;
using SaveData;
using Data;
using GameUI;
using Godot;
namespace Game;
/// <summary>
/// 卡槽数据
/// </summary>
public partial class Card_Data : Node
{
	[Signal] public delegate void Selected_ChangeEventHandler(Card card);


	[Export] public Godot.Collections.Array<Godot.Collections.Array> Data = new Godot.Collections.Array<Godot.Collections.Array>()
	{
	// 源器械读取0
	new Godot.Collections.Array(){},
	// 消耗1
	new Godot.Collections.Array(){},
	// 冷却2
	new Godot.Collections.Array(){},
	// 第一次冷却减免3
	new Godot.Collections.Array(){},
	// 展示坐标偏移4
	new Godot.Collections.Array(){},
	// 缩放5
	new Godot.Collections.Array(){},
	// 光标坐标偏移6
	new Godot.Collections.Array(){},
	// 地图坐标偏移7
	new Godot.Collections.Array(){},
	// 地图大小8
	new Godot.Collections.Array(){},
	// 物体类型9
	new Godot.Collections.Array(){},
	// 依赖ID10
	new Godot.Collections.Array(){},
	// 依赖标签11
	new Godot.Collections.Array(){},
	// 放置在依赖之上是否自动销毁12
	new Godot.Collections.Array(){},
	};
	/// <summary>
	/// 已选择卡槽的剩余冷却
	/// </summary>
	[Export] public Godot.Collections.Array<float> Selected_CD = new Godot.Collections.Array<float>()
	{};
	/// <summary>
	/// 选卡物体原数据
	/// </summary>
	[Export] public Godot.Collections.Array Selected = [new Node(),new Node(),new Node(),new Node(),new Node(),new Node(),new Node(),new Node(),new Node(),new Node()];
	/// <summary>
	/// 选择卡的原数据
	/// </summary>
	[Export] public Card Selected_raw_Object = null;
	/// <summary>
	/// 当前卡槽的原数据
	/// </summary>
	GameUI.Card Current_Selected_raw_Object = null;
	/// <summary>
	/// 已拥有的器械
	/// </summary>
	[Export] public Godot.Collections.Array<int> Obtained_Data = new Godot.Collections.Array<int>(){0,1,2,3,4,5};
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		if (Current_Selected_raw_Object != Selected_raw_Object)
		{
			Current_Selected_raw_Object = Selected_raw_Object;
			EmitSignal("Selected_Change",Current_Selected_raw_Object);
		}
    }

	public override void _Ready() {
		base._Ready();
		Initialization();
		#region 增加器械数据
		Add_Data(Game.ResourceScene.LoadScene("res://Object/Equipment/Transmitter.tscn"),100,7.5f,3,new Vec(2,2),new Vec(64,87));
		Add_Data(Game.ResourceScene.LoadScene("res://Object/Equipment/Furnace.tscn"),50,7.5f,7.5f,new Vec(2,2),new Vec(72,87));
		Add_Data(Game.ResourceScene.LoadScene("res://Object/Monster/Zombies.tscn"),0,0,0,new Vec(2,2),new Vec(80,186),null,new Vec(40,70f));
		Add_Data(Game.ResourceScene.LoadScene("res://Object/Equipment/Obsidian.tscn"),50,30,15,new Vec(2,2),new Vec(73,94));
		Add_Data(Game.ResourceScene.LoadScene("uid://7b6d3hect1in"),0,0,0,new Vec(2,2),new Vec(64,87));
		Add_Data(Game.ResourceScene.LoadScene("res://Object/Equipment/好友/HuanChong152.tscn"),0,0,0,new Vec(2,2),new Vec(64,87),null,null,null, MVZ2.Type.ObjectType.Normal,null,new Array_String(["给给"]),true);
		Add_Data(Game.ResourceScene.LoadScene("uid://bokxlltcu2pxm"),0,0,0,new Vec(2,2),new Vec(64,87));
		Add_Data(Game.ResourceScene.LoadScene("uid://bcfm88f3tbgt8"),0,0,0,new Vec(2,2),new Vec(64,87));
		#endregion
	}
	public void CD_Initialization()
	{
		Selected_CD.Resize(Data[0].Count);
		for (int i = 0 ; i < Selected_CD.Count; i++)
		{
			Selected_CD[i] = 0;
		}
	}
	/// <summary>
	/// 初始化
	/// </summary>
	public void Initialization()
	{
		Selected_raw_Object = null;
		Selected.Clear();
		for (int Count = 0;Count < Game.Static.PlayerData.Card_Quantity; Count++)
		{
			Control New_Null = new Control();
			New_Null.Name = "Null";
			New_Null.QueueFree();
			Selected.Add(New_Null);
		}
	}
	/// <summary>
	/// 获取卡槽索引
	/// </summary>
	/// <param name="Node"></param>
	/// <returns></returns>
	public int Get_Card_Index(Control @Node = null)
	{
		int Index = -1;
		Index = Selected.IndexOf(@Node);
		return Index;
	}
	/// <summary>
	/// 移除卡槽
	/// </summary>
	/// <param name="node"></param>
	public void Remove_Card_Index(Control @node = null)
	{
		if (@node == null){return;}
		Selected.Remove(@node);
		arrange_Card();
	}
	/// <summary>
	/// 添加卡槽
	/// </summary>
	/// <param name="node"></param>
	public void Add_Card_Index(Control @node = null)
	{
		if (@node == null){return;}
		Selected.Add(@node);
		arrange_Card();
	}
	/// <summary>
	/// 排列
	/// </summary>
	public int Get_Selected_Card_Len()
	{
		int dex = -1;
		foreach(Control control in Selected)
		{
			if(control != null)
			{
				dex += 1;
			}
		}
		return dex;
	}
	/// <summary>
	/// 排列卡槽
	/// </summary>
	public void arrange_Card()
	{
		Godot.Collections.Array variants = [];
		foreach (Control node in Selected)
		{
			if (node != null)
			{
				variants.Add(node);
			}
		}
		for (int Length = variants.Count;Length < Game.Static.PlayerData.Card_Quantity; Length++)
		{
			Control New_Null = new Control();
			New_Null.QueueFree();
			variants.Add(New_Null);
		}
		Selected = variants;
	}
	#region 数据管理
	/// <summary>
	/// 获取卡槽数据
	/// </summary>
	/// <param name="Index"></param>
	/// <returns></returns>
		/// <summary>
	/// 添加数据
	/// </summary>
	/// <param name="Scene">物体</param>
	/// <param name="sonsume">消耗</param>
	/// <param name="CD">冷却</param>
	/// <param name="RemoveCD">游戏开始时减少冷却</param>
	/// <param name="Scale">大小</param>
	/// <param name="Offset">材质偏移</param>
	/// <param name="Map_Scale">地图大小</param>
	/// <param name="Map_Offset">地图偏移</param>
	/// <param name="Mouse_Offset">鼠标偏移</param>
	/// <param name="Object_Type">物体类型</param>
	/// <param name="Reliant_UUID">依赖ID</param>
	/// <param name="Reliant_Tag">依赖标签</param>
	/// <param name="Auto_Free">放置在依赖之上是否自动销毁</param>
	public void Add_Data(
		PackedScene Scene = null,
		int sonsume = 0,
		float CD = 0,
		float RemoveCD = 0,
		Vec Scale = null,
		Vec Offset = null,
		Vec Mouse_Offset = null,
		Vec Map_Offset = null,
		Vec Map_Scale = null,
		MVZ2.Type.ObjectType Object_Type = MVZ2.Type.ObjectType.Normal,
		SaveData.Array_String Reliant_UUID = null,
		SaveData.Array_String Reliant_Tag = null,
		bool Auto_Free = false
	){
		Godot.Vector2 New_Scale = new Godot.Vector2(2,2);
		Godot.Vector2 New_Offset = new Godot.Vector2(64,87);
		Godot.Vector2 New_Mouse_Offset = Godot.Vector2.Zero;
		Godot.Vector2 New_Map_Offset = new Godot.Vector2(40,48);
		Godot.Vector2 New_Map_Scale = new Godot.Vector2(1,1);
		Godot.Collections.Array<String> New_Reliant_UUID = new(){};
		Godot.Collections.Array<String> New_Reliant_Tag = new(){};
		if (Scale != null){
			New_Scale = new Godot.Vector2(Scale.X,Scale.Y);
		}
		if (Offset != null){
			New_Offset = new Godot.Vector2(Offset.X,Offset.Y);
		}
		if (Mouse_Offset != null){
			New_Mouse_Offset = new Godot.Vector2(Mouse_Offset.X,Mouse_Offset.Y);
		}
		if (Map_Offset != null){
			New_Map_Offset = new Godot.Vector2(Map_Offset.X,Map_Offset.Y);
		}
		if (Map_Scale != null)
		{
			New_Map_Scale = new Godot.Vector2(Map_Scale.X,Map_Scale.Y);
		}
		if (Reliant_UUID != null)
		{
			New_Reliant_UUID = Reliant_UUID.Variants;
		}
		if (Reliant_Tag != null)
		{
			New_Reliant_Tag = Reliant_Tag.Variants;
		}
		Data[0].Add(Scene);
		Data[1].Add(sonsume);
		Data[2].Add(CD);
		Data[3].Add(RemoveCD);
		Data[4].Add(New_Offset);
		Data[5].Add(New_Scale);
		Data[6].Add(New_Mouse_Offset);
		Data[7].Add(New_Map_Offset);
		Data[8].Add(New_Map_Scale);
		Data[9].Add((int)Object_Type);
		Data[10].Add(New_Reliant_UUID);
		Data[11].Add(New_Reliant_Tag);
		Data[12].Add(Auto_Free);
	}
	#endregion
	/// <summary>
	/// 获取索引数据
	/// </summary>
	/// <param name="Index"></param>
	/// <returns></returns>
	public Data.GlobalData Get_CardData(int Index)
	{
		if (Index > -1)
		{
			PackedScene scene = (PackedScene)Data[0][Index];
			short sonsume = (short)Data[1][Index];
			float cd = (float)Data[2][Index];
			float removeCD = (float)Data[3][Index];
			Godot.Vector2 offset = (Godot.Vector2)Data[4][Index];
			Godot.Vector2 scale = (Godot.Vector2)Data[5][Index]; 
			Godot.Vector2 mouse_Offset = (Godot.Vector2)Data[6][Index];
			Godot.Vector2 map_Offset = (Godot.Vector2)Data[7][Index];
			Godot.Vector2 map_Scale = (Godot.Vector2)Data[8][Index];
			MVZ2.Type.ObjectType object_Type = GlobalData.GetObjectType((int)Data[9][Index]);
			Godot.Collections.Array<String> reliant_UUID = (Godot.Collections.Array<String>)Data[10][Index];
			Godot.Collections.Array<String> reliant_Tag = (Godot.Collections.Array<String>)Data[11][Index];
			bool auto_Free_Reliant = (bool)Data[12][Index];
			
			Data.GlobalData Back = new Data.GlobalData()
			{
				Scene = scene,
				CD = cd,
				Sonsume = sonsume,
				First_Time_RemoveCD = removeCD,
				Offset = offset,
				Scale = scale,
				Mouse_Offset = mouse_Offset,
				Map_Offset = map_Offset,
				Map_Scale = map_Scale,
				Object_Type = object_Type,
				Reliant_UUID = reliant_UUID,
				Reliant_Tag = reliant_Tag,
				Auto_Free_Reliant = auto_Free_Reliant
			};
			return Back;
		}
		else{return GlobalData.Zero();}
	}
	/// <summary>
	/// 场景搜索方法
	/// </summary>
	/// <param name="Index"></param>
	/// <returns></returns>
	public Data.GlobalData Get_CardData(PackedScene Index)
	{
		return Get_CardData(Data[0].IndexOf(Index));
	}
	#region Class
	/// <summary>
	/// 卡槽贴图实例
	/// </summary>
	/// <param name="Texture_PC">电脑端卡槽背景贴图</param>
	/// <param name="Texture_Border_PC_Focus">电脑端卡槽选中边框贴图</param>
	/// <param name="Texture_Border_PC">电脑端卡槽边框贴图</param>
	/// <param name="Texture_PE">手机端卡槽背景贴图</param>
	/// <param name="Texture_Border_PE_Focus">手机端卡槽选中边框贴图</param>
	/// <param name="Texture_Border_PE">手机端卡槽边框贴图</param>
	public class Card_Texture(Image @Texture_PC,Image @Texture_Border_PC,Image @Texture_Border_PC_Focus,Image @Texture_PE,Image @Texture_Border_PE,Image @Texture_Border_PE_Focus)
	{
		/// <summary>
		/// 电脑端卡槽背景贴图
		/// </summary>
		Image Texture_PC = @Texture_PC;
		/// <summary>
		/// 电脑端卡槽选中边框贴图
		/// </summary>
		Image Texture_Border_PC_Focus = @Texture_Border_PC_Focus;
		/// <summary>
		/// 电脑端卡槽边框贴图
		/// </summary>
		Image Texture_Border_PC = @Texture_Border_PC;
		/// <summary>
		/// 手机端卡槽背景贴图
		/// </summary>
		Image Texture_PE = @Texture_PE;
		/// <summary>
		/// 手机端卡槽选中边框贴图
		/// </summary>
		Image Texture_Border_PE_Focus = @Texture_Border_PE_Focus;
		/// <summary>
		/// 手机端卡槽边框贴图
		/// </summary>
		Image Texture_Border_PE = @Texture_Border_PE;
	}

	#endregion
}
