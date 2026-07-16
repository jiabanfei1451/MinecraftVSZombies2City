using Godot;
using System;

public partial class Level_Script : Node2D
{
	[Export] public Godot.Collections.Array<Godot.Collections.Array<int>> 草坪数组 = new Godot.Collections.Array<Godot.Collections.Array<int>>()
	{
		new Godot.Collections.Array<int>(){0,0,0,0,0,0,0,0,0},
		new Godot.Collections.Array<int>(){0,0,0,0,0,0,0,0,0},
		new Godot.Collections.Array<int>(){0,0,0,0,0,0,0,0,0},
		new Godot.Collections.Array<int>(){0,0,0,0,0,0,0,0,0},
		new Godot.Collections.Array<int>(){0,0,0,0,0,0,0,0,0},
	};
	[Export] public Godot.Collections.Array<ColorRect> 草坪数据;
	[Export] public bool 自动生成草坪 = true;
	[Export] public Godot.Vector2 生成偏移;
}
