using Godot;
using System;
using System.Linq;
namespace Game;
static partial class Temp_Node : Object
{
	public static Godot.Collections.Array<Godot.Collections.Array> nodes = new Godot.Collections.Array<Godot.Collections.Array>([[],[]]);
	
	public static int Get_NodeIndex(String Name)
	{
		Godot.Collections.Array Strings = (Godot.Collections.Array)nodes[1];
		int dex = Strings.IndexOf(Name);
		if (dex == -1)
		{
			DEBUG.Info.PrintErr("Node is Null");
		}
		return dex;
	}
	public static void Add_Node(Node @Node = null,String Name = "Object")
	{
		nodes[0].Add(@Node);
		Godot.Collections.Array Strings = (Godot.Collections.Array)nodes[1];
		int dex = Strings.IndexOf(Name);
		if (dex == -1)
		{
			nodes[1].Add(Name);
		}
		else
		{
			nodes[1].Add(Name + dex.ToString());
		}
	}
	public static void Clear_Node()
	{
		nodes[1].Clear();
		nodes[0].Clear();
	}
}
