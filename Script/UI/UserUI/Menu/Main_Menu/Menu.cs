using Godot;
using PVZEngine;
using System;
using Touch;
namespace UIObject;
public partial class Menu : CanvasLayer
{
	Tween Tw = null;
	[Export] public Label VersionLabel = null;
	[Export] public Control SelectedNode = null;
	[Export] public Control MouseNode = null;
	[Export] public Godot.Collections.Array<Touch.TouchPad> AddSignalNode = new();
	public override void _Ready() {
		base._Ready();
		VersionLabel.Text = "Version:" + ProjectSettings.GetSetting("application/config/version").ToString();
		foreach(TouchPad d in AddSignalNode)
		{
			d.Focus_Join += Join;
			d.Button_Pressed += Pressed;
		}
	}
	public void Pressed(TouchPad pad,Godot.Vector2 Event_Position)
	{
		switch (pad.Name)
		{
			case "Exit Game":
				GetTree().Quit();
				break;
			case "Test Game":
				GetTree().ChangeSceneToPacked(ResourceScene.LoadScene("uid://cff7if8145pjk"));
				break;
		}
	}
	public void Join(TouchPad pad,Godot.Vector2 Event_Position)
	{
		SelectedNode = pad;
	}
	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (Tw is null)
		{
			Tw = CreateTween();
		}
		else
		{
			Tw.Kill();
			Tw = CreateTween();
		}
		if (SelectedNode is null)
		{
			Tw.TweenProperty(MouseNode,new NodePath(Control.PropertyName.Size),new Vector2(20,20),0.2);
			Tw.Parallel().TweenProperty(MouseNode,new NodePath(Control.PropertyName.GlobalPosition),Game.Get_GlobalNode.InputPosition.Position,0.2);
		}
		else
		{
			Tw.TweenProperty(MouseNode,new NodePath(Control.PropertyName.Size),SelectedNode.Size * SelectedNode.Scale,0.2);
			Tw.Parallel().TweenProperty(MouseNode,new NodePath(Control.PropertyName.GlobalPosition),SelectedNode.GlobalPosition,0.2);
		}
	}
}
