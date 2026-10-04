using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

public partial class Text : Node2D
{
    public override void _Ready() {
        base._Ready();
        PackedScene scene = GD.Load<PackedScene>("uid://lyb0noko5sk1");
        MVZ2.Object.Equipment.Equipment equipment = scene.Instantiate<MVZ2.Object.Equipment.Equipment>();
        GD.Print("CH:地雷出土"[3..]);
    }
}
