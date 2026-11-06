using Godot;
using System;
namespace Game;
public partial class InputPosition : Node
{
    public Godot.Vector2 Global_Position = new(0,0);
    public Godot.Vector2 Position = new(0,0);
    public override void _Input(InputEvent @event) {
        base._Input(@event);
        if (@event is InputEventMouseMotion)
        {
            Global_Position = ((InputEventMouseMotion)@event).GlobalPosition;
            Position = ((InputEventMouseMotion)@event).Position;
        }
        if (@event is InputEventScreenTouch)
        {
            Position = ((InputEventScreenTouch)@event).Position;
        }
        if (@event is InputEventScreenDrag)
        {
            Position = ((InputEventScreenDrag)@event).Position;
        }
        
    }
}
