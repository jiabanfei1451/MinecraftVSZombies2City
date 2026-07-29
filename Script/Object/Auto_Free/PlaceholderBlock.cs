using Godot;
using System;

public partial class PlaceholderBlock : Node
{
	public override void _Ready() {
		base._Ready();
		QueueFree();
	}
}
