using Godot;
using System;
using System.Threading.Tasks;

public partial class QueneLine : Label
{
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		Vector2 s = Size;
		Set(PropertyName.CustomMaximumSize,new Vector2(0,20));
		Set(PropertyName.Modulate,new Color(0,0,0,0));
		CreateTween().TweenProperty(this,new NodePath(Label.PropertyName.Modulate),new Color(1,1,1,1),1).SetTrans(Tween.TransitionType.Sine);
		CreateTween().TweenProperty(this,new NodePath(Label.PropertyName.CustomMaximumSize),s,1).SetTrans(Tween.TransitionType.Sine);
		await Task.Delay(10000);
		Set(PropertyName.CustomMaximumSize,Size);
		CreateTween().TweenProperty(this,new NodePath(Label.PropertyName.Modulate),new Color(0,0,0,0),1).SetTrans(Tween.TransitionType.Sine);
		CreateTween().TweenProperty(this,new NodePath(Label.PropertyName.CustomMaximumSize),new Vector2(0,17),1).SetTrans(Tween.TransitionType.Sine);
		await Task.Delay(1500);
		QueueFree();
	}
}
