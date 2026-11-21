using Godot;
using System;

public partial class VisibleOnScreenNotifier2d : VisibleOnScreenNotifier2D
{
	[Export] public Control SetNode = null;
	public override void _ExitTree() {
		base._ExitTree();
		ScreenEntered -= Entered;
		ScreenExited -= Exited;
	}
	public override void _Ready() {
		base._Ready();
		ScreenEntered += Entered;
		ScreenExited += Exited;
	}
	public void Entered()
	{
		SetNode.Visible = true;
	}
	public void Exited()
	{
		GD.Print(1);
		SetNode.Visible = false;
	}
}
