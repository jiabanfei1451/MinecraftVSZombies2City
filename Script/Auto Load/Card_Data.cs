using Godot;
using System;
namespace Game;
public partial class Card_Data : Node
{
	[Export] public Godot.Collections.Array<Godot.Collections.Array> Data = new Godot.Collections.Array<Godot.Collections.Array>()
	{
	// 源器械读取
	new Godot.Collections.Array(){GD.Load<PackedScene>("uid://bx76t0lp5w10a"),GD.Load<PackedScene>("uid://bx76t0lp5w10a"),GD.Load<PackedScene>("uid://bx76t0lp5w10a"),GD.Load<PackedScene>("uid://bx76t0lp5w10a")},
	// 消耗
	new Godot.Collections.Array(){0},
	// 冷却
	new Godot.Collections.Array(){0},
	// 开局减免
	new Godot.Collections.Array(){0},
	new Godot.Collections.Array(){new Node(),new Node(),new Node(),new Node(),new Node(),new Node(),new Node(),new Node(),new Node(),new Node(),}
	};
	public override void _Ready() {
		base._Ready();
		Initialization();
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
}
