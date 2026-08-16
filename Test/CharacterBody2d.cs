using Godot;
using System;
using System.Threading.Tasks;

namespace Test;
public partial class CharacterBody2d : CharacterBody2D
{
    public override async void _Ready() {
        base._Ready();
        await Task.Delay(5000);
        QueueFree();
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        Position = GetGlobalMousePosition();
    }
}
