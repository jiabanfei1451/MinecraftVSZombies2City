using Godot;
using System;
namespace Test;
public partial class Label : Godot.Label
{
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        Text = Performance.GetMonitor( Performance.Monitor.TimeFps).ToString();
    }
}
