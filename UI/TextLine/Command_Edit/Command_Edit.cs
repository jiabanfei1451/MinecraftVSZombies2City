using System;
using Godot;
namespace AutoLoad;
public partial class Command_Edit : TextEdit
{
	/// <summary>
	/// 玩家发送信息事件
	/// </summary>
	[Signal] public delegate void Player_sendEventHandler(String Seed_Why);
	[Export] public bool SB = false;
	static PackedScene Line = Game.ResourceScene.LoadScene("uid://bd74mkx2jelk1");
	public override void _Ready() {
		base._Ready();
		Game.Get_GlobalNode.CommandEdit = this;
		return;
	}
	public override void _Input(InputEvent @event) {
		base._Input(@event);
		if (@event is InputEventKey)
		{
			InputEventKey Keys = (InputEventKey)@event;
			if (!Keys.Pressed && Keys.Keycode == Key.Enter)
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
					EmitSignal("Player_send",Lineinstantiate.Text);
					this.GetNode<BoxContainer>("../../TextLine").AddChild(Lineinstantiate);
					Position = new Vector2(0,0);
					Editable = false;
					CreateTween().TweenProperty(this.GetNode<ColorRect>("../../ColorRect"),new NodePath(ColorRect.PropertyName.Color),new Color(0,0,0,0),0.5f);
					CreateTween().TweenProperty(this,new NodePath(TextEdit.PropertyName.Position),new Vector2(0,30),0.5f).SetTrans(Tween.TransitionType.Quart);
				}
			}
		}
	}
}
