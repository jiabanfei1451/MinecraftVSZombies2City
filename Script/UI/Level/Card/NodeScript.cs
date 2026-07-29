using Game;
using Godot;
using System;
using System.Threading.Tasks;
namespace UIObject{
public partial class NodeScript : Control
{
	PackedScene BorderScene = GD.Load<PackedScene>("uid://bxk215b41db4p");
	PackedScene CardScene = GD.Load<PackedScene>("uid://c2y62prxcbege");
	public override async void _Ready()
	{
		var it = Summand_Node();
		await it;
		Summand_Card();
	}
	public async Task<bool> Summand_Node()
	{
		// Temp_Variant
		
		foreach (Node Objects in GetNode<GridContainer>("Global_Position_Index").GetChildren())
		{
			Objects.QueueFree();
		}
		int len = (Get_GlobalNode.Get_Card_Data(GetTree()).Data[0].Count);
		for (int Index = 0;Index < len; Index++)
		{
			// 实例化
			Control BorderInstantiate = BorderScene.Instantiate<Control>();
			BorderInstantiate.Name = Index.ToString();
			GetNode<GridContainer>("Global_Position_Index").AddChild(BorderInstantiate);
		}
		await Task.Delay(1);
		return true;
	}
	public async void Summand_Card()
	{
		foreach (Control node in GetNode<Control>("Global_Position_Index").GetChildren())
		{
			UIObject.Card cardObject = CardScene.Instantiate<UIObject.Card>();
			cardObject.Position = node.Position;
			cardObject.Scale = new Vector2(0.5f,0.5f);
			cardObject.Name = "Card" + node.Name;
			GetNode<Control>("Card").AddChild(cardObject);
		}
	}
}
}