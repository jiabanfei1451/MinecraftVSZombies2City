using Godot;
using System;

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
	};
}
