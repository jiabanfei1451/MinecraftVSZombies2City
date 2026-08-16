using System;
using GameUI;
using Godot;
namespace Game;
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
	new Godot.Collections.Array(){}
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
		Add_Data(GD.Load<PackedScene>("uid://du3y4377ebqq2"),100,7.5f,3,new Vec(2,2),new Vec(64,87));
		Add_Data(GD.Load<PackedScene>("uid://dhqc163eiuqrd"),78,0,0,new Vec(2,2),new Vec(64,87));
		Add_Data(GD.Load<PackedScene>("uid://7b6d3hect1in"),0,0,0,new Vec(2,2),new Vec(64,87));
		Add_Data(GD.Load<PackedScene>("uid://djytruxu3c3qt"),0,0,0,new Vec(2,2),new Vec(64,87));
		Add_Data(GD.Load<PackedScene>("uid://bokxlltcu2pxm"),0,0,0,new Vec(2,2),new Vec(64,87));
		Add_Data(GD.Load<PackedScene>("uid://bcfm88f3tbgt8"),0,0,0,new Vec(2,2),new Vec(64,87));
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
		for (int Count = 0;Count < Game.PlayerData.Card_Quantity; Count++)
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
		for (int Length = variants.Count;Length < Game.PlayerData.Card_Quantity; Length++)
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
	public void Add_Data(
		PackedScene Scene = null,
		int sonsume = 0,
		float CD = 0,
		float RemoveCD = 0,
		Vec Scale = null,
		Vec Offset = null,
		Vec Mouse_Offset = null,
		Vec Map_Offset = null,
		Vec Map_Scale = null
	){
		Godot.Vector2 New_Scale = new Godot.Vector2(2,2);
		Godot.Vector2 New_Offset = new Godot.Vector2(64,87);
		Godot.Vector2 New_Mouse_Offset = Godot.Vector2.Zero;
		Godot.Vector2 New_Map_Offset = new Godot.Vector2(40,48);
		Godot.Vector2 New_Map_Scale = new Godot.Vector2(1,1);
		if (Scale != null){
			New_Scale = new Godot.Vector2(Scale.X,Scale.Y);
		}
		if (Offset != null){
			New_Offset = new Godot.Vector2(Offset.X,Offset.Y);
		}
		if (Mouse_Offset != null){
			New_Mouse_Offset = new Godot.Vector2(Mouse_Offset.X,Mouse_Offset.Y);
		}
		if (Mouse_Offset != null){
			New_Map_Offset = new Godot.Vector2(Mouse_Offset.X,Map_Offset.Y);
		}
		if (Map_Scale != null)
		{
			New_Map_Scale = new Godot.Vector2(Map_Scale.X,Map_Scale.Y);
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
	}
	#endregion
	public GlobalData Get_CardData(int Index)
	{
		if (Index > -1)
		{
			PackedScene scene = (PackedScene)Data[0][Index];
			short sonsume = (short)Data[1][Index];
			float CD = (float)Data[2][Index];
			float RemoveCD = (float)Data[3][Index];
			Godot.Vector2 Offset = (Godot.Vector2)Data[4][Index];
			Godot.Vector2 Scale = (Godot.Vector2)Data[5][Index]; 
			Godot.Vector2 Mouse_Offset = (Godot.Vector2)Data[6][Index];
			Godot.Vector2 Map_Offset = (Godot.Vector2)Data[7][Index];
			Godot.Vector2 Map_Scale = (Godot.Vector2)Data[8][Index];
			GlobalData Back = new GlobalData(scene,sonsume,CD,RemoveCD,Scale,Offset,Mouse_Offset,Map_Offset,Map_Scale);
			return Back;
		}
		else{return null;}
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
	/// <summary>
	/// 坐标存储
	/// </summary>
	/// <param name="X"></param>
	/// <param name="Y"></param>
	public class Vec(float X,float Y)
	{
		public float X {get;set;} = X;
		public float Y {get;set;} = Y;
	}
	/// <summary>
	/// 返回数据
	/// </summary>
	/// <param name="Scene">场景</param>
	/// <param name="sonsume">消耗</param>
	/// <param name="CD">冷却</param>
	/// <param name="First_Time_RemoveCD">游戏开始时减少冷却</param>
	/// <param name="Scale">大小</param>
	/// <param name="Offset">偏移</param>
	/// <param name="Mouse_Offset">光标坐标偏移</param>
	/// <param name="Map_Offset">地图坐标偏移</param>
	/// <param name="Map_Scale">地图大小</param>
	public class GlobalData(
		PackedScene @Scene,
		short @sonsume,
		float @CD,
		float @First_Time_RemoveCD,
		Godot.Vector2 @Scale,
		Godot.Vector2 @Offset,
		Godot.Vector2 @Mouse_Offset,
		Godot.Vector2 @Map_Offset,
		Godot.Vector2 @Map_Scale
		)
	{

		/// <summary>
		/// 物体场景
		/// </summary>
		public PackedScene Scene = @Scene;
		/// <summary>
		/// 消耗
		/// </summary>
		public short Sonsume = @sonsume;
		/// <summary>
		/// 冷却时间
		/// </summary>
		public float CD = @CD;
		/// <summary>
		/// 首次冷却时减少冷却
		/// </summary>
		public float First_Time_RemoveCD = @First_Time_RemoveCD;
		/// <summary>
		/// 材质大小
		/// </summary>
		public Godot.Vector2 Scale = @Scale;
		/// <summary>
		/// 材质偏移
		/// </summary>
		public Godot.Vector2 Offset = @Offset;
		public Godot.Vector2 Mouse_Offset = @Mouse_Offset;
		public Godot.Vector2 Map_Offset = @Map_Offset;
		public Godot.Vector2 Map_Scale = @Map_Scale;
	}
	#endregion
}
