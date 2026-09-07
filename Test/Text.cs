using Godot;
using System;

public partial class Text : Node2D
{
    public override void _Ready() {
        base._Ready();
        GD.Print("sdsdasd"[4..]);
    }
}
