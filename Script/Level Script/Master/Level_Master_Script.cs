using Godot;
using Level;
using System;
public partial class Level_Master_Script : Node2D{
	[ExportGroup("Variant")]
	[Export] public ColorRect Selected_Lawn;
	[Export] public Godot.Collections.Array<Godot.Collections.Array<int>> Lawn_Array = new Godot.Collections.Array<Godot.Collections.Array<int>>()
	{
		new Godot.Collections.Array<int>(){0,0,0,0,0,0,0,0,0},
		new Godot.Collections.Array<int>(){0,0,0,0,0,0,0,0,0},
		new Godot.Collections.Array<int>(){0,0,0,0,0,0,0,0,0},
		new Godot.Collections.Array<int>(){0,0,0,0,0,0,0,0,0},
		new Godot.Collections.Array<int>(){0,0,0,0,0,0,0,0,0},
	};
	[Export] public Godot.Collections.Array<Godot.Collections.Array<Godot.Vector2>> Lawn_Offset_Array = new Godot.Collections.Array<Godot.Collections.Array<Vector2>>([[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[],[]]);
	[Export] public Godot.Collections.Array<Godot.Collections.Array> Lawn_Data = new Godot.Collections.Array<Godot.Collections.Array>([[]]);
	[Export] public bool Auto_Spawn_Lawn = true;
	[Export] public Godot.Vector2 Lawn_Spawn_Offect = new Godot.Vector2(80,80);
	[Export] public Godot.Vector2 Lawn_Spawn_Position = new Godot.Vector2(-428,-181);
	[Export] public PackedScene LawnScene = GD.Load<PackedScene>("uid://dim8rk13omwvv");
	[ExportGroup("Get_Node")]
	[Export] public Node2D Lawn_Node;
	[ExportGroup("Light")]
	[Export] public float Light = 1;
	public override void _Ready() {
		base._Ready();
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
	public void Lawn_Green(ColorRect This)
	{
		This.Color = new Color(0,1,0,1);
		Selected_Lawn = This;
	}
	public void Lawn_Alpha(ColorRect This)
	{
		This.Color = new Color(0,0,0,0);
		Selected_Lawn = This;
	}
}