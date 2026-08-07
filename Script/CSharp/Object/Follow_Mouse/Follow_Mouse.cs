using Godot;
using System;

public partial class Follow_Mouse : Control
{
    public override void _Process(double delta) {
        base._Process(delta);
        Position = GetGlobalMousePosition();
    }
}
