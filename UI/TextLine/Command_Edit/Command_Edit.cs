using Godot;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

public partial class Command_Edit : TextEdit
{
	[Export] public bool SB = false;
	static PackedScene Line = GD.Load<PackedScene>("uid://bd74mkx2jelk1");
	public override void _Input(InputEvent @event) {
		base._Input(@event);
		if (@event is InputEventKey)
		{
			InputEventKey Keys = (InputEventKey)@event;
			if (!Keys.Pressed && Keys.Keycode == Key.KpEnter)
			{
				if (SB == false)
				{
					SB = true;
					Position = new Vector2(0,30);
					Text = "";
					GrabFocus(true);
					Editable = true;
					CreateTween().TweenProperty(this.GetNode<ColorRect>("../../ColorRect"),new NodePath(ColorRect.PropertyName.Color),new Color(0,0,0,0.5f),0.5f);
					CreateTween().TweenProperty(this,new NodePath(TextEdit.PropertyName.Position),new Vector2(0,0),0.5f).SetTrans(Tween.TransitionType.Quart);
				}
				else
				{
					SB = false;
					Label Lineinstantiate = Line.Instantiate<Label>();
					Lineinstantiate.Text = "  " + "<" + "User" + ">:" + Text[0..(Text.Length - 1)] + " ";
					this.GetNode<BoxContainer>("../../TextLine").AddChild(Lineinstantiate);
					Position = new Vector2(0,0);
					this.GetNode<Control>("..").GrabFocus(false);
					Editable = false;
					CreateTween().TweenProperty(this.GetNode<ColorRect>("../../ColorRect"),new NodePath(ColorRect.PropertyName.Color),new Color(0,0,0,0),0.5f);
					CreateTween().TweenProperty(this,new NodePath(TextEdit.PropertyName.Position),new Vector2(0,30),0.5f).SetTrans(Tween.TransitionType.Quart);
				}
			}
		}
	}
}
