using Godot;
using My_Csharp_Node;
using System;
using System.Threading.Tasks;
namespace UIObject;
public partial class LevelUi : CanvasLayer
{
	Tween GetTween = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		show_Select_CardUI();
		DEBUG.Info.Print(Game.Get_GlobalNode.Node_Data.Get_Node<Control>("1"));
	}
	/// <summary>
	/// 显示选卡UI
	/// </summary>
	public void show_Select_CardUI()
	{
		DEBUG.Info.Print(Touch.Touch_Index.TouchIndex);
		DEBUG.Info.Print(Touch.Touch_Index.TouchIndex_Enable);
		if (GetTween != null){GetTween.Kill();}
		GetNode<Control>("Select_CardUI").Visible = true;
		Tween sd = CreateTween();
		GetTween = sd;
		GetNode<Control>("Select_CardUI").Modulate = new Color(0,0,0,0);
		GetNode<Control>("Select_CardUI").OffsetTransformPosition = new Godot.Vector2(0,500);
		sd.TweenProperty(GetNode<Control>("Select_CardUI"),new NodePath(Control.PropertyName.Modulate),new Color(1,1,1,1),0.75).SetTrans(Tween.TransitionType.Cubic);
		sd.Parallel().TweenProperty(GetNode<Control>("Card_Slot"),new NodePath(Control.PropertyName.OffsetTransformPosition),new Godot.Vector2(0,0),0.75).SetTrans(Tween.TransitionType.Quad);
		sd.Parallel().TweenProperty(GetNode<Control>("Select_CardUI"),new NodePath(Control.PropertyName.OffsetTransformPosition),new Godot.Vector2(0,0),0.75).SetTrans(Tween.TransitionType.Quad);
	}
	/// <summary>
	/// 隐藏选卡UI
	/// </summary>
	public async Task<bool> hide_Select_CardUI()
	{
		Touch.Touch_Index.Set_Index_Enable(0,true);
		DEBUG.Info.Print(Touch.Touch_Index.TouchIndex);
		DEBUG.Info.Print(Touch.Touch_Index.TouchIndex_Enable);
		if (GetTween != null){GetTween.Kill();}
		Tween sd = CreateTween();
		GetTween = sd;
		sd.TweenProperty(GetNode<Control>("Select_CardUI"),new NodePath(Control.PropertyName.OffsetTransformPosition),new Godot.Vector2(0,900),0.75).SetTrans(Game.TweenType.GetTweenType(Game.TweenType.Twee.三次));
		await ToSignal(sd,Tween.SignalName.Finished);
		GetNode<Control>("Level").Visible = true;
		GetNode<Control>("Select_CardUI").Visible = false;
		return true;
	}
	public void Card_Initialization()
	{
		foreach (GameUI.Card card in GetNode("Card").GetChildren())
		{
			card.Stop_While = true;
			card.Card_Mode = GameUI.Card.Mode.Gameing;
		}
	}
}
