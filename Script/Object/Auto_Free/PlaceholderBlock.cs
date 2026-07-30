using Godot;
using System;
namespace Namess{
namespace s{
public partial class PlaceholderBlock : Node
{
	public override void _Ready() {
		base._Ready();
		QueueFree();
	}
}
}
}