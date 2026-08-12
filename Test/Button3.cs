using Godot;
using System;

public partial class Button3 : Button
{
    public override void _Ready() {
        base._Ready();
        Pressed += pressed;
    }
    public void pressed()
    {
    }
}
