using Game;
using Godot;
using Touch;
using GameUI;
using System.Threading.Tasks;
namespace UIObject{
public partial class NodeScript : Control
{
	float is_Y = 0;
	float m = 1;
	Vector2 pos;
	PackedScene BorderScene = Game.ResourceTool.LoadScene("uid://bxk215b41db4p");
	PackedScene CardScene = Game.ResourceTool.LoadScene("uid://c2y62prxcbege");
	public override async void _Ready()
	{
		GetNode<TouchPad>("TouchPad").Drag_Ing += Add_YPosition;
		var it = Summand_Node();
		await it;
		Summand_Card();
		pos = GetNode<Control>("Card").Position;
	}
	public override void _PhysicsProcess(double delta) {
		CreateTween().TweenProperty(GetNode<Control>("Card"),new NodePath(Control.PropertyName.Position),pos + new Vector2(0,is_Y),0.2);
		if (is_Y > 0)
		{
			is_Y -= (60 * (float)delta) * m;
			m += 1f;
		}else
		{m = 1;}
	}	

	public void Add_YPosition(TouchPad pad,Godot.Vector2 Event_Potition,Godot.Vector2 Velocity)
	{
		is_Y += Velocity.Y * 0.01f;
	}
	/// <summary>
	/// 生成节点
	/// </summary>
	/// <returns></returns>
	public async Task<bool> Summand_Node()
	{
		// Temp_Variant
		
		foreach (Node Objects in GetNode<GridContainer>("Global_Position_Index").GetChildren())
		{
			Objects.QueueFree();
		}
		int len = (Get_GlobalNode.Get_Card_Data(GetTree()).Obtained_Data.Count);
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
	/// <summary>
	/// 生成卡槽
	/// </summary>
	public void Summand_Card()
	{
		int dex = 0;
		foreach (Control node in GetNode<Control>("Global_Position_Index").GetChildren())
		{
			Card cardObject = CardScene.Instantiate<Card>();
			cardObject.Position = node.Position;
			cardObject.Card_Index = Game.Get_GlobalNode.Get_Card_Data(GetTree()).Obtained_Data[dex];
			cardObject.Scale = new Vector2(0.5f,0.5f);
			cardObject.Name = "Card" + node.Name;
			GetNode<Control>("Card").AddChild(cardObject);
			dex += 1;
		}
	}
}
}