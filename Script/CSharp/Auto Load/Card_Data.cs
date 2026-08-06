using Godot;
namespace Game;
public partial class Card_Data : Node
{
	[Export] public Godot.Collections.Array<Godot.Collections.Array> Data = new Godot.Collections.Array<Godot.Collections.Array>()
	{
	// 源器械读取
	new Godot.Collections.Array(){},
	// 消耗
	new Godot.Collections.Array(){},
	// 冷却
	new Godot.Collections.Array(){},
	// 开局减免
	new Godot.Collections.Array(){},
	// 已选定
	new Godot.Collections.Array(){new Node(),new Node(),new Node(),new Node(),new Node(),new Node(),new Node(),new Node(),new Node(),new Node()},
	// 坐标偏移
	new Godot.Collections.Array(){},
	// 缩放
	new Godot.Collections.Array(){}
	};
	[Export] public Godot.Collections.Array<int> Obtained_Data = new Godot.Collections.Array<int>(){0,1,2,3,4,5};
	public override void _Ready() {
		base._Ready();
		Initialization();
		#region 增加器械数据
		Add_Data(GD.Load<PackedScene>("uid://du3y4377ebqq2"),100,7.5f,3,new Vec(2,2),new Vec(64,87));
		Add_Data(GD.Load<PackedScene>("uid://dhqc163eiuqrd"),0,0,0,new Vec(2,2),new Vec(64,87));
		Add_Data(GD.Load<PackedScene>("uid://7b6d3hect1in"),0,0,0,new Vec(2,2),new Vec(64,87));
		Add_Data(GD.Load<PackedScene>("uid://djytruxu3c3qt"),0,0,0,new Vec(2,2),new Vec(64,87));
		Add_Data(GD.Load<PackedScene>("uid://bokxlltcu2pxm"),0,0,0,new Vec(2,2),new Vec(64,87));
		Add_Data(GD.Load<PackedScene>("uid://bcfm88f3tbgt8"),0,0,0,new Vec(2,2),new Vec(64,87));
		#endregion
	}
	/// <summary>
	/// 添加数据
	/// </summary>
	/// <param name="Scene">物体</param>
	/// <param name="sonsume">消耗</param>
	/// <param name="CD">冷却</param>
	/// <param name="RemoveCD">游戏开始时减少冷却</param>
	/// <param name="Scale">大小</param>
	/// <param name="Offset">材质偏移</param>
	public void Add_Data(PackedScene Scene = null,int sonsume = 0,float CD = 0,float RemoveCD = 0,Vec Scale = null,Vec Offset = null)
	{
		Godot.Vector2 New_Scale = new Godot.Vector2(Scale.X,Scale.Y);
		Godot.Vector2 New_Offset = new Godot.Vector2(Offset.X,Offset.Y);
		Data[0].Add(Scene);
		Data[1].Add(sonsume);
		Data[2].Add(CD);
		Data[3].Add(RemoveCD);
		Data[5].Add(New_Offset);
		Data[6].Add(New_Scale);
	}
	/// <summary>
	/// 初始化
	/// </summary>
	public void Initialization()
	{
		Data[4].Clear();
		for (int Count = 0;Count < Game.PlayerData.Card_Quantity; Count++)
		{
			Control New_Null = new Control();
			New_Null.Name = "Null";
			New_Null.QueueFree();
			Data[4].Add(New_Null);
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
		Index = Data[4].IndexOf(@Node);
		return Index;
	}
	/// <summary>
	/// 移除卡槽
	/// </summary>
	/// <param name="node"></param>
	public void Remove_Card_Index(Control @node = null)
	{
		if (@node == null){return;}
		Data[4].Remove(@node);
		arrange_Card();
	}
	/// <summary>
	/// 添加卡槽
	/// </summary>
	/// <param name="node"></param>
	public void Add_Card_Index(Control @node = null)
	{
		if (@node == null){return;}
		Data[4].Add(@node);
		arrange_Card();
	}
	/// <summary>
	/// 排列
	/// </summary>
	public int Get_Selected_Card_Len()
	{
		int dex = -1;
		foreach(Control control in Data[4])
		{
			if(control != null)
			{
				dex += 1;
			}
		}
		return dex;
	}
	public void arrange_Card()
	{
		Godot.Collections.Array variants = [];
		foreach (Control node in Data[4])
		{
			if (node != null)
			{
				GD.Print("Null");
				variants.Add(node);
			}
		}
		for (int Length = variants.Count;Length < Game.PlayerData.Card_Quantity; Length++)
		{
			Control New_Null = new Control();
			New_Null.QueueFree();
			variants.Add(New_Null);
		}
		GD.Print("TempData:",variants,"Data:",Data[4]);
		Data[4] = variants;
	}
	/// <summary>
	/// 获取卡槽数据
	/// </summary>
	/// <param name="Index"></param>
	/// <returns></returns>
	public BackData Get_CardData(int Index)
	{
		if (Index > -1)
		{
			PackedScene scene = (PackedScene)Data[0][Index];
			int consume = (int)Data[1][Index];
			float CD = (float)Data[2][Index];
			float RemoveCD = (float)Data[3][Index];
			Godot.Vector2 Offset = (Godot.Vector2)Data[5][Index];
			Godot.Vector2 Scale = (Godot.Vector2)Data[6][Index]; 
			BackData Back = new BackData(scene,consume,CD,RemoveCD,Scale,Offset);
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
	/// <param name="Scene"></param>
	/// <param name="sonsume"></param>
	/// <param name="CD"></param>
	/// <param name="RemoveCD"></param>
	/// <param name="Scale"></param>
	/// <param name="Offset"></param>
	public class BackData(PackedScene @Scene,int @sonsume,float @CD,float @RemoveCD,Godot.Vector2 @Scale,Godot.Vector2 @Offset)
	{
		/// <summary>
		/// 物体场景
		/// </summary>
		public PackedScene Scene = @Scene;
		/// <summary>
		/// 消耗
		/// </summary>
		public int Sonsume = @sonsume;
		/// <summary>
		/// 冷却
		/// </summary>
		public float CD = @CD;
		/// <summary>
		/// 游戏开始时减少冷却
		/// </summary>
		public float RemoveCD = @RemoveCD;
		/// <summary>
		/// 材质大小
		/// </summary>
		public Godot.Vector2 Scale = @Scale;
		/// <summary>
		/// 材质偏移
		/// </summary>
		public Godot.Vector2 Offset = @Offset;
	}
	#endregion
}
