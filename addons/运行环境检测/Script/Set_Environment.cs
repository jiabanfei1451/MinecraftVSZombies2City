using Godot;
using System;

public partial class Set_Environment : Node
{
    public override void _Ready() {
        base._Ready();
        GetTree().Root.GetNode("Start_Environment").Set("Start_Environment","CSharp");
    }
}
