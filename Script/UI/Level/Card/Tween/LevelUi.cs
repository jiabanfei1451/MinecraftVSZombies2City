using Godot;
using System;
using System.Threading.Tasks;
namespace UIObject;
public partial class LevelUi : CanvasLayer
{
	Tween GetTween = null;
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		show_Select_CardUI();
		GD.Print(Game.Get_GlobalNode.Node_Data.Get_Node<Control>("1"));
	}
	/// <summary>
	/// 显示选卡UI
	/// </summary>
	public async void show_Select_CardUI()
	{
		if (GetTween != null){GetTween.Kill();}
		GetNode<Control>("Select_CardUI").Visible = true;
		Tween sd = CreateTween();
		GetTween = sd;
		GetNode<Control>("Select_CardUI").Modulate = new Color(0,0,0,0);
		GetNode<Control>("Select_CardUI").OffsetTransformScale = new Godot.Vector2(0.5f,0.5f);
		GetNode<Control>("Select_CardUI").OffsetTransformPosition = new Godot.Vector2(0,500);
		sd.TweenProperty(GetNode<Control>("Select_CardUI"),new NodePath(Control.PropertyName.Modulate),new Color(1,1,1,1),0.75).SetTrans(Tween.TransitionType.Cubic);
		sd.Parallel().TweenProperty(GetNode<Control>("Card_Slot"),new NodePath(Control.PropertyName.OffsetTransformPosition),new Godot.Vector2(0,0),0.75).SetTrans(Tween.TransitionType.Quad);
		sd.Parallel().TweenProperty(GetNode<Control>("Select_CardUI"),new NodePath(Control.PropertyName.OffsetTransformPosition),new Godot.Vector2(0,0),0.75).SetTrans(Tween.TransitionType.Quad);
		sd.Parallel().TweenProperty(GetNode<Control>("Select_CardUI"),new NodePath(Control.PropertyName.OffsetTransformScale),new Godot.Vector2(1,1),0.75).SetTrans(Tween.TransitionType.Back);
	}
	/// <summary>
	/// 隐藏选卡UI
	/// </summary>
	public async Task<bool> hide_Select_CardUI()
	{
		if (GetTween != null){GetTween.Kill();}
		Tween sd = CreateTween();
		GetTween = sd;
		sd.Parallel().TweenProperty(GetNode<Control>("Select_CardUI"),new NodePath(Control.PropertyName.OffsetTransformPosition),new Godot.Vector2(0,900),0.75).SetTrans(Tween.TransitionType.Quart);
		await ToSignal(sd,Tween.SignalName.Finished);
		GetNode<Control>("Select_CardUI").Visible = false;
		return true;
	}
}
