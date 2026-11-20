using Godot;
using System;
using Touch;
namespace UIObject;

public partial class DragNode : TouchPad
{
	float DragY = 0;
	[Export] public Control Add_ChildYPositionNode = null;
    public override void _DragIng(TouchPad pad, Vector2 Event_Position, Vector2 Velocity)
    {
        base._DragIng(pad, Event_Position, Velocity);
		DragY = Velocity.Y * 0.02f;
	}
	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (Add_ChildYPositionNode != null)
		{
			foreach(Control Child in Add_ChildYPositionNode.GetChildren())
			{
				Child.Position = new Vector2(Child.Position.X,Child.Position.Y + DragY);
			}
			DragY *= 0.9f;
		}
	}
}
